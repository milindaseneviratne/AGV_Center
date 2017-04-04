using CommonLibraries.Models;
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
        //private static AsynchonousSocketListner asyncListner = new AsynchonousSocketListner();

        //private Action startSeerver = new Action(AsynchonousSocketListner.StartListning);


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

        private void InitializeServer()
        {
            //startSeerver.BeginInvoke();

            Thread listnerThread = new Thread(AsynchonousSocketListner.StartListning);
            listnerThread.Start();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            UserProperty = (ApplicationUser)navigationContext.Parameters[typeof(ApplicationUser).Name] ?? UserProperty;

            InitializeServer();
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
