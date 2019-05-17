using System;
using System.Collections.Generic;
using System.Linq;
using Generic.Queue;

namespace GenericQueue.JobQueue
{
    public class JobManager : IJobManager
    {
        private readonly int MAX = 10000;
        private Dictionary<Guid, JobStatus> _jobRecord;

        public JobManager()
        {
            this._jobRecord = new Dictionary<Guid, JobStatus>();
        }

        public bool AllJobsFinished()
        {
            lock (_jobRecord)
            {
                if (_jobRecord.Count == 0)
                {
                    return false;
                }
                else if (_jobRecord.Any(status => status.Value == JobStatus.Started))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public virtual void JobCompleted(Guid jobId)
        {
            if (_jobRecord.ContainsKey(jobId))
            {
                Update(jobId, JobStatus.Completed);
            }
            else
            {
                throw new KeyNotFoundException(string.Format(@"{0}", jobId));
            }
        }

        public void JobStarted(Guid jobId)
        {
            if (_jobRecord.ContainsKey(jobId))
            {
                Update(jobId, JobStatus.Started);
            }
            else
            {
                Add(jobId, JobStatus.Started);
            }
        }

        public void JobFailed(Guid jobId)
        {
            if (_jobRecord.ContainsKey(jobId))
            {
                Update(jobId, JobStatus.Failed);
            }
        }

        private void Update(Guid jobId, JobStatus status)
        {
            lock (_jobRecord)
            {
                _jobRecord[jobId] = status;
                if (_jobRecord.Count > MAX)
                {
                    var completed = _jobRecord.Where(s => s.Value == JobStatus.Completed).Select(s => s.Key).ToList();
                    foreach (var record in completed)
                    {
                        _jobRecord.Remove(record);
                    }
                }
            }
        }

        private void Add(Guid jobId, JobStatus status)
        {
            lock (_jobRecord)
            {
                _jobRecord.Add(jobId, status);
            }
        }

        public JobStatus GetJobStatus(Guid jobId)
        {
            lock (_jobRecord)
            {
                if (_jobRecord.ContainsKey(jobId))
                {
                    return _jobRecord[jobId];
                }
                throw new KeyNotFoundException(string.Format(@"{0}", jobId));
            }
        }
    }
}