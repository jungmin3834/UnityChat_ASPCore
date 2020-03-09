using Microsoft.AspNetCore.SignalR;
using ServerASP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ServerASP.Hubs
{
    public class ConnectHub : Hub
    {
        Dictionary<string, UserInfo> userList = new Dictionary<string, UserInfo>();
        Stack<UserInfo> groupWaitList = new Stack<UserInfo>();
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public Task ConnectionList(string userName)
        {
            userList.Add(Context.ConnectionId, new UserInfo(userName, Context.ConnectionId, ""));
            return Clients.User(Context.ConnectionId).SendAsync("ConnectionState","Success");
        }

        public Task JoinGroupList()
        {
            UserInfo connections;
            if (!userList.TryGetValue(Context.ConnectionId, out connections))
                groupWaitList.Push(connections);

            lock (groupWaitList)
            {
                if (groupWaitList.Count > 2)
                {
                    string groupName = "test";
                    //Group 생성
                    for (int i = 2; i > 0; i--)
                    {
                        UserInfo info = groupWaitList.Pop();
                        userList[info.UserId].GroupName = groupName;
                        Groups.AddToGroupAsync(info.UserId, groupName);
                    }
                    return Clients.Group("test").SendAsync("BroadCastMessage", "System", "접속 성공");
                }
                else
                    return Clients.Caller.SendAsync("ConnectionState", "Succecss");
            }
        }

        public Task SendGroupMessage(string message)
        {
            UserInfo connections;
            if (!userList.TryGetValue(Context.ConnectionId, out connections))
                return Clients.Group(connections.GroupName).SendAsync("BroadCastMessage", connections.UserName, message);
            else
                return Clients.Caller.SendAsync("BroadCastMessage", "System", "접속 에러");
        }
    }
}
