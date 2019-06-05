using System;

namespace Punctual
{
    /// <summary>
    /// Alias used to configure an action with a certain name in the configuration
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ScheduledActionAliasAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new alias
        /// </summary>
        /// <param name="alias"></param>
        public ScheduledActionAliasAttribute(string alias)
        {
            Alias = alias;
        }

        /// <summary>
        /// The alias to use
        /// </summary>
        public string Alias { get; }

    }
}