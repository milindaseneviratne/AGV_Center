using AGV_Control_Center.Events;
using AGV_Control_Center.Models;
using AGV_Control_Center.Navigation;
using AGV_Control_Center.Views;
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

namespace AGV_Control_Center.ViewModels
{
    class ApplicationShellViewModel : BindableBase, IRegionMemberLifetime
    {
        private readonly RegionManager _regionManager;
        private readonly EventAggregator _eventAggregator;


        private string title;

        private SQLCommunicator sqldbCommunicator = new SQLCommunicator();

        public bool KeepAlive
        {
            get
            {
                return true;
            }
        }
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private ApplicationUser user;

        public ApplicationUser UserProperty
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }


        public DelegateCommand ExitCommand { get; set; }

        public ApplicationShellViewModel(RegionManager regionManager, EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            _eventAggregator.GetEvent<UserCredentialsDTO>().Subscribe(LoadUserCredentials);

            ExitCommand = new DelegateCommand(exExitCmd);

            Title = "AGV Control Center";

            _regionManager.RegisterViewWithRegion(RegionNames.PrimaryContentRegion, typeof(Login));
        }
        private void exExitCmd()
        {
            UserProperty.LogOut = DateTime.Now;
            sqldbCommunicator.LogUserOUT(UserProperty);
            Application.Current.Shutdown();
        }
        private void LoadUserCredentials(UserCredentialsDTO obj)
        {
            UserProperty = obj.User;
        }
    }
}
