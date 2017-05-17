using AGV_Control_Center.Events;
using AGV_Control_Center.Navigation;
using CommonLibraries.Models;
using CommonLibraries.Database;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CommonLibraries.Enumerations;
using CommonLibraries.Extensions;

namespace AGV_Control_Center.ViewModels
{
    public class LoginViewModel :BindableBase, IRegionMemberLifetime, INavigationAware
    {
        private readonly RegionManager _regionManager;
        private readonly EventAggregator _eventAggregator;

        private readonly UserCredentialsDTO userCredentials = new UserCredentialsDTO();
        private SQLCommunicator sqldbCommunicator = new SQLCommunicator();

        private ApplicationUser userProperty;

        public ApplicationUser UserProperty
        {
            get { return userProperty; }
            set { SetProperty(ref userProperty,value); }
        }

        private string loginFailedMessage;

        public string LoginFailedMessage
        {
            get { return loginFailedMessage; }
            set { SetProperty(ref loginFailedMessage, value); }
        }

        public bool KeepAlive
        {
            get
            {
                return true;
            }
        }

        public DelegateCommand<object> LoginCommand { get; set; }

        public LoginViewModel(RegionManager regionManager, EventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            LoginCommand = new DelegateCommand<object>(exLoginCmd);

            UserProperty = new ApplicationUser();
        }

        public void exLoginCmd(object sender)
        {
            try
            {
                //throw new UnauthorizedAccessException();
                //ModelFactory modelfactory = new ModelFactory();
                //modelfactory.Convert();

                PasswordBox pwdBox = sender as PasswordBox;
                UserProperty.Password = pwdBox.Password;

                var dbUserInfo = sqldbCommunicator.GetuserInfo(UserProperty);

                if (dbUserInfo == null)
                {
                    LoginFailedMessage = "Incorrect username/password, please try again!";
                }
                else
                {
                    LoginFailedMessage = string.Empty;

                    UserProperty.Id = dbUserInfo.Id;
                    UserProperty.Name = dbUserInfo.Name;
                    UserProperty.Password = dbUserInfo.Password; // Delete this for security reasons once prduction version is launched.
                    UserProperty.Group = dbUserInfo.Group.ToUserGroup();
                    UserProperty.LogIn = DateTime.Now;

                    sqldbCommunicator.LogUserIN(UserProperty);

                    userCredentials.User = UserProperty;
                    _eventAggregator.GetEvent<UserCredentialsDTO>().Publish(userCredentials);

                    DisplayUI();
                }
            }
            catch (Exception e)
            {
                e.WriteLog().SaveToDataBase().Display();
            }
           

            //LoginFailedMessage = string.Empty;

            //UserProperty.Id = 100;
            //UserProperty.Name = "Milinda";
            //UserProperty.Password = "qwerty"; // Delete this for security reasons once prduction version is launched.
            //UserProperty.Group = UserGroups.Administrator;
            //UserProperty.LogIn = DateTime.Now;

            //userCredentials.User = UserProperty;
            //_eventAggregator.GetEvent<UserCredentialsDTO>().Publish(userCredentials);
            //DisplayUI();

            //sqldbCommunicator.LogUserIN(UserProperty);
        }

        private void DisplayUI()
        {
            if (UserProperty.Group == UserGroups.Administrator)
            {
                _regionManager.RequestNavigate(RegionNames.ListContentRegion, ViewNames.ApplicationExplorer);
                _regionManager.RequestNavigate(RegionNames.PrimaryContentRegion, ViewNames.AGV_Control_Center_Home);
            }

            if (UserProperty.Group == UserGroups.Operator)
            {
                _regionManager.RequestNavigate(RegionNames.PrimaryContentRegion, ViewNames.CommandClient);
            }

            if (UserProperty.Group == UserGroups.User)
            {
                _regionManager.RequestNavigate(RegionNames.PrimaryContentRegion, ViewNames.CommandClient);
            }
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
