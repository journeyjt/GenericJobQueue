﻿using System.Collections.Generic;
using System.Threading;

namespace Generic.Queue
{
    public class JobQueue : IJobQueue
    {
        private readonly Queue<IJob> _queue;

        public JobQueue()
        {
            this._queue = new Queue<IJob>();
        }

        public void Enqueue(IJob job)
        {
            lock (this._queue)
            {
                this._queue.Enqueue(job);
                if (this._queue.Count >= 1)
                {
                    // wake up blocked dequeue
                    Monitor.PulseAll(this._queue);
                }
            }
        }

        public bool TryDequeue(out IJob job)
        {
            lock (this._queue)
            {
                if (this._queue.Count == 0)
                {
                    job = default(IJob);
                    return false;
                }
                job = this._queue.Dequeue();
                return true;
            }
        }

        public bool Empty()
        {
            lock (this._queue)
            {
                return this._queue.Count == 0;
            }
        }

        public int Size()
        {
            lock (this._queue)
            {
                return this._queue.Count;
            }
        }
    }
}