using Common_Libraries.Navigation;
using Common_Libraries.Views;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Common_Libraries.ViewModels
{
    class ApplicationExplorerViewModel : BindableBase, IRegionMemberLifetime
    {
        private readonly RegionManager _regionManager;
        private readonly EventAggregator _eventAggregator;
        public bool KeepAlive
        {
            get
            {
                return true;
            }
        }
        private List<string> applicationsListProperty;

        public List<string> ApplicationsListProperty
        {
            get { return applicationsListProperty; }
            set { SetProperty(ref applicationsListProperty, value); }
        }

        public ApplicationExplorerViewModel(RegionManager regionManager, EventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            //foreach (var propertyName in typeof(ViewNames).GetProperties())
            //{
            //    ApplicationsListProperty.Add(propertyName.Name);
            //}

            applicationsListProperty.Add("Sample1");
            applicationsListProperty.Add("Sample2");
            applicationsListProperty.Add("Sample3");
            applicationsListProperty.Add("Sample4");
            applicationsListProperty.Add("Sample5");
        }

    }
}
