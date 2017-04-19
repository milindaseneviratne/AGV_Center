using CommonLibraries.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries.Database
{
    class sqlDbContextEF : DbContext
    {
        public int MyProperty { get; set; }
        public sqlDbContextEF() : base("name=CommonLibraries.Properties.Settings.AGV_Control_Center")
        {
        }


    }
}
