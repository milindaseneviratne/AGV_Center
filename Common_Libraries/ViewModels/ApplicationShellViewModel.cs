using Common_Libraries.Views;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Libraries.ViewModels
{
    class ApplicationShellViewModel : BindableBase, IRegionMemberLifetime
    {
        public readonly IEventAggregator _eventAggregator;
        public readonly IRegionManager _regionManager;

        private string title;

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

        public ApplicationShellViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            Title = "AGV Control Center | Version : 0.0.0";

            _regionManager.RegisterViewWithRegion("PrimaryContentRegion", typeof(Login));
        }

    }
}
