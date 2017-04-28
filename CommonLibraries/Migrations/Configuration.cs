namespace CommonLibraries.Migrations
{
    using Database;
    using Models;
    using Models.dbModels;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Linq.Expressions;

    internal sealed class Configuration : DbMigrationsConfiguration<sqlDbContextEF>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(sqlDbContextEF context)
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

            context.agvModel_Info.AddOrUpdate(u => u.Name,
                new agvModel_Info { Name = "DaVinci Super", Code = "3FSXX", Remark = "The Models Should be here." },
                new agvModel_Info { Name = "DaVinci Super Colour", Code = "3FCXX", Remark = "The Models Should be here." }
                );

            context.agvZone_Info.AddOrUpdate(u => u.Name,
                new agvZone_Info { Name = "Zone1", Remark = "Remarks go here." },
                new agvZone_Info { Name = "Zone2", Remark = "Remarks go here." },
                new agvZone_Info { Name = "Zone3", Remark = "Remarks go here." }
                );

            context.agvCommand_Type_Info.AddOrUpdate(u => u.Name,
                new agvCommand_Type_Info { Name = "Table_Move", Byte = "11", Type = "Table_Move#10", Argument = "TaskID+ PickupLoc(Zone,X,Y)+DestLoc (Zone,X,Y)", DataSource = "agvStationTestFlow", Remark = "Only Table Move" }
                );

            context.agvStatus_Info.AddOrUpdate(u => u.Status,
                new agvStatus_Info { Status = "Initial", Arg1 = string.Empty, Arg2 = string.Empty, Arg3 = string.Empty, Remark = "Remarks go here" },
                new agvStatus_Info { Status = "OK", Arg1 = string.Empty, Arg2 = string.Empty, Arg3 = string.Empty, Remark = "Remarks go here" }
                );

            var model = context.agvModel_Info.Where(x => x.Id < 100).FirstOrDefault();
            var zone = context.agvZone_Info.Where(x => x.Id < 100).FirstOrDefault();
            if (model != null && zone != null) AddStations(context);

            var stations = context.agvStation_Info.FirstOrDefault();
            if (model != null && zone != null && stations != null) AddStationTestFlow(context);
        }

        private static void AddStationTestFlow(sqlDbContextEF context)
        {
            for (int i = 0; i < 3; i++)
            {
                context.agvStationTestFlow.AddOrUpdate(x => new { x.Command_TypeId, x.Current_StationId, x.Dest_StationId },
                new agvStationTestFlow
                {
                    Command_TypeId = context.agvCommand_Type_Info.Where(x => x.Name.StartsWith("T")).FirstOrDefault().Id,
                    Current_StationId = context.agvStation_Info.Where(x => x.Name.StartsWith("S")).FirstOrDefault().Id + i,
                    Dest_StationId = context.agvStation_Info.Where(x => x.Name.StartsWith("S")).FirstOrDefault().Id + i + 1,
                    ZoneId = context.agvZone_Info.Where(x => x.Id < 100).FirstOrDefault().Id,
                    StatusId = context.agvStatus_Info.Where(x => x.Id < 100).FirstOrDefault().Id,
                    UpdateTime = DateTime.Now
                });
            }
        }

        public Expression<Func<agvStationTestFlow, object>> identifierExpression()
        {
            return x => x.Current_StationId;
        }
        private static void AddStations(sqlDbContextEF context)
        {
            string stationName = string.Empty;
            CreateStation(context, "Initial");

            for (int i = 1; i < 11; i++)
            {
                CreateStation(context, "S" + i.ToString());
            }

            for (int i = 1; i < 5 + 1; i++)
            {
                CreateStation(context, "M" + i.ToString());
            }

            for (int i = 1; i < 30 + 1; i++)
            {
                CreateStation(context, "P" + i.ToString());
            }

            for (int i = 1; i < 3 + 1; i++)
            {
                CreateStation(context, "FDS" + i.ToString());
            }

            for (int i = 1; i < 15 + 1; i++)
            {
                CreateStation(context, "CSA" + i.ToString());
            }

            for (int i = 1; i < 3 + 1; i++)
            {
                CreateStation(context, "Packing" + i.ToString());
            }
        }

        private static void CreateStation(sqlDbContextEF context, string stationName)
        {
            context.agvStation_Info.AddOrUpdate(u => u.Name,
                                new agvStation_Info
                                {
                                    Name = stationName,
                                    Face = "Up",
                                    Location = "*001002",
                                    ModelId = context.agvModel_Info.Where(x => x.Id < 100).FirstOrDefault().Id,
                                    Remark = "Remarks go here.",
                                    Status = "Default",
                                    UpdateTime = DateTime.Now,
                                    ZoneId = context.agvZone_Info.Where(x => x.Id < 100).FirstOrDefault().Id
                                });
        }
    }
}
