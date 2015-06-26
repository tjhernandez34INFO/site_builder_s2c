using System.Text.RegularExpressions;

namespace site_builder_s2c.Helpers
{
    internal static class PathCleanerHelper
    {
        public static string CleanJsPath(string filename)
        {
            var matchCase = Regex.Match(filename, @"\.js.+");
            var regEx = new Regex(@"\.js.+");
            if (matchCase.Length > 0)
            {
                filename = regEx.Replace(filename, ".js");
            }
            return filename;
        }

        public static string CleanCSSPath(string filename)
        {
            var matchCase = Regex.Match(filename, @"\.css.+");
            var regEx = new Regex(@"\.css.+");
            if (matchCase.Length > 0)
            {
                regEx.Replace(filename, ".css");
            }
            return filename;
        }
    }
}
