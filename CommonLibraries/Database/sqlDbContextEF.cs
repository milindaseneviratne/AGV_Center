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
        public DbSet<agvStationTestFlow> AgvStationTestFlow { get; set; }
        public DbSet<ApplicationErrorLog> ApplicationErrorLog { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserActivityLog> UserActivityLog { get; set; }



        public sqlDbContextEF() : base("name=AGV_Control_Center")
        {
        }


    }
}
