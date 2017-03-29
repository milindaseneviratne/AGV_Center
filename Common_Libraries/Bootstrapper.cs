using Common_Libraries.Navigation;
using Common_Libraries.Views;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Common_Libraries
{
    class Bootstrapper :UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.TryResolve<ApplicationShell>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            Container.RegisterTypeForNavigation<Login>(ViewNames.LoginPage);
            Container.RegisterTypeForNavigation<ApplicationExplorer>(ViewNames.ApplicationExplorer);
            Container.RegisterTypeForNavigation<SubmitCommand>(ViewNames.SubmitCommand);
            Container.RegisterTypeForNavigation<CommandServer>(ViewNames.CommandServer);
        }
    }
}
