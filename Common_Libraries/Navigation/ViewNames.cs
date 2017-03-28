using Common_Libraries.Views;
using System;
using System.Collections.Generic;
using System.Linq;
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

    }
}
