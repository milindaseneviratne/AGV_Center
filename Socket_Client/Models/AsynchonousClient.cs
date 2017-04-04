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

namespace Socket_Client.Models
{
    public class AsynchonousClient
    {
        private const int port = 11000;

        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent recieveDone = new ManualResetEvent(false);

        private static string response = string.Empty;
        private static Socket client = null;

        public static void StartClient(string data, string server)
        {
            try
            {
                IPHostEntry ipHostEntry = Dns.GetHostEntry(server);
                IPAddress ipAddress = ipHostEntry.AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

               // while (true)
                //{
                    connectDone.Reset();
                    sendDone.Reset();
                    recieveDone.Reset();

                    client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
                    connectDone.WaitOne();

                    Send(client, data);
                    sendDone.WaitOne();

                    Recieve(client);
                    recieveDone.WaitOne();

                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                //}
            }
            catch (Exception e)
            {
                e.WriteLog().SaveToDataBase().Display();
            }
        }

        private static void Recieve(Socket client)
        {
            try
            {
                StateObject state = new StateObject();
                state.workSocket = client;

                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(RecieveCallback), state);
            }
            catch (Exception e)
            {
                e.WriteLog().SaveToDataBase().Display();
            }
        }

        private static void RecieveCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;

            int bytesRead = client.EndReceive(ar);

            if (bytesRead > 0)
            {
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(RecieveCallback), state);
            }
            else
            {
                if (state.sb.Length > 1)
                {
                    response = state.sb.ToString();
                }
                recieveDone.Set();
            }
        }

        private static void Send(Socket client, string data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            client.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(SendCallback), client);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                int bytesSent = client.EndSend(ar);
                sendDone.Set();
            }
            catch (Exception e)
            {
                e.WriteLog().SaveToDataBase().Display();
            }
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);
                connectDone.Set();
            }
            catch (Exception e)
            {
                e.WriteLog().SaveToDataBase().Display();
            }
        }
    }
}
