using AGV_Control_Center.Navigation;
using AGV_Control_Center.Views;
using Prism.Commands;
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

namespace AGV_Control_Center.ViewModels
{
    class ApplicationExplorerViewModel : BindableBase, IRegionMemberLifetime
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        public bool KeepAlive
        {
            get
            {
                return true;
            }
        }

        private string someTextProperty;

        public string SomeTextProperty
        {
            get { return someTextProperty; }
            set { SetProperty(ref someTextProperty, value); }
        }

        private List<string> items;

        public List<string> Items
        {
            get { return items; }
            set { SetProperty( ref items, value); }
        }

        public DelegateCommand<object>NavigateCommand { get; set; }

        public ApplicationExplorerViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            Items = new List<string>();

            foreach (var propertyName in typeof(ViewNames).GetProperties())
            {
                Items.Add(propertyName.Name);
            }

            Items.Remove(Items.Where(x => x.Equals(typeof(ApplicationExplorer).Name)).FirstOrDefault());
            SomeTextProperty = "Selection";

            NavigateCommand = new DelegateCommand<object>(exNavigateCmd);
        }

        private void exNavigateCmd(object obj)
        {
            _regionManager.RequestNavigate(RegionNames.PrimaryContentRegion, obj.ToString());
        }
    }
}
