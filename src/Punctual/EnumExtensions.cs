using System;
using System.Collections.Generic;
using System.Linq;

namespace Punctual
{
    /// <summary>
    /// Extension methods for <see cref="Enum"/>
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets a list of flags
        /// </summary>
        /// <param name="e"></param>
        /// <param name="singleBitFlagsOnly"></param>
        /// <returns></returns>
        public static IEnumerable<Enum> GetFlags(this Enum e, bool singleBitFlagsOnly = false)
        {
            var setFlags = Enum.GetValues(e.GetType()).Cast<Enum>().Where(e.HasFlag);

            if (singleBitFlagsOnly)
            {
                setFlags = setFlags.Where(f =>
                {
                    var value = Convert.ToInt32(f);

                    return value != 0 && (value & (value - 1)) == 0;
                });
            }

            return setFlags;
        }
    }
}