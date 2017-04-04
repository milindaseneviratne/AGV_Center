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
using System.Collections.ObjectModel;
using Socket_Client.Models;

namespace AGV_Control_Center.ViewModels
{
    class SubmitCommandViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        private readonly RegionManager _regionManager;
        private readonly EventAggregator _eventAggregator;

        //private AsynchonousClient asyncClient = new AsynchonousClient();

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

        public DelegateCommand SendCommand { get; set; }
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

        public SubmitCommandViewModel(RegionManager regionManager, EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            //_eventAggregator.GetEvent<UserCredentialsDTO>().Subscribe(LoadUserCredentials);
            SendCommand = new DelegateCommand(exSendCmd, canSendCmd).ObservesProperty(() => UserProperty);
            SendQrCode = new DelegateCommand(exSendQrCodeCmd, canSendQrCodeCmd).ObservesProperty(() => QrCode);
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

        private void exSendQrCodeCmd()
        {
            AsynchonousClient.StartClient(QrCode,"C8810");
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
