using CommonLibraries.Extensions;
using CommonLibraries.Models;
using CommonLibraries.Models.dbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries.Models
{
    public class CommunicationLogger
    {
        SQLCommunicator sqlCommunicatior = new SQLCommunicator();
        public void LogServerClientCommunications(string rxCommand, string txCommand)
        {
            ServerClientCommunicationLog serverClientLog = new ServerClientCommunicationLog();
            serverClientLog.TxCommand = txCommand;
            serverClientLog.RxCommand = rxCommand;
            serverClientLog.Time = DateTime.Now;

            serverClientLog.Result = rxCommand.Validate();

            sqlCommunicatior.LogServerClientCommunications(serverClientLog);
        }


    }
}
