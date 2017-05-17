using CommonLibraries.Extensions;
using CommonLibraries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socket_Server.Models
{
    public class AsynchonousServer
    {
        private BarcodeDecoder bacrodeDecoder = new BarcodeDecoder();
        private VCSCommunicator vcsComm = new VCSCommunicator();
        private agvTaskCreator agvTaskCreator = new agvTaskCreator();

        public ManualResetEvent allDone = new ManualResetEvent(false);
        public Socket listner = null;
        public void StartListning()
        {
            byte[] bytes = new byte[1024];

            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostEntry.AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            listner = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listner.Bind(localEndPoint);
                listner.Listen(100);

                while (true)
                {
                    allDone.Reset();
                    listner.BeginAccept(new AsyncCallback(AcceptCallback), listner);
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                e.WriteLog().SaveToDataBase().Display();
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();

            Socket listner = (Socket)ar.AsyncState;
            Socket handler = listner.EndAccept(ar);

            StateObject state = new StateObject();
            state.workSocket = handler;

            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(ReadCallback), state);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                string recvievedBarcode = string.Empty;

                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;

                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                    recvievedBarcode = state.sb.ToString();
                }

                if (recvievedBarcode.IndexOf("<EOF>") > -1)
                {
                    // finished recieving. What should I do now?
                    var rawBarcode = recvievedBarcode.RemoveEOF();
                    var vcsCommand = bacrodeDecoder.GetVCSCommand(rawBarcode);

                    bool success = false;
                    if (vcsCommand.HasData()) success = vcsComm.SendCommand(vcsCommand);

                    bool taskCreated = false;
                    if (success) taskCreated = agvTaskCreator.CreateTask(vcsCommand);

                    if (success && taskCreated)
                    {
                        recvievedBarcode = (recvievedBarcode.RemoveEOF() + " OK").AddEOF();
                    }
                    else
                    {
                        recvievedBarcode = (recvievedBarcode.RemoveEOF() + " ERROR").AddEOF();
                    }

                    // Send ACK
                    Send(handler, recvievedBarcode);
                }
                else
                {
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
            }
            catch (Exception e)
            {
                e.WriteLog().SaveToDataBase().Display();
            }
        }

        private void Send(Socket handler, string content)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(content);

            handler.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(SendCallback), handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                e.WriteLog().SaveToDataBase().Display();
            }
        }
    }
}
