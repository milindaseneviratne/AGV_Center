using CommonLibraries.Extensions;
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
    public static class AsynchonousSocketListner
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public static Socket listner = null;

        public static void StartListning()
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

        private static void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();

            Socket listner = (Socket)ar.AsyncState;
            Socket handler = listner.EndAccept(ar);

            StateObject state = new StateObject();
            state.workSocket = handler;

            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(ReadCallback), state);
        }

        private static void ReadCallback(IAsyncResult ar)
        {
            try
            {
                string content = string.Empty;

                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;

                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                    content = state.sb.ToString();
                }

                if (content.IndexOf("<EOF>") > -1)
                {
                    // finished recieving. What should I do now?

                    // Send ACK
                    Send(handler, content);
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

        private static void Send(Socket handler, string content)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(content);

            handler.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
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
