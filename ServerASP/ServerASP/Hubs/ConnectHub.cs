using Microsoft.AspNetCore.SignalR;
using ServerASP.Container;
using ServerASP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ServerASP.Hubs
{
    public class ConnectHub : Hub
    {

        public Task ConnectionList(string userName)
        {
            lock(UserContainer.Singleton.UserList){
                if (UserContainer.Singleton.UserList.TryAdd(Context.ConnectionId, new UserInfo(userName, Context.ConnectionId, "")))
                    return Clients.Caller.SendAsync("ConnectionState", $"Success {Context.ConnectionId}");
                else
                    return Clients.Caller.SendAsync("ConnectionState", $"Success Fail");
            }
    
        }
        public Task JoinGroupList(string groupName)
        {
            lock(UserContainer.Singleton.UserList)
            {
              
                UserInfo connections;
                if (UserContainer.Singleton.UserList.TryGetValue(Context.ConnectionId, out connections) == false)
                    return Clients.Caller.SendAsync("ConnectionState", $"Fail {Context.ConnectionId}");
                UserContainer.Singleton.GroupWaitList.Enqueue(connections);

                
                 if (UserContainer.Singleton.GroupWaitList.Count > 1)
                 {
                     string group = groupName.Count() == 0 ? "test" : groupName;
                     //Group 생성
                     for (int i = 2; i > 0; i--)
                     {
                        UserInfo info = UserContainer.Singleton.GroupWaitList.Dequeue();
                        UserContainer.Singleton.UserList[info.UserId].GroupName = group;
                        Clients.All.SendAsync("BroadcastMessage", "", $"Player 접속 : {info.UserId}");
                        Groups.AddToGroupAsync(info.UserId, group);
                        Clients.Group(group).SendAsync("BroadcastMessage", "System", $"{Context.ConnectionId} joined {groupName}");
                    }
                   
                      return Clients.Group("test").SendAsync("BroadcastMessage", "System", "그룹 접속 성공");
                 }


                return Clients.Caller.SendAsync("ConnectionState", $"Fail {UserContainer.Singleton.GroupWaitList.Count} :  {Context.ConnectionId}");
     
            }
        }

        public async Task SendGroupMessage(string message)
        {
            UserInfo connections;
            if (UserContainer.Singleton.UserList.TryGetValue(Context.ConnectionId, out connections) == true)
            {
                await Clients.All.SendAsync("BroadcastMessage", "check", connections.UserName + connections.GroupName);
                await Clients.Group("test").SendAsync("BroadcastMessage", "System", "그룹 접속 성공");
                return;
            }
            else
                await Clients.All.SendAsync("BroadcastMessage", "Fail ", message);

            await Clients.Caller.SendAsync("BroadcastMessage", "System", "접속 에러");
        }
    }
}
