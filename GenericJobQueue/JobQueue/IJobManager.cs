using System;
using Generic.Queue;

namespace GenericQueue.JobQueue
{
    public interface IJobManager
    {
        bool AllJobsFinished();

        void JobStarted(Guid jobId);

        void JobCompleted(Guid jobId);

        void JobFailed(Guid jobId);

        JobStatus GetJobStatus(Guid jobId);
    }
}