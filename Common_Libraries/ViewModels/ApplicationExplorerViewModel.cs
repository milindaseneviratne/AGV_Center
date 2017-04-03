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
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AGV_Control_Center.ViewModels
{
    class ApplicationExplorerViewModel : BindableBase, IRegionMemberLifetime, INavigationAware
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
        private ApplicationUser userProperty;

        public ApplicationUser UserProperty
        {
            get { return userProperty; }
            set { SetProperty(ref userProperty, value); }
        }

        private string someTextProperty;

        public string SomeTextProperty
        {
            get { return someTextProperty; }
            set { SetProperty(ref someTextProperty, value); }
        }

        private ObservableCollection<string> items;

        public ObservableCollection<string> Items
        {
            get { return items; }
            set { SetProperty( ref items, value); }
        }

        public DelegateCommand<object>NavigateCommand { get; set; }

        public ApplicationExplorerViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            Items = new ObservableCollection<string>();

            foreach (var propertyName in typeof(ViewNames).GetProperties())
            {
                Items.Add(propertyName.Name);
            }

            //_eventAggregator.GetEvent<UserCredentialsDTO>().Subscribe(LoadUserData);

            Items.Remove(Items.Where(x => x.Equals(typeof(ApplicationExplorer).Name)).FirstOrDefault());
            Items.Remove(Items.Where(x => x.Equals(typeof(Login).Name)).FirstOrDefault());
            SomeTextProperty = "Selection";

            NavigateCommand = new DelegateCommand<object>(exNavigateCmd);
        }

        private void LoadUserData(UserCredentialsDTO obj)
        {
            UserProperty = obj.User;
        }

        private void exNavigateCmd(object obj)
        {
            _regionManager.RequestNavigate(RegionNames.PrimaryContentRegion, obj.ToString());
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
