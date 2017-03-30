using AGV_Control_Center.Database;
using AGV_Control_Center.Enumerations;
using AGV_Control_Center.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGV_Control_Center.Models
{
    class SQLCommunicator
    {
        private AGV_Control_CenterDataContext dbContext = new AGV_Control_CenterDataContext();

        public dbUser GetuserInfo(ApplicationUser user)
        {
            var query = (from row in dbContext.dbUsers
                         where row.Name == user.Name && row.Password == user.Password
                         select row).FirstOrDefault();         

            return query;
        }
        public bool InsertNewUser(ApplicationUser user)
        {
            bool successFlag = false;

            dbUser dbUserObject = new dbUser();

            dbUserObject.Name = user.Name;
            dbUserObject.Password = user.Password;
            dbUserObject.Group = user.Group.ToString();

            dbContext.dbUsers.InsertOnSubmit(dbUserObject);

            submitChanges();

            return successFlag;
        }

        public bool InsertExceptionInformation(Exception ex)
        {
            bool successFlag = false;

            dbApplicationErrorLog dbAppErrorLog = new dbApplicationErrorLog();

            dbAppErrorLog.Dump = ex.ToString();
            dbAppErrorLog.HashCode = ex.GetHashCode().ToString();
            dbAppErrorLog.HelpLink = ex.HelpLink;
            dbAppErrorLog.Message = ex.Message;
            dbAppErrorLog.Source = ex.Source;
            dbAppErrorLog.StackTrace = ex.StackTrace;

            dbContext.dbApplicationErrorLogs.InsertOnSubmit(dbAppErrorLog);

            submitChanges();

            return successFlag;
        }

        internal void LogUserOUT(ApplicationUser userProperty)
        {
            var query = (from row in dbContext.dbUserLogs
                        where row.UserId == userProperty.Id
                        orderby row.TimeStamp descending
                        select row).FirstOrDefault();

            query.LogoutTime = userProperty.LogOut;

            submitChanges();
        }

        private void submitChanges()
        {
            try
            {
                dbContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                ex.WriteLog().SaveToDataBase().Display();
            }
        }

        internal void LogUserIN(ApplicationUser userProperty)
        {
            dbUserLog userLog = new dbUserLog();

            userLog.UserId = userProperty.Id;
            userLog.UserName = userProperty.Name;
            userLog.UserGroup = userProperty.Group.ToString();
            userLog.LoginTime = userProperty.LogIn;
            userLog.LogoutTime = DateTime.MaxValue;
            dbContext.dbUserLogs.InsertOnSubmit(userLog);

            submitChanges();
        }
    }
}
