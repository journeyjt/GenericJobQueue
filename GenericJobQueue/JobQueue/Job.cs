using System;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Queue
{
    public class Job<T> : IJob
    {
        private Func<T> _action;
        private Func<T> _onCompletion;
        private Func<T> _onFailure;
        public Guid JobId { get; set; }

        public Job(Guid jobId, Func<T> action, Func<T> onCompletion = null, Func<T> onFailure = null)
        {
            _action = action;
            _onCompletion = onCompletion;
            _onFailure = onFailure;
            JobId = jobId;
        }

        public T ActionReturnValue { get; private set; }
        public T OnCompleteReturnValue { get; private set; }
        public T OnFailureReturnValue { get; private set; }

        public void DoJob(CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    ActionReturnValue = _action.Invoke();
                    if (_onCompletion != null)
                    {
                        OnCompleteReturnValue = _onCompletion.Invoke();
                    }
                }
                catch (Exception e)
                {
                    if (_onFailure != null)
                    {
                        OnFailureReturnValue = _onFailure.Invoke();
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
        }
    }
}