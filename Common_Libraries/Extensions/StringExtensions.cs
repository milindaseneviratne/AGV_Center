using Common_Libraries.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Libraries.Extensions
{
    public static class StringExtensions
    {
        public static UserGroups ToUserGroup(this string inputString)
        {
            UserGroups temp = UserGroups.Operator;

            try
            {
                 temp = (UserGroups)Enum.Parse(typeof(UserGroups), inputString);
            }
            catch (Exception ex)
            {
                ex.WriteLog().SaveToDataBase().Display();
            }

            return temp;
        }
    }
}
