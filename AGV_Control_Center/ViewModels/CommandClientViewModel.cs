using AGV_Control_Center.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibraries.Enumerations;
using CommonLibraries.Models;
using CommonLibraries.Models.dbModels;
using System.Collections.ObjectModel;
using Socket_Client.Models;
using System.Threading;

namespace AGV_Control_Center.ViewModels
{
    class CommandClientViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        private readonly RegionManager _regionManager;
        private readonly EventAggregator _eventAggregator;
        private AsynchonousClient client = new AsynchonousClient();
        private CommunicationLogger sqlLogger = new CommunicationLogger();
        private SerialCommunicator serialCom = new SerialCommunicator();
        private CancellationTokenSource cts = new CancellationTokenSource();
        
        //private CommunicationLogger
        private ApplicationUser user;

        public ApplicationUser UserProperty
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }

        public bool KeepAlive
        {
            get
            {
                return true;
            }
        }

        public ObservableCollection<BarcodeScanner> BarcodeScanners { get; set; }
        public DelegateCommand SendCommand { get; set; }
        public DelegateCommand ConnectScannerCommand { get; set; }
        public DelegateCommand SendQrCode { get; set; }
        private string qrCode;

        public string QrCode
        {
            get { return qrCode; }
            set { SetProperty(ref qrCode, value); }
        }

        private string errorMessage;

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }

        private ObservableCollection<string> communicationsList;

        public ObservableCollection<string> CommunicationsList
        {
            get { return communicationsList; }
            set { SetProperty( ref communicationsList, value); }
        }

        public string scannedBarcode { get; set; }
        public string rxCommand { get; set; }

        public CommandClientViewModel(RegionManager regionManager, EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            SendCommand = new DelegateCommand(exSendCmd, canSendCmd).ObservesProperty(() => UserProperty);
            SendQrCode = new DelegateCommand(exSendQrCodeCmd, canSendQrCodeCmd).ObservesProperty(() => QrCode);
            ConnectScannerCommand = new DelegateCommand(exConnectToScanner);
        }

        private void exConnectToScanner()
        {
            
        }

        private async Task Startprocess(BarcodeScanner scanner)
        {
            while (true)
            {
                scannedBarcode = await serialCom.GetScannedBarcode(scanner, cts.Token);

                if (!string.IsNullOrWhiteSpace(scannedBarcode))
                {
                    CommunicationsList.Insert(0, "Sent to Server ---->" + scannedBarcode);
                    rxCommand = await client.SendRecTCPCommand(scannedBarcode + "<EOF>", "10.52.12.73");
                    CommunicationsList.Insert(0, "Recvd from Server <----" + rxCommand);
                    sqlLogger.LogServerClientCommunications(rxCommand, scannedBarcode);
                }
            }
        }

        private bool canSendQrCodeCmd()
        {
            bool successFlag = false;

            if (QrCode.Contains("<EOF>"))
            {
                successFlag = true;
            }

            return successFlag;
        }

        private async void exSendQrCodeCmd()
        {
            CommunicationsList.Insert(0, "Sent to Server ---->" + QrCode);
            rxCommand = await client.SendRecTCPCommand(QrCode, "10.52.12.73");
            CommunicationsList.Insert(0, "Recvd from Server <----" + rxCommand);
            sqlLogger.LogServerClientCommunications(rxCommand, QrCode);
        }

        private bool canSendCmd()
        {
            bool canSendCmd = false;

            if (UserProperty != null)
            {
                canSendCmd = UserProperty.Group != UserGroups.Operator;
            }
             
            return canSendCmd;
        }

        private void exSendCmd()
        {
            //SendCommandLogic Here.
        }

        //private void LoadUserCredentials(UserCredentialsDTO obj)
        //{
        //    UserProperty = obj.User;
        //}

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            UserProperty = (ApplicationUser)navigationContext.Parameters[typeof(ApplicationUser).Name] ?? UserProperty;
            InitializeUI();
            await InitializeBarcodeScanner();
        }

        private async Task InitializeBarcodeScanner()
        {
            BarcodeScanners.AddRange(serialCom.AttemptConnectionToScanners());

            if (BarcodeScanners.Count > 0)
            {
                foreach (var scanner in BarcodeScanners)
                {
                    Startprocess(scanner);
                }
            }
        }

        private void InitializeUI()
        {
            CommunicationsList = new ObservableCollection<string>();
            BarcodeScanners = new ObservableCollection<BarcodeScanner>();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            navigationContext.Parameters.Add(typeof(ApplicationUser).Name, UserProperty);
            FinalizeUI();
        }

        private void FinalizeUI()
        {
            foreach (var scanner in BarcodeScanners)
            {
                if (scanner.SerialPort.IsOpen)
                {
                    scanner.SerialPort.DiscardInBuffer();
                    scanner.SerialPort.DiscardOutBuffer();
                    scanner.SerialPort.Close();
                    scanner.SerialPort.Dispose();
                }
            }
        }
    }
}
