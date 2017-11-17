using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LandMark.Common.TCP
{
    public class BufferPool
    {

        private int sign = 1;
        private object pushLock = new object();
        private object popLock = new object();
        private bool bl = true;
        public BufferPool()
        {
            
        }

        public void Push()
        {

        }

        public string Pop()
        {
            return null;
        }
    }
}
