using Common_Libraries.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common_Libraries.Navigation
{
    public static class ViewNames
    {
        public static string LoginPage
        {
            get
            {
                return typeof(Login).Name;
            }
        }

        public static string SubmitCommand
        {
            get
            {
                return typeof(SubmitCommand).Name;
            }
        }

        public static string ApplicationExplorer
        {
            get
            {
                return typeof(ApplicationExplorer).Name;
            }
        }
        public static string CommandServer 
        {
            get
            {
                return typeof(CommandServer).Name;
            }
        }
        //public static IEnumerator GetEnumerator()
        //{
        //    Type type = typeof(ViewNames);
        //    PropertyInfo[] properties = type.GetProperties();

        //    foreach (PropertyInfo property in properties)
        //    {
        //        yield return property;
        //    }
        //}
    }
}
