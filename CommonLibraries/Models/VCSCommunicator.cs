using CommonLibraries.Extensions;
using CommonLibraries.Models.dbModels;
using Communication;
using System;
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
        private CommunicationClient Client;
        private IPAddress IP = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(addresses => addresses.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
        private Communication.Communication.IPandMessage messagetoSend;

        public bool SendCommand(Barcode barcode, string vcsIP = "", int vcsPort = 26000 )
        {
            try
            {
                byte[] message = ToBytes(barcode);

                messagetoSend = new Communication.Communication.IPandMessage(IP, message);

                Send(vcsPort);
                return true;
            }
            catch (Exception e)
            {
                e.WriteLog().SaveToDataBase();
            }

            return false;
        }
        public bool SendDequeueTaskCommand(agvTask task, agvStation_Info destination, int vcsPort = 26000)
        {
            try
            {
                byte[] message = ToBytes(task, destination);

                messagetoSend = new Communication.Communication.IPandMessage(IP, message);

                Send(vcsPort);
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
            message[0] = messageRAW[0];
            message[1] = messageRAW[1];
            message[2] = messageRAW[2];
            message[3] = messageRAW[3];
            return message;
        }
        
        private void Send(int vcsPort)
        {
            Client = new CommunicationClient(IP.ToString(), vcsPort);
            Client.sendMessage(messagetoSend);
        }
    }
}
