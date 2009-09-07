using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ncUtils
{
    /// <summary>
    /// Template class provides synchronized Queue for using in threads communication
    /// </summary>
    /// <typeparam name="DataType"></typeparam>
    public class SyncQueue<DataType>
    {
        private Queue<DataType> queue = new Queue<DataType>();
        private readonly Object locker = new Object();

        /// <summary>
        /// Dequeue first element from the queue or wait until it arrives if the queue is empty.
        /// </summary>
        /// <returns></returns>
        public DataType Dequeue()
        {
            lock (locker)
            {
                if (queue.Count == 0)
                {
                    Monitor.Wait(locker);
                }
                return queue.Dequeue();
            }
        }

        /// <summary>
        /// Clear all queue elements and return the last one or wait for first element if the queue is empty
        /// </summary>
        /// <returns></returns>
        public DataType DequeueAllGetLast()
        {
            lock (locker)
            {
                if (queue.Count == 0)
                {
                    Monitor.Wait(locker);
                }
                while (queue.Count > 1)
                    queue.Dequeue();
                return queue.Dequeue();
            }
        }

        /// <summary>
        /// Put the element into the end of the queue and notify all waiters.
        /// </summary>
        /// <param name="data"></param>
        public void Enqueue(DataType data)
        {
            lock (locker)
            {
                queue.Enqueue(data);
                Monitor.Pulse(locker);
            }
        }

        /// <summary>
        /// Return number of elements in the queue
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            lock (locker)
            {
                return queue.Count;
            }
        }
    }
}
