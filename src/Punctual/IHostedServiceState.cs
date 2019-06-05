using System;

namespace Punctual
{
    public interface IHostedServiceState
    {
        DateTimeOffset NextRun { get; }
    }
}