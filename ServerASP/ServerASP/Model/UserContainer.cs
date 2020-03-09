using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerASP.Model
{
    public class UserContainer
    {
        List<UserInfo> infolist = new List<UserInfo>();

        public void Add(UserInfo info)
        {
            infolist.Add(info);
        }



        Dictionary<string, HashSet<string>> _groupList = new Dictionary<string, HashSet<string>>();
    }
}
