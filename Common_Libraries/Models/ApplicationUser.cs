using Common_Libraries.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Libraries.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserGroups Group { get; set; }
        public string Password { get; set; }
    }
}
