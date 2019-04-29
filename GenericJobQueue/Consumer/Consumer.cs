using System;
using System.Threading;
using System.Threading.Tasks;
using Generic.Queue;

namespace Generic.Consumer
{
    public class Consumer : IConsumer
    {
        private readonly IJobQueue _jobQueue;
        private CancellationToken _cancellationToken;

        public Consumer(IJobQueue jobQueue, CancellationToken cancellationToken)
        {
            this._jobQueue = jobQueue;
            this._cancellationToken = cancellationToken;
        }

        public void ConsumeJobs()
        {
            while (true)
            {
                if (_cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                if (!this._jobQueue.Empty())
                {
                    Task.Run(async () =>
                    {
                        IJob job = null;
                        if (this._jobQueue.TryDequeue(out job))
                        {
                            try
                            {
                                await job.DoJobAsync(_cancellationToken).ConfigureAwait(false);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                    });
                }
                else
                {
                    Thread.Sleep(500);
                }
            }
        }
    }
}