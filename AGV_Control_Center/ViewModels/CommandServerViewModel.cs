using CommonLibraries.Extensions;
using CommonLibraries.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Socket_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AGV_Control_Center.ViewModels
{
    public class CommandServerViewModel : BindableBase, IRegionMemberLifetime, INavigationAware
    {
        private agvTaskCreator agvTaskCreator = new agvTaskCreator();
        private BarcodeDecoder bacrodeDecoder = new BarcodeDecoder();
        private VCSCommunicator vcsComm = new VCSCommunicator();
        private RecievedMessagesController rxMessageQueue = new RecievedMessagesController();

        public bool KeepAlive
        {
            get
            {
                return true;
            }
        }

        private ApplicationUser user;

        public ApplicationUser UserProperty
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }

        private AsynchonousServer agvControlSystemServer;
        public AsynchonousServer AgvControlSystemServer
        {
            get
            {
                if (agvControlSystemServer == null)
                {
                    agvControlSystemServer = new AsynchonousServer();
                }
                return agvControlSystemServer;
            }
            set
            {
                if (agvControlSystemServer == null)
                {
                    agvControlSystemServer = new AsynchonousServer();
                }
            }
        }

        public DelegateCommand StartServerCommand { get; set; }
        public CommandServerViewModel()
        {
            StartServerCommand = new DelegateCommand(exStartServerCmd, canExStartServerCmd).ObservesProperty(() => UserProperty);
        }

        private void exStartServerCmd()
        {
            InitializeServer();
        }

        private bool canExStartServerCmd()
        {
            if (AgvControlSystemServer.listner == null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// This section is incomeplete as at 25th May 2017.
        /// The Original communication method with the AGV
        /// </summary>
        private void InitializeServer()
        {
            //Start the server
            //Load values to the messageQueue
            AgvControlSystemServer.StartListning(rxMessageQueue);

            //ProcessValues in the message queue
            byte[] rxMessageArray;
            rxMessageQueue.TryTake(out rxMessageArray);
            var vcsCommand = bacrodeDecoder.GetVCSCommand(rxMessageArray);

            //Start VCS Communication server

            //Send Messages to VCS.
            bool success = false;
            if (vcsCommand.HasData()) success = vcsComm.SendCommand(vcsCommand);

            //Recieve Messages from VCS

            //Create a Task
            bool taskCreated = false;
            if (success) taskCreated = agvTaskCreator.CreateTask(vcsCommand);

            //Create ACK for Clients
            //if (success && taskCreated)
            //{
            //    recvievedBarcode = (recvievedBarcode.RemoveEOF() + " OK").AddEOF();
            //}
            //else
            //{
            //    recvievedBarcode = (recvievedBarcode.RemoveEOF() + " ERROR").AddEOF();
            //}
            //Send Client Response.

            //Dequeue Tasks
            agvTaskCreator.DequeueTasks();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            UserProperty = (ApplicationUser)navigationContext.Parameters[typeof(ApplicationUser).Name] ?? UserProperty;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            navigationContext.Parameters.Add(typeof(ApplicationUser).Name, UserProperty);
        }
    }
}
