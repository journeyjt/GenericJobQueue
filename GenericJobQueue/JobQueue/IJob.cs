﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Queue
{
    public interface IJob
    {
        void DoJob(CancellationToken cancellationToken);

        Guid JobId { get; set; }
    }

    public enum JobStatus { Started, Completed, Failed }
}