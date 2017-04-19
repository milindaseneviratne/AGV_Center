namespace CommonLibraries.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CommonLibraries.Database.sqlDbContextEF>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(CommonLibraries.Database.sqlDbContextEF context)
        {
            //This method will be called after migrating to the latest version.

            //You can use the DbSet<T>.AddOrUpdate() helper extension method
            //to avoid creating duplicate seed data.E.g.

              context.User.AddOrUpdate(
                u => u.Name,
                new User { Name = "Milinda", Password = "qwerty", Group = "Administrator" },
                new User { Name = "Jeffery", Password = "qwerty", Group = "Administrator" },
                new User { Name = "MIS", Password = "1234", Group = "User" },
                new User { Name = "OP", Password = string.Empty, Group = "User" }
              );

        }
    }
}
