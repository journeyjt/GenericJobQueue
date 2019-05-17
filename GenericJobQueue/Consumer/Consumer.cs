using System;
using System.Threading;
using System.Threading.Tasks;
using Generic.Queue;
using GenericQueue.JobQueue;

namespace Generic.Consumer
{
    public class Consumer : IConsumer
    {
        private readonly IJobQueue _jobQueue;
        private readonly IJobManager _jobManager;
        private CancellationToken _cancellationToken;

        public Consumer(IJobQueue jobQueue, IJobManager jobManager, CancellationToken cancellationToken)
        {
            this._jobQueue = jobQueue;
            this._jobManager = jobManager;
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
                    Task.Run(() =>
                    {
                        IJob job = null;
                        if (this._jobQueue.TryDequeue(out job))
                        {
                            try
                            {
                                _jobManager.JobStarted(job.JobId);
                                job.DoJob(_cancellationToken);
                                _jobManager.JobCompleted(job.JobId);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                _jobManager.JobFailed(job.JobId);
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