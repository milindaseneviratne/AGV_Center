using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibraries.Database;
using CommonLibraries.Extensions;
using CommonLibraries.Models.dbModels;

namespace CommonLibraries.Models
{
    public class SQLCommunicator
    {
        private sqlDbContextEF dbContext = new sqlDbContextEF();

        public User GetuserInfo(ApplicationUser user)
        {
            var query = (from row in dbContext.User
                         where row.Name == user.Name && row.Password == user.Password
                         select row).FirstOrDefault();

            return query;
        }
        public bool InsertNewUser(ApplicationUser user)
        {
            bool successFlag = false;

            User dbUserObject = new User();

            dbUserObject.Name = user.Name;
            dbUserObject.Password = user.Password;
            dbUserObject.Group = user.Group.ToString();

            dbContext.User.Add(dbUserObject);

            SaveChanges();

            return successFlag;
        }

        public string GetDestination(Barcode barcode)
        {
            var result = dbContext.agvStationTestFlow
                                  .Where(row => row.Current_Station.Name == barcode.Station && row.Status.Status == barcode.Status)
                                  .FirstOrDefault()
                                  .Dest_Station.Name;
            return result;
        }

        public string GetCommandType(Barcode barcode)
        {
            var result = dbContext.agvStationTestFlow
                                  .Where(row => row.Current_Station.Name == barcode.Station && row.Status.Status == barcode.Status)
                                  .FirstOrDefault()
                                  .Command_Type.Name;

            return result;
        }

        public bool InsertExceptionInformation(Exception ex)
        {
            bool successFlag = false;

            ApplicationErrorLog dbAppErrorLog = new ApplicationErrorLog();

            dbAppErrorLog.DumpString = ex.ToString();
            dbAppErrorLog.HashCode = ex.GetHashCode().ToString();
            dbAppErrorLog.HelpLink = ex.HelpLink;
            dbAppErrorLog.Message = ex.Message;
            dbAppErrorLog.Source = ex.Source;
            dbAppErrorLog.StackTrace = ex.StackTrace;

            dbContext.ApplicationErrorLog.Add(dbAppErrorLog);

            SaveChanges();

            return successFlag;
        }

        public void LogUserOUT(ApplicationUser userProperty)
        {
            var query = (from row in dbContext.UserActivityLog
                         where row.UserId == userProperty.Id
                         orderby row.TimeStamp descending
                         select row).FirstOrDefault();

            query.LogoutTime = userProperty.LogOut;

            SaveChanges();
        }

        private void SaveChanges()
        {
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                ex.WriteLog().SaveToDataBase().Display();
            }
        }

        public void LogUserIN(ApplicationUser userProperty)
        {
            UserActivityLog userLog = new UserActivityLog();

            userLog.UserId = userProperty.Id;
            userLog.UserName = userProperty.Name;
            userLog.UserGroup = userProperty.Group.ToString();
            userLog.LoginTime = userProperty.LogIn;
            userLog.LogoutTime = DateTime.MaxValue;
            dbContext.UserActivityLog.Add(userLog);

            SaveChanges();
        }

        public void LogServerClientCommunications(ServerClientCommunicationLog log)
        {
            dbContext.CommunicationLog.Add(log);

            SaveChanges();
        }
    }
}
