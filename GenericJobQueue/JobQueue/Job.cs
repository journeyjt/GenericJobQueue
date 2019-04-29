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
            JobId = JobId;
        }

        public Task DoJobAsync(CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    _action.Invoke();
                    if (_onCompletion != null)
                    {
                        _onCompletion.Invoke();
                    }
                }
                catch (Exception e)
                {
                    if (_onFailure != null)
                    {
                        _onFailure.Invoke();
                    }
                    throw e;
                }
            }
            else
            {
                return Task.FromResult(cancellationToken);
            }
            return Task.FromResult(true);
        }
    }
}