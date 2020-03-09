using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerASP.Model
{
    public class GameInfo
    {
        private bool _isEnd { get; set; }
        private string[] _idArray;
        private int _size = 0;
        public GameInfo()
        {
            _idArray = new string[4];
        }

        public void SetSize(int size)
        {
            _size = size;
        }

        public string[] GetConnectionArray()
        {
            return _idArray;
        }

        public bool AddConnectionToArray(int idx ,string id)
        {
            _idArray[idx] = id;
            _size++;
            if (_size.Equals(4))
                return true;
            return false;
        }
    }
}
