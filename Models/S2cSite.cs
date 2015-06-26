using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery;
using site_builder_s2c.Helpers;

namespace site_builder_s2c.Models
{
    public class S2cSite
    {
        public string PageUrl { get; set; }
        public CQ PageObject { get; set; }
        public string SiteName { get; set; }
        private string ViewFolder { get; set; }
        public List<string> SubdirectoriesList { get; set; }
        public bool IsBonnier { get; set; }
        public bool IsYahooOnly { get; set; }
        public string SiteRoute { get; set; }
        public string RouteFilepath { get; set; }
        public string ViewsFilepath { get; set; }
        public string ResourcesFilepath { get; set; }
        public string RightRailFilepath { get; set; }
        public string ScriptUriFilepath { get; set; }
        public Factory _pathsAndDirectoriesFactory { get; set; }
        public Factory _jsonFilesFactory { get; set; }
        public Factory _viewsFactory { get; set; }
        public Factory _lessFilesFactory { get; set; }
        public Factory _javaScriptFilesFactory { get; set; }
        public Factory _imagesFactory { get; set; }
        public S2cSite(Dictionary<string,dynamic> siteDictionary)
        {
            SubdirectoriesList = new List<string> { "RightRailContent", "ScriptUris", "partials", "img", "css", "scripts" };
            PageUrl = SetPageUrl(siteDictionary["url"]);
            PageObject = SetPageObject(siteDictionary["url"]);
            SiteName = SetSiteName(siteDictionary["siteRouteName"]);
            ViewFolder = SetViewFolder(siteDictionary["siteRouteName"]);
            IsBonnier = siteDictionary["bonnier"];
            IsYahooOnly = siteDictionary["yahooOnly"];
            SiteRoute = SetSiteRoute(siteDictionary["bonnier"], SiteName);
            //ViewsFilepath = "C:\\insp\\site_s2c\\SerpToContent\\Views\\Shared\\" + ViewFolder + "\\";
            //ResourcesFilepath = "C:\\insp\\site_s2c\\SerpToContent\\content\\" + SiteName + "\\";
            //RightRailFilepath = "C:\\insp\\site_s2c\\SerpToContent\\Resources\\RightRailContent\\";
            //ScriptUriFilepath = "C:\\insp\\site_s2c\\SerpToContent\\Resources\\ScriptUris\\";
            RouteFilepath = "C:\\Users\\thernan\\Desktop\\automation\\" + SiteName;
            ViewsFilepath = "C:\\Users\\thernan\\Desktop\\automation\\" + SiteName + "\\partials\\";
            ResourcesFilepath = "C:\\Users\\thernan\\Desktop\\automation\\" + SiteName + "\\";
            RightRailFilepath = "C:\\Users\\thernan\\Desktop\\automation\\" + SiteName + "\\RightRailContent\\";
            ScriptUriFilepath = "C:\\Users\\thernan\\Desktop\\automation\\" + SiteName + "\\ScriptUris\\";
            _pathsAndDirectoriesFactory = new Factory();
            _jsonFilesFactory = new Factory();
            _viewsFactory = new Factory();
            _lessFilesFactory = new Factory();
            _javaScriptFilesFactory = new Factory();
            _imagesFactory = new Factory();
        }
        private static string SetPageUrl(string rawPageUrl)
        {
            var webAddress = UtilityHelper.SortByLength(rawPageUrl.Split('/')).Last();
            return "http://" + webAddress;
        }

        private static CQ SetPageObject(string rawPageUrl)
        {
            return CQ.CreateFromUrl(rawPageUrl);
        }

        private static string SetSiteName(string siteRouteName)
        {
            return String.Join("", siteRouteName.Split('_'));
        }

        private static string SetViewFolder(string siteRouteName)
        {
            List<string> joinedRouteName = new List<string>();
            foreach (string word in siteRouteName.Split('_'))
            {
                joinedRouteName.Add(char.ToUpper(word[0]) + word.Substring(1));
            }
            return String.Join("", joinedRouteName);
        }

        private static string SetSiteRoute(bool bonnier, string siteName)
        {
            return bonnier ? "bonnier/" + siteName : siteName;
        }
        

        public void Build(Factory factory, string type)
        {
            Command command = new FactoryCommand(
             factory, this, type);
            command.Execute();
        }        
    }
}
