using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerASP.Model
{
    public class UserInfo
    {
        public string UserName { get; }
        public string UserId { get; }

        public string GroupName { get; set; }

        public UserInfo(string userName , string userId, string groupName)
        {
            UserName = userName;
            UserId = userId;
            GroupName = groupName;
        }
    }
}
