using AGV_Control_Center.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AGV_Control_Center.Navigation
{
    public static class ViewNames
    {
        public static string Login
        {
            get
            {
                return typeof(Login).Name;
            }
        }

        public static string CommandClient
        {
            get
            {
                return typeof(CommandClient).Name;
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
        public static string AGV_Control_Center_Home
        {
            get
            {
                return typeof(AGV_Control_Center_Home).Name;
            }
        }
        public static string ApplicationConfiguration
        {
            get
            {
                return typeof(ApplicationConfiguration).Name;
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
