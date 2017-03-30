using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGV_Control_Center.ViewModels
{
    class AGV_Control_Center_HomeViewModel :BindableBase, IRegionMemberLifetime, INavigationAware
    {
        public readonly IEventAggregator _eventAggregator;
        public readonly IRegionManager _regionManager;

        public bool KeepAlive
        {
            get
            {
                return true;
            }
        }


        public AGV_Control_Center_HomeViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
           //
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //
        }
    }
}
