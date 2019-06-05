using System;

namespace Punctual
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ScheduledActionAliasAttribute : Attribute
    {
        public ScheduledActionAliasAttribute(string alias)
        {
            Alias = alias;
        }

        public string Alias { get; }

    }
}