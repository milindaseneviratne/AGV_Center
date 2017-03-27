using AGV_CIMCenter;
using Common_Libraries.Extensions;
using Common_Libraries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Libraries.ViewModels
{
    public class LoginViewModel
    {
        private SQLCommunicator sqldbCommunicator = new SQLCommunicator();

        public bool Login(ApplicationUser user)
        {
            bool successFlag = false;
            //sqldbCommunicator.InsertNewUser(user);
            var dbUserInfo = sqldbCommunicator.GetuserInfo(user);

            if (dbUserInfo == null)
            {
                successFlag = false;
            }
            else
            {
                user.Id = dbUserInfo.Id;
                user.Name = dbUserInfo.Name;
                user.Password = dbUserInfo.Password;
                user.Group = dbUserInfo.Group.ToUserGroup();

                AGV_Center_Launcher agvCIMCenterLauncher = new AGV_Center_Launcher();
                agvCIMCenterLauncher.Show();

                successFlag = true;

            }
            return successFlag;
        }
    }
}
