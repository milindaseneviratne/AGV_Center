using Common_Libraries.Events;
using Common_Libraries.Models;
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
    class SubmitCommandViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;

        private ApplicationUser user;

        public ApplicationUser UserProperty
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }

        public SubmitCommandViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            _eventAggregator.GetEvent<UserCredentialsDTO>().Subscribe(LoadUserCredentials);
        }

        private void LoadUserCredentials(UserCredentialsDTO obj)
        {
            UserProperty = obj.User;
        }
    }
}
