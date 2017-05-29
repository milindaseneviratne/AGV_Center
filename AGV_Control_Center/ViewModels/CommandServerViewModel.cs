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
using System.Windows.Threading;

namespace AGV_Control_Center.ViewModels
{
    public class CommandServerViewModel : BindableBase, IRegionMemberLifetime, INavigationAware
    {
        private BlockingCollection<Barcode> vcsTxQueue = new BlockingCollection<Barcode>( new ConcurrentQueue<Barcode>());
        private BlockingCollection<byte[]> vcsRxQueue = new BlockingCollection<byte[]>(new ConcurrentQueue<byte[]>());

        private BlockingCollection<string> agvRxQueue = new BlockingCollection<string>(new ConcurrentQueue<string>());

        private VCSCommunicator vcsServer;
        private BarcodeDecoder bacrodeDecoder;
        private agvTaskDequer agvTaskDequer;

        private ApplicationUser user;

        private AsynchonousServer agvControlSystemServer;

        private string _AGV_Server_Status;
        private string _AGV_Task_Dequer_Status;
        private string _Barcode_Decoder_Status;
        private string _VCS_Tx_Server_Status;
        private string _VCS_Rx_Server_Status;

        private Task agvServerListner;
        private Task barcodeDecoderProcesor;
        private Task vcs_tx_CommandProcess;
        private Task vcs_rx_CommandProcess;
        private Task agvTaskDequerProcess;

        private DispatcherTimer checkProcessStatusTimer = new DispatcherTimer();

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
        public ApplicationUser UserProperty
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }
        public DelegateCommand StartServerCommand { get; set; }
        public bool KeepAlive
        {
            get
            {
                return true;
            }
        }

        public string Barcode_Decoder_Status
        {
            get
            {
                return _Barcode_Decoder_Status;
            }
            set
            {
                SetProperty(ref _Barcode_Decoder_Status, value);
            }
        }
        public string VCS_Tx_Server_Status
        {
            get
            {
                return _VCS_Tx_Server_Status;
            }
            set
            {
                SetProperty(ref _VCS_Tx_Server_Status, value);
            }
        }
        public string VCS_Rx_Server_Status
        {
            get
            {
                return _VCS_Rx_Server_Status;
            }
            set
            {
                SetProperty(ref _VCS_Rx_Server_Status, value);
            }
        }
        public string AGV_Task_Dequer_Status
        {
            get
            {
                return _AGV_Task_Dequer_Status;
            }
            set
            {
                SetProperty(ref _AGV_Task_Dequer_Status, value);
            }
        }
        public string AGV_Server_Status
        {
            get
            {
                return _AGV_Server_Status;
            }
            set
            {
                SetProperty(ref _AGV_Server_Status, value);
            }
        }

        public CommandServerViewModel()
        {
            vcsServer = new VCSCommunicator(vcsRxQueue, vcsTxQueue);
            bacrodeDecoder = new BarcodeDecoder(agvRxQueue, vcsTxQueue);
            agvTaskDequer = new agvTaskDequer(vcsServer);

            StartServerCommand = new DelegateCommand(exStartServerCmd, canExStartServerCmd).ObservesProperty(() => UserProperty);

            checkProcessStatusTimer.Interval = TimeSpan.FromMilliseconds(1000);
            checkProcessStatusTimer.Tick += new EventHandler(checkProcessStatusTimer_Tick);
            checkProcessStatusTimer.Start();
        }

        private void checkProcessStatusTimer_Tick(object sender, EventArgs e)
        {
            if (agvServerListner == null || barcodeDecoderProcesor == null || vcs_tx_CommandProcess == null || vcs_rx_CommandProcess == null || agvTaskDequerProcess == null)
            {
                AGV_Server_Status = "AGV Server status: NULL";
                Barcode_Decoder_Status = "Barcode Decoder Status: NULL";
                VCS_Tx_Server_Status = "VCS Tx Server Status: NULL";
                VCS_Rx_Server_Status = "VCS Rx Server Status: NULL";
                AGV_Task_Dequer_Status = "AGV Task Dequer Status: NULL";
                return;
            }

            AGV_Server_Status = "AGV Server status: " + agvServerListner.Status.ToString().ToUpper();
            Barcode_Decoder_Status = "Barcode Decoder Status: " + barcodeDecoderProcesor.Status.ToString().ToUpper();
            VCS_Tx_Server_Status = "VCS Tx Server Status: " + vcs_tx_CommandProcess.Status.ToString().ToUpper();
            VCS_Rx_Server_Status = "VCS Rx Server Status: " + vcs_rx_CommandProcess.Status.ToString().ToUpper();
            AGV_Task_Dequer_Status = "AGV Task Dequer Status: " + agvTaskDequerProcess.Status.ToString().ToUpper();

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
            //Start the server, load values into RxQueue, send Client Response.
            agvServerListner = Task.Run(() => AgvControlSystemServer.StartListning());

            //ProcessValues in the Rxqueue
            barcodeDecoderProcesor = Task.Run(() => bacrodeDecoder.ProcessBarcodes());
            
            //Start VCS Communication server
            vcsServer.SetupServer();

            //Send Commands to VCS and create task.
            vcs_tx_CommandProcess = Task.Run(() => vcsServer.SendVCSCommands());

            //Recieve Commands from VCS
            vcs_rx_CommandProcess = Task.Run(() => vcsServer.RecieveVCSCommands());

            //Dequeue Tasks
            agvTaskDequerProcess = Task.Run(() => agvTaskDequer.DequeueTasks());
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
