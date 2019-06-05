using System;
using System.Reflection;

namespace Punctual.Internal
{
    internal class ScheduledActionAliasUtilities
    {
        private const string AliasAttibuteTypeFullName = "Punctual.ScheduledActionAliasAttribute";
        private const string AliasAttibuteAliasProperty = "Alias";

        internal static string GetAlias(Type providerType)
        {
            foreach (var attribute in providerType.GetTypeInfo().GetCustomAttributes(inherit: false))
            {
                if (attribute.GetType().FullName == AliasAttibuteTypeFullName)
                {
                    var valueProperty = attribute
                        .GetType()
                        .GetProperty(AliasAttibuteAliasProperty, BindingFlags.Public | BindingFlags.Instance);

                    if (valueProperty != null)
                    {
                        return valueProperty.GetValue(attribute) as string;
                    }
                }
            }

            return providerType.Name;
        }
    }
}
