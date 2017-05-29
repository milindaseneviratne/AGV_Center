using CommonLibraries.Extensions;
using CommonLibraries.Models.dbModels;
using Communication;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries.Models
{
    public class VCSCommunicator
    {
        private agvTaskCreator agvTaskDequer = new agvTaskCreator();
        private CommunicationServer serverForVcs;
        private IPAddress IP = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(addresses => addresses.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
        private BlockingCollection<byte[]> _vcsRxQueue;
        private BlockingCollection<Barcode> _vcsTxStack;
        private int _vcsPort;

        public VCSCommunicator(BlockingCollection<byte[]> vcsRxQueue, BlockingCollection<Barcode> vcsTxQueue, string vcsIP = "", int vcsPort = 26000)
        {
            _vcsRxQueue = vcsRxQueue;
            _vcsTxStack = vcsTxQueue;
            _vcsPort = vcsPort;
        }

        public void SendVCSCommands()
        {
            while (true)
            {
                try
                {
                    //Looping here untill we get a barcode to decode.
                    //Barcode result;
                    //bool success = _vcsTxStack.TryDequeue(out result);
                    //if (!success) continue;
                    Barcode result = _vcsTxStack.Take();

                    //Convert Barcode to bytes.
                    byte[] message = ToBytes(result);
                    var messagetoSend = new Communication.Communication.IPandMessage(IP, message);

                    //Send Barcode to VCS.
                    serverForVcs.sendMessage(messagetoSend);

                    //Create a task entry in the SQL Server.
                    agvTaskDequer.CreateTask(result);
                }
                catch (Exception e)
                {
                    e.WriteLog().SaveToDataBase();
                    break;
                }
            }
        }

        public bool SendDequeueTaskCommand(agvTask task, agvStation_Info destination, int vcsPort = 26000)
        {
            try
            {
                byte[] message = ToBytes(task, destination);

                var messagetoSend = new Communication.Communication.IPandMessage(IP, message);

                Send(messagetoSend);
                return true;
            }
            catch (Exception e)
            {
                e.WriteLog().SaveToDataBase();
            }
            return false;
        }

        private byte[] ToBytes(agvTask task, agvStation_Info destination)
        {
            byte[] messageRAW = Encoding.ASCII.GetBytes(task.Id + destination.Name + destination.Face);

            byte[] message = new byte[4];
            message[0] = messageRAW[0];
            message[1] = messageRAW[1];
            message[2] = messageRAW[2];
            message[3] = messageRAW[3];
            return message;
        }

        private static byte[] ToBytes(Barcode barcode)
        {
            byte[] messageRAW = Encoding.ASCII.GetBytes(barcode.Comand + barcode.Destination);

            byte[] message = new byte[4];
            message[0] = messageRAW.Length > 0 ? messageRAW[0] : (byte)0;
            message[1] = messageRAW.Length > 1 ? messageRAW[1] : (byte)0;
            message[2] = messageRAW.Length > 2 ? messageRAW[2] : (byte)0;
            message[3] = messageRAW.Length > 3 ? messageRAW[3] : (byte)0;
            return message;
        }

        public void SetupServer()
        {
            if (serverForVcs == null) serverForVcs = new CommunicationServer(_vcsPort);
        }

        private void Send(Communication.Communication.IPandMessage messagetoSend)
        {
            serverForVcs.sendMessage(messagetoSend);
        }

        public void RecieveVCSCommands()
        {
            while (true)
            {
                try
                {
                    //Loop here until we get a response from VCS.
                    var response = serverForVcs.receiveMessage();
                    if (response == null) continue;

                    //Save the response byte[] in the queue
                    var responseMesage = response.Message.ToArray();
                    _vcsRxQueue.Add(responseMesage);
                }
                catch (Exception e)
                {
                    e.WriteLog().SaveToDataBase().Display();
                    break;
                }
               
            }
        }
    }
}
