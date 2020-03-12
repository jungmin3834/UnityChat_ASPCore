using ServerASP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerASP.Container
{
    public class UserContainer
    {
        public static UserContainer Singleton;

        public Dictionary<string, UserInfo> UserList = new Dictionary<string, UserInfo>();
        public Queue<UserInfo> GroupWaitList = new Queue<UserInfo>();
        public Dictionary<string, GroupControl> GroupList = new Dictionary<string, GroupControl>();
        public UserContainer()
        {
            if (Singleton == null)
                Singleton = this;
        }


    }
}
