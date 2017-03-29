using AGV_CIMCenter;
using Common_Libraries.Extensions;
using Common_Libraries.Models;
using Common_Libraries.Navigation;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Common_Libraries.ViewModels
{
    public class LoginViewModel :BindableBase, IRegionMemberLifetime
    {
        private readonly RegionManager _regionManager;
        private readonly EventAggregator _eventAggregator;

        private SQLCommunicator sqldbCommunicator = new SQLCommunicator();

        private ApplicationUser userProperty;

        public ApplicationUser UserProperty
        {
            get { return userProperty; }
            set { SetProperty(ref userProperty,value); }
        }

        private string loginFailedMessage;

        public string LoginFailedMessage
        {
            get { return loginFailedMessage; }
            set { SetProperty(ref loginFailedMessage, value); }
        }


        public bool KeepAlive
        {
            get
            {
                return true;
            }
        }

        public DelegateCommand<object> LoginCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }

        public LoginViewModel(RegionManager regionManager, EventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            LoginCommand = new DelegateCommand<object>(exLoginCmd);
            ExitCommand = new DelegateCommand(exBackCmd);

            UserProperty = new ApplicationUser();
        }

        private void exBackCmd()
        {
            Application.Current.Shutdown();
        }

        public void exLoginCmd(object sender)
        {
            PasswordBox pwdBox = sender as PasswordBox;
            UserProperty.Password = pwdBox.Password;

            //var dbUserInfo = sqldbCommunicator.GetuserInfo(UserProperty);

            //var dbUserInfo = sqldbCommunicator.GetuserInfo(UserProperty);

            //if (dbUserInfo == null)
            //{
            //    LoginFailedMessage = "Incorrect username/password, please try again!";
            //}
            //else
            //{
            //    LoginFailedMessage = string.Empty;

            //    UserProperty.Id = dbUserInfo.Id;
            //    UserProperty.Name = dbUserInfo.Name;
            //    UserProperty.Password = dbUserInfo.Password;
            //    UserProperty.Group = dbUserInfo.Group.ToUserGroup();
            //    _regionManager.RequestNavigate(RegionNames.ListContentRegion, ViewNames.ApplicationExplorer);
            //}
            _regionManager.RequestNavigate(RegionNames.ListContentRegion, ViewNames.ApplicationExplorer);

        }
    }
}
