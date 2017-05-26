using CommonLibraries.Extensions;
using CommonLibraries.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Socket_Server.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AGV_Control_Center.ViewModels
{
    public class CommandServerViewModel : BindableBase, IRegionMemberLifetime, INavigationAware
    {
        private ConcurrentQueue<Barcode> vcsTxQueue = new ConcurrentQueue<Barcode>();
        private ConcurrentQueue<byte[]> vcsRxQueue = new ConcurrentQueue<byte[]>();

        private ConcurrentQueue<string> agvRxQueue = new ConcurrentQueue<string>();

        private VCSCommunicator vcsServer;
        private BarcodeDecoder bacrodeDecoder;
        private agvTaskDequer agvTaskDequer;


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
                    agvControlSystemServer = new AsynchonousServer(agvRxQueue);
                }
                return agvControlSystemServer;
            }
            set
            {
                if (agvControlSystemServer == null)
                {
                    agvControlSystemServer = new AsynchonousServer(agvRxQueue);
                }
            }
        }

        public DelegateCommand StartServerCommand { get; set; }
        public CommandServerViewModel()
        {
            vcsServer = new VCSCommunicator(vcsRxQueue, vcsTxQueue);
            bacrodeDecoder = new BarcodeDecoder(agvRxQueue, vcsTxQueue);
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
        /// The Original communication method with the AGV Center and VCS was incompatible.
        /// The VCS needs continous read write communication.
        /// </summary>
        private void InitializeServer()
        {
            //Start the server
            Task.Run(() => AgvControlSystemServer.StartListning());
            //Load values to the messageQueue

            //ProcessValues in the message queue
            Task.Run(() => bacrodeDecoder.ProcessBarcodes());

            //Start VCS Communication server
            vcsServer.SetupServer();

            //Send Commands to VCS and create task.
            Task.Run(() => vcsServer.SendVCSCommands());
            
            //Recieve Commands from VCS
            Task.Run(() => vcsServer.RecieveVCSCommands());

            //Create ACK for Clients

            //Send Client Response.

            //Dequeue Tasks
            Task.Run(() => agvTaskDequer.DequeueTasks());
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
