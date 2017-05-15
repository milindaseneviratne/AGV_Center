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

        private AsynchonousSocketListner agvControlSystemServer;
        public AsynchonousSocketListner AgvControlSystemServer
        {
            get
            {
                if (agvControlSystemServer == null)
                {
                    agvControlSystemServer = new AsynchonousSocketListner();
                }
                return agvControlSystemServer;
            }
            set
            {
                if (agvControlSystemServer == null)
                {
                    agvControlSystemServer = new AsynchonousSocketListner();
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

        private void InitializeServer()
        {
            AgvControlSystemServer = new AsynchonousSocketListner();
            Thread listnerThread = new Thread(AgvControlSystemServer.StartListning);
            listnerThread.SetApartmentState(ApartmentState.STA);
            listnerThread.Start();
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
