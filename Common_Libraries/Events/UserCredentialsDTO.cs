using Common_Libraries.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Libraries.Events
{
    class UserCredentialsDTO : PubSubEvent<UserCredentialsDTO>
    {
        public ApplicationUser User { get; set; }
    }
}
