using System;
using System.Threading;
using System.Threading.Tasks;
using Generic.Consumer;
using Generic.Queue;
using GenericQueue.JobQueue;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericJobQueueTests
{
    [TestClass]
    public class JobQueueTests
    {
        private IJobQueue _jobQueue;
        private IConsumer _consumer;
        private IJobManager _jobManager;
        private Guid _jobId;
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;

        public JobQueueTests()
        {
            Initialize();
        }
        public void Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            _jobQueue = new JobQueue();
            _jobId = Guid.NewGuid();
            _jobManager = new JobManager();
            _consumer = new Consumer(_jobQueue, _cancellationToken);
            Task.Run(() => {
                _consumer.ConsumeJobs();
            });
        }

        [TestCleanup]
        private void Cleanup()
        {
            _cancellationTokenSource.Cancel();
        }

        private IJob CreateTestJob()
        {
            return new TestJob(
                jobId: _jobId,
                action : () => 
                {
                    _jobManager.JobStarted(_jobId);
                    return true;
                },
                onCompletion: () => 
                {
                    _jobManager.JobCompleted(_jobId);
                    return true;
                }
                );
        }

        [TestMethod]
        public async void Add_Job_To_Queue()
        {
            _jobQueue.Enqueue(CreateTestJob());
            var result = await _jobManager.GetJobStatus(_jobId);
            Assert.AreEqual(JobStatus.Completed, result);
        }
    }
}
