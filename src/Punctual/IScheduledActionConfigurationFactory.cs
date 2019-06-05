using Microsoft.Extensions.Configuration;
using System;

namespace Punctual
{
    /// <summary>
    /// Interface for a factory that returns configurations for a scheduled action
    /// </summary>
    public interface IScheduledActionConfigurationFactory
    {
        /// <summary>
        /// Returns a configuration based on the type of scheduled action
        /// </summary>
        /// <param name="ScheduledActionType"></param>
        /// <returns></returns>
        IConfiguration GetConfiguration(Type ScheduledActionType);
    }
}