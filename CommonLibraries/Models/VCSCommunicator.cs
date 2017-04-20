using CommonLibraries.Extensions;
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
    public static class VCSCommunicator
    {
        public static bool SendCommand(Barcode barcode, string vcsIP = "", int vcsPort = 26000 )
        {
            try
            {
                CommunicationClient Client;
                IPAddress IP = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(addresses => addresses.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();

                Client = new CommunicationClient(IP.ToString(), vcsPort);
                byte[] message = Encoding.ASCII.GetBytes(barcode.Comand + barcode.Destination);
                Communication.Communication.IPandMessage messagetoSend = new Communication.Communication.IPandMessage(IP, message);
                Client.sendMessage(messagetoSend);
                return true;
            }
            catch (Exception e)
            {
                e.WriteLog().SaveToDataBase();
            }

            return false;
        }
    }
}
