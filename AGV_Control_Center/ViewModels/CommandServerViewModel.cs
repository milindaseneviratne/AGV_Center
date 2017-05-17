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

        private void InitializeServer()
        {
            Thread listnerThread = new Thread(AgvControlSystemServer.StartListning);
            listnerThread.SetApartmentState(ApartmentState.STA);
            listnerThread.Start();

            Thread dequeueThread = new Thread(agvTaskCreator.DequeueTasks);
            dequeueThread.SetApartmentState(ApartmentState.STA);
            dequeueThread.Start();
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
