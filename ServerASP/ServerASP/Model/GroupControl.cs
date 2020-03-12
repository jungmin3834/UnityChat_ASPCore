using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerASP.Model
{
    public class GroupControl
    {
        public Queue<UserInfo> _playerOrder { get; }
        public GroupControl(Queue<UserInfo> playerlist)
        {
            _playerOrder = playerlist;
        }

        public bool CheckIsPlayerTime(string userId)
        {
            return _playerOrder.Peek().UserId != userId ? false : true;
        }

        public void PlayerOrderPopPush()
        {
            _playerOrder.Enqueue(_playerOrder.Dequeue());
        }
    }
}
