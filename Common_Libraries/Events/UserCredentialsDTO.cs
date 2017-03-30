using AGV_Control_Center.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGV_Control_Center.Events
{
    class UserCredentialsDTO : PubSubEvent<UserCredentialsDTO>
    {
        public ApplicationUser User { get; set; }
    }
}
