using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries.Models
{
    public class RecievedMessagesController :IEnumerable
    {
        ConcurrentQueue<byte[]> messageQueue = new ConcurrentQueue<byte[]>();

        public int Count
        {
            get
            {
                return messageQueue.Count();
            }
        }

        public void CopyTo(byte[][] array, int index)
        {
            messageQueue.CopyTo(array, index);
        }

        public IEnumerator<byte[]> GetEnumerator()
        {
            return messageQueue.GetEnumerator();
        }

        public byte[][] ToArray()
        {
            return messageQueue.ToArray();
        }

        public bool TryAdd(byte[] item)
        {
            messageQueue.Enqueue(item);
            return true;
        }

        public bool TryTake(out byte[] item)
        {
            return messageQueue.TryDequeue(out item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return messageQueue.GetEnumerator();
        }
    }
}
