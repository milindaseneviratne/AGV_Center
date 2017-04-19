using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibraries.Database;
using CommonLibraries.Extensions;

namespace CommonLibraries.Models
{
    public class SQLCommunicator
    {
        private sqlDataContext dbContext = new sqlDataContext();

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

            //dbContext.dbApplicationErrorLogs.InsertOnSubmit(dbAppErrorLog);

            submitChanges();

            return successFlag;
        }

        public void LogUserOUT(ApplicationUser userProperty)
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

        public void LogUserIN(ApplicationUser userProperty)
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
