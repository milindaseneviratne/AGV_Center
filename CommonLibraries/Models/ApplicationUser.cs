using CommonLibraries.Enumerations;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries.Models
{
    public class ApplicationUser : BindableBase
    {
        public int Id { get; set; }

        private string name;

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private UserGroups group;

        public UserGroups Group
        {
            get { return group; }
            set
            {
                SetProperty(ref group, value);
            }
        }
        public string Password { get; set; }

        public DateTime LogIn { get; set; }
        public DateTime LogOut { get; set; }
    }
}
