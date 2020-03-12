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
                return Clients.Caller.SendAsync("ConnectionState", $"Success Fail");
            }
    
        }
        public Task JoinGroupList(string groupName)
        {

            UserInfo connections; 
            if (UserContainer.Singleton.UserList.TryGetValue(Context.ConnectionId, out connections) == false)
                return Clients.Caller.SendAsync("ConnectionState", $"Fail {Context.ConnectionId}");

            lock (UserContainer.Singleton.GroupWaitList)
            {
                UserContainer.Singleton.GroupWaitList.Enqueue(connections);

                //그룹 인원 체크
                if (UserContainer.Singleton.GroupWaitList.Count < 2)
                    return Clients.Caller.SendAsync("ConnectionState", "Waitting...");

                //test = 랜덤으로 생성되야할 이름 : groupName은 나중에 친구가 방 만든경우
                string group = groupName.Count() == 0 ? "test" : groupName;
                //Group 생성
                Queue<UserInfo> userlist = new Queue<UserInfo>();

                for (int i = 2; i > 0; i--)
                {
                    userlist.Enqueue(UserContainer.Singleton.GroupWaitList.Dequeue());
                    UserContainer.Singleton.UserList[userlist.Last().UserId].GroupName = group;
                    Groups.AddToGroupAsync(userlist.Last().UserId, group);
                }

                GroupControl groupControl = new GroupControl(userlist);
                UserContainer.Singleton.GroupList.Add(group, groupControl);
                return Clients.Group("test").SendAsync("BroadcastMessage", "System", "그룹 접속 성공");
            }
        }

        public async Task SendGroupMessage(string message)
        {
            GroupControl groupControl;
            if (UserContainer.Singleton.GroupList.TryGetValue("test", out groupControl) == false
                || groupControl._playerOrder.Peek().UserId != Context.ConnectionId)
            {
                await Clients.Caller.SendAsync("BroadcastMessage", "System", "자기 차례가 아닙니다.");
                return;
            }
            
            groupControl.PlayerOrderPopPush();

            UserInfo connections;
            if (UserContainer.Singleton.UserList.TryGetValue(Context.ConnectionId, out connections) == true)
                await Clients.All.SendAsync("BroadcastMessage", "check", connections.UserName + connections.GroupName);
            else
                await Clients.Caller.SendAsync("BroadcastMessage", "System", "접속 에러");
        }
    }
}
