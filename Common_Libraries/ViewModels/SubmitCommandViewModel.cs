﻿using AGV_Control_Center.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibraries.Enumerations;
using CommonLibraries.Models;

namespace AGV_Control_Center.ViewModels
{
    class SubmitCommandViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        private readonly RegionManager _regionManager;
        private readonly EventAggregator _eventAggregator;

        private ApplicationUser user;

        public ApplicationUser UserProperty
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }

        public bool KeepAlive
        {
            get
            {
                return true;
            }
        }

        public DelegateCommand SendCommand { get; set; }


        public SubmitCommandViewModel(RegionManager regionManager, EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            //_eventAggregator.GetEvent<UserCredentialsDTO>().Subscribe(LoadUserCredentials);
            SendCommand = new DelegateCommand(exSendCmd, canSendCmd).ObservesProperty(() => UserProperty);
        }

        private bool canSendCmd()
        {
            bool canSendCmd = false;

            if (UserProperty != null)
            {
                canSendCmd = UserProperty.Group != UserGroups.Operator;
            }
             
            return canSendCmd;
        }

        private void exSendCmd()
        {
            //SendCommandLogic Here.
        }

        private void LoadUserCredentials(UserCredentialsDTO obj)
        {
            UserProperty = obj.User;
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
