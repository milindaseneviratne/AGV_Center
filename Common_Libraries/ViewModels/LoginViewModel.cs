using AGV_CIMCenter;
using Common_Libraries.Extensions;
using Common_Libraries.Models;
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

namespace Common_Libraries.ViewModels
{
    public class LoginViewModel :BindableBase
    {
        private readonly RegionManager _regionManager;
        private readonly EventAggregator _eventAggregator;

        private SQLCommunicator sqldbCommunicator = new SQLCommunicator();

        public ApplicationUser UserProperty = new ApplicationUser();

        public bool KeepAlive
        {
            get
            {
                return false;
            }
        }

        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }

        public LoginViewModel(RegionManager regionManager, EventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            LoginCommand = new DelegateCommand(exLoginCmd);
            ExitCommand = new DelegateCommand(exBackCmd);
        }

        private void exBackCmd()
        {
            Application.Current.Shutdown();
        }

        public void exLoginCmd()
        {
            var dbUserInfo = sqldbCommunicator.GetuserInfo(UserProperty);

            if (dbUserInfo == null)
            {
                //Do nothing.
            }
            else
            {
                UserProperty.Id = dbUserInfo.Id;
                UserProperty.Name = dbUserInfo.Name;
                UserProperty.Password = dbUserInfo.Password;
                UserProperty.Group = dbUserInfo.Group.ToUserGroup();

                //AGV_Center_Launcher agvCIMCenterLauncher = new AGV_Center_Launcher();
                //agvCIMCenterLauncher.Show();
            }
        }
    }
}
