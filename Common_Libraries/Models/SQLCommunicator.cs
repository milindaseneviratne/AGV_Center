﻿using Common_Libraries.Database;
using Common_Libraries.Enumerations;
using Common_Libraries.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Libraries.Models
{
    class SQLCommunicator
    {
        private AGV_Control_CenterDataContext dbContext = new AGV_Control_CenterDataContext();

        public dbUsers GetuserInfo(ApplicationUser user)
        {
            //var userLocal = new ApplicationUser();
            //var dbUserLocal = new dbUsers();

            var query = (from row in dbContext.dbUsers
                         where row.Name == user.Name && row.Password == user.Password
                         select row).FirstOrDefault();

            //if (query != null)
            //{
            //    dbUserLocal = query;

            //    userLocal.Id = dbUserLocal.Id;
            //    userLocal.Name = dbUserLocal.Name;
            //    userLocal.Password = dbUserLocal.Password;
            //    userLocal.Group = dbUserLocal.Group.ToUserGroup();
            //}           

            return query;
        }
        public bool InsertNewUser(ApplicationUser user)
        {
            bool successFlag = false;

            dbUsers dbUserObject = new dbUsers();

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
    }
}