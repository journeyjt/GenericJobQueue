using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generic.Queue;

namespace GenericJobQueueTests
{
    public class TestJob : Job<bool>
    {
        public TestJob(
            Guid jobId, 
            Func<bool> action, 
            Func<bool> onCompletion = null, 
            Func<bool> onFailure = null) 
            : base(
                  jobId, 
                  action, 
                  onCompletion, 
                  onFailure
                  )
        {
        }
    }
}
