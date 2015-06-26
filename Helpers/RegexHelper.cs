using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace site_builder_s2c.Helpers
{
    internal static class RegexHelper
    {

        public static Regex CssRegex()
        {
            return new Regex(@"\.css");
        }

        public static Regex JavaScriptRegex()
        {
            return new Regex(@"\.js");
        }

        public static Regex LessRegex()
        {
            return new Regex(@"\.less");
        }

        public static Regex HttpRegex()
        {
            return new Regex(@"^http");

        }        
        
        public static Regex UrlFormatRegex()
        {
            return new Regex(@"url\((?<imageUrl>[^;)]+)\)");

        }

        public static Regex FileExtensionRegex()
        {
            return new Regex(@"\.\D+$");
        }
    }

}
