using System;
using System.Collections.Generic;
using site_builder_s2c.Models;

namespace site_builder_s2c
{
    class Program
    {
        static void Main()
        {
            Dictionary<string,dynamic> siteArguments = new Dictionary<string,dynamic>
            {
                {"url", "http://www.sportrider.com/find/bike"},
                {"siteRouteName", "sport_rider"},
                {"bonnier", true},
                {"yahooOnly", false}
            };
            S2cSite site = new S2cSite(siteArguments);

            site.Build(site._pathsAndDirectoriesFactory, "paths");
            site.Build(site._jsonFilesFactory, "json");
            site.Build(site._viewsFactory, "view");
            site.Build(site._lessFilesFactory, "less");
            site.Build(site._javaScriptFilesFactory, "javascript");
            site.Build(site._imagesFactory, "image");
            Console.WriteLine("Site Complete press enter to exit...");
            Console.ReadLine();

        }
    }
}
