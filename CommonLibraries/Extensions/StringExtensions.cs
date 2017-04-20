using CommonLibraries.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries.Extensions
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

        public static string RemoveEOF(this string inputString)
        {
            return inputString.Replace("<EOF>", "");
        }
        public static string AddEOF(this string inputString)
        {
            return inputString + "<EOF>";
        }

        public static string Validate(this string inputString)
        {
            if (inputString.Contains("OK"))
            {
                return "OK";
            }

            return "ERROR";
        }
    }
}
