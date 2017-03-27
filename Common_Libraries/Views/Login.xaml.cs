using Common_Libraries.Models;
using Common_Libraries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Common_Libraries
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        private LoginViewModel loginVM = new LoginViewModel();
        private ApplicationUser user = new ApplicationUser();
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            user.Name = txtUsername.Text;
            user.Password = txtPassword.Password;
            if (loginVM.Login(user)) 
            {
                this.Close();
            }
        }
    }
}
