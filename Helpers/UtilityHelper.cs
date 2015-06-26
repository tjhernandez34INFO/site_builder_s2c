using System.Collections.Generic;
using System.Linq;

namespace site_builder_s2c.Helpers
{
    internal static class UtilityHelper
    {
        public static IEnumerable<string> SortByLength(IEnumerable<string> list)
        {
            var sorted = from word in list
                         orderby word.Length ascending
                         select word;
            return sorted;
        }

    }

}
