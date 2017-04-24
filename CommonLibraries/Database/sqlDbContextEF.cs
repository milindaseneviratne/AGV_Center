using CommonLibraries.Models;
using CommonLibraries.Models.dbModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries.Database
{
    class sqlDbContextEF : DbContext
    {
        public DbSet<ApplicationErrorLog> ApplicationErrorLog { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserActivityLog> UserActivityLog { get; set; }
        public DbSet<ServerClientCommunicationLog> CommunicationLog { get; set; }

        public DbSet<agvStationTestFlow> agvStationTestFlow { get; set; }
        public DbSet<agvAGV_Info> agvAGV_Info { get; set; }
        public DbSet<agvCommand_Type_Info> agvCommand_Type_Info { get; set; }
        public DbSet<agvConfig> agvConfig { get; set; }
        public DbSet<agvModel_Info> agvModel_Info { get; set; }
        public DbSet<agvStation_Info> agvStation_Info { get; set; }
        public DbSet<agvStatus_Info> agvStatus_Info { get; set; }
        public DbSet<agvTable_Info> agvTable_Info { get; set; }
        public DbSet<agvTask> agvTask { get; set; }
        public DbSet<agvZone_Info> agvZone_Info { get; set; }

        //JefferySQLExpress
        //MilindaSQLExpress
        public sqlDbContextEF() : base("name=MilindaSQLExpress") 
        {
            
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

    }
}
