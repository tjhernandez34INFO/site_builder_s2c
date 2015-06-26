using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CsQuery;
using site_builder_s2c.Helpers;

namespace site_builder_s2c.Models
{
    public class Factory
    {
        private Dictionary<string, string> DataDictionary { get; set; }
        private Dictionary<string, string> FilePathDictionary { get; set; }
        private List<string> FormattedSubdirectoryList { get; set; }
        private List<string> LessFileFilePaths { get; set; }
        private List<string> LessImportsStatementsList { get; set; }
        private List<string> FinalCssList { get; set; }
        private List<string> JavascriptFilesList { get; set; }
        private List<S2cFile> S2CFilesList { get; set; }
        private CQ SiteDomTagsForProcessing { get; set; }

        public void GenerateFilePaths(S2cSite site, string @filetype)
        {
            FilePathDictionary = new Dictionary<string, string>();
            Console.WriteLine("Generating {0} filepaths...", @filetype);
            switch (@filetype)
            {
                case "paths":
                    GeneratePathsAndDirectories(site);
                    break;
                case "json":
                    GenerateJsonResourceFilePaths(site);
                    break;
                case "view":
                    GenerateBulkFilePaths(FilePathDictionary, site.ViewsFilepath, "",
                        new List<string> {"TopSerpBanner.cshtml", "BottomSerpBanner.cshtml"});
                    break;
                case "less":
                    GenerateBulkFilePaths(FilePathDictionary, site.ResourcesFilepath, "css",
                        new List<string> {"insp-custom.less", "site.less"});
                    break;
                case "javascript":
                    GenerateBulkFilePaths(FilePathDictionary, site.ResourcesFilepath, "scripts",
                        new List<string> {"adstyles.js"});
                    break;
                case "image":
                    break;
            }
            Console.WriteLine("Finished");
        }

        public void FetchData(S2cSite site, string @filetype)
        {
            DataDictionary = new Dictionary<string, string>();
            Console.WriteLine(string.Format("Fetching {0} data...", @filetype));
            switch (@filetype)
            {
                case "paths":
                    break;
                case "json":
                    FetchJsonResourceData(site);
                    break;
                case "view":
                    FetchSerpBannerData(site);
                    break;
                case "less":
                    FetchLessData(site);
                    break;
                case "javascript":
                    FetchJavaScriptData(site);
                    break;
                case "image":
                    FetchImageData(site);
                    break;
            }
            Console.WriteLine("Finished");
        }

        public void TransformData(S2cSite site, string @filetype)
        {
            Console.WriteLine(string.Format("Processing {0} data...", @filetype));
            switch (@filetype)
            {
                case "paths":
                    break;
                case "json":
                    TransformJsonResourceData(site, FilePathDictionary, DataDictionary);
                    break;
                case "view":
                    TransformViewData(FilePathDictionary, DataDictionary);
                    break;
                case "less":
                    TransformLessData(site);
                    break;
                case "javascript":
                    JavaScriptSiteScriptTagsToS2CJavaScriptFiles(site);
                    break;
                case "image":
                    TransformImageData(site);
                    break;
            }
            Console.WriteLine("Finished");
        }

        public void WriteData(S2cSite site, string @filetype)
        {
            Console.WriteLine(string.Format("Saving {0} data...", @filetype));
            switch (@filetype)
            {
                case "paths":
                    WritePaths(site);
                    break;
                case "json":
                    WriteS2cFileData(S2CFilesList);
                    break;
                case "view":
                    WriteS2cFileData(S2CFilesList);
                    break;
                case "less":
                    WriteLessFileData(FilePathDictionary);
                    break;
                case "javascript":
                    WriteJavaScriptData(FilePathDictionary, DataDictionary);
                    break;
                case "image":
                    WriteImageData();
                    break;
            }
            Console.WriteLine("Finished");
        }


        // FILEPATHS METHODS
        private void GeneratePathsAndDirectories(S2cSite site)
        {
            FormattedSubdirectoryList = new List<string>();
            CreateSubdirectoryPaths(site);
        }

        private void CreateSubdirectoryPaths(S2cSite site)
        {
            foreach (var directory in site.SubdirectoriesList)
            {
                var subdirectory = directory == "partials"
                    ? site.ViewsFilepath + directory
                    : site.ResourcesFilepath + directory;
                FormattedSubdirectoryList.Add(subdirectory);
            }
        }

        private string GenerateFilePath(string basePath, string exstentionFile, string filename)
        {
            return exstentionFile == "" ? basePath + filename : basePath + exstentionFile + '\\' + filename;
        }

        private void GenerateJsonResourceFilePaths(S2cSite site)
        {
            FilePathDictionary.Add("rightrailfilepath",
                GenerateFilePath(site.RightRailFilepath, "", site.SiteName + ".json"));
            FilePathDictionary.Add("scripturifilepath",
                GenerateFilePath(site.ScriptUriFilepath, "", site.SiteName + ".json"));
        }

        private void GenerateBulkFilePaths(Dictionary<string, string> filePathDictionary, string baseFilePath,
            string fileExtension, List<string> fileNames)
        {
            foreach (var fileName in fileNames)
            {
                filePathDictionary.Add(fileName, GenerateFilePath(baseFilePath, fileExtension, fileName));
            }
        }


        //DATA FETCH METHODS

        private void FetchJsonResourceData(S2cSite site)
        {
            FetchRightRailData(site);
            GenerateInitialJavascriptFileList(site);
            S2CFilesList = new List<S2cFile>();
        }

        private void FetchRightRailData(S2cSite site)
        {
            DataDictionary.Add("rightraildata",
                "{" + "\n\t" + "\"data\": [" + "\n\t\t" + ArticleJsonProducer(site, 4) + "\n\t" + "]" + "\n" + "}");
        }

        private string ArticleJsonProducer(S2cSite site, int numberOfArticles)
        {
            List<string> articlePlaceholderList = new List<string>();
            string articlePlaceholder = "{" + "\n\t\t\t" + "\"url\" : \"http//www.example.com\"," + "\n\t\t\t" +
                                        "\"title\" : \"Example\"," + "\n\t\t\t" + "\"description\" : \"Change me in " +
                                        site.SiteName + ".json\"" +
                                        "\n\t\t" + "}";

            for (var i = 0; i < numberOfArticles; i++)
            {
                articlePlaceholderList.Add(articlePlaceholder);
            }

            return String.Join(",\n\t\t", articlePlaceholderList);
        }

        private void GenerateInitialJavascriptFileList(S2cSite site)
        {
            string adstylesFile = "\"~/content/" + site.SiteRoute + "/scripts/adstyles.js\"";
            JavascriptFilesList = new List<string>
            {
                "\"~/scripts/baseinit.js\"",
                "\"~/scripts/siteinit.js\"",
                adstylesFile
            };
        }

        private void FetchSerpBannerData(S2cSite site)
        {
            S2CFilesList = new List<S2cFile>();
            DataDictionary.Add("TopSerpBanner.cshtml", site.PageObject["header"].First().RenderSelection());
            DataDictionary.Add("BottomSerpBanner.cshtml", site.PageObject["footer"].First().RenderSelection());
        }

        private void FetchLessData(S2cSite site)
        {
            S2CFilesList = new List<S2cFile>();
            LessImportsStatementsList = new List<string>();
            SiteDomTagsForProcessing = site.PageObject["link"];
            FetchStyleCssFiles(site);
        }

        private void FetchStyleCssFiles(S2cSite site)
        {
            foreach (var styleTag in site.PageObject["style"])
            {
                var result = Regex.Match(styleTag.InnerText, "\"http.+\"");
                if (result.Success)
                {
                    LessImportsStatementsList.Add(result.ToString());
                }
            }
        }

        private void FetchJavaScriptData(S2cSite site)
        {
            SiteDomTagsForProcessing = site.PageObject["script"];
            DataDictionary.Add("adstyles.js",
                "wsNs = wsNs || {};\nwsNs.csr = wsNs.csr || {};\nwsNs.csr.adStyles = {\n\ttitleColor: '000000',\n\ttitleUnderline: 'false',\n\turlColor: '666666',\n\tbackgroundColor: 'fafafa',\n\ttextColor: '000'\n};");
        }

        private void FetchImageData(S2cSite site)
        {
            SiteDomTagsForProcessing = site.PageObject["img"];
            SiteDomTagsForProcessing.Add(FindFaviconTag(site));
        }

        //Transform Data Methods

        private void TransformJsonResourceData(S2cSite site, Dictionary<string, string> filePathDictionary,
            Dictionary<string, string> dataDictionary)
        {
            CreateScriptUri(site, JavascriptFilesList, site.PageObject["script"]);
            S2CFilesList.Add(new S2cFile(filePathDictionary["rightrailfilepath"], dataDictionary["rightraildata"]));
            S2CFilesList.Add(new S2cFile(filePathDictionary["scripturifilepath"], dataDictionary["scripturidata"]));
        }

        private void CreateScriptUri(S2cSite site, List<string> javascriptFilesList, CQ scriptsCq)
        {
            foreach (IDomObject scriptCq in scriptsCq)
            {
                var matchCase = Regex.Match(scriptsCq["src"].Text(), @"\.js");
                var scriptWithSource = scriptCq["type"] == "text/javascript" && scriptCq["src"] != null;
                var scriptWithFile = scriptCq["src"] != null && matchCase.Success;
                if (scriptWithSource || scriptWithFile)
                {
                    var rawFileName = scriptCq["src"].Split('/').Last();
                    var formattedFile = "\"~/content/" + site.SiteRoute + "/scripts/" +
                                        PathCleanerHelper.CleanJsPath(rawFileName) + "\"";
                    javascriptFilesList.Add(formattedFile);
                }
            }
            DataDictionary.Add("scripturidata",
                "{" + "\n\t" + "\"uris\": [" + "\n\t\t" + String.Join(",\n\t\t", javascriptFilesList) + "\n\t" +
                "]" + "\n" + "}");
        }

        private void TransformViewData(Dictionary<string, string> filePathDictionary,
            Dictionary<string, string> dataDictionary)
        {
            S2CFilesList.Add(new S2cFile(filePathDictionary["TopSerpBanner.cshtml"],
                dataDictionary["TopSerpBanner.cshtml"]));
            S2CFilesList.Add(new S2cFile(filePathDictionary["BottomSerpBanner.cshtml"],
                dataDictionary["BottomSerpBanner.cshtml"]));
        }

        private void TransformLessData(S2cSite site)
        {
            LessFileFilePaths = new List<string>();
            FinalCssList = new List<string>();
            InsertYahooOnlyStyleReference(site);
            GenerateLessFiles(site, SiteDomTagsForProcessing);
            ToLessFilesFromImportStatementList(site);
            CreateSiteLessFile();
        }

        private void InsertYahooOnlyStyleReference(S2cSite site)
        {
            if (!site.IsYahooOnly) return;
            var yahooFileRoute = site.IsBonnier ? "../../../css/yahoo.less" : "../../css/yahoo.less";
            LessFileFilePaths.Insert(1, yahooFileRoute);
        }

        private void GenerateLessFiles(S2cSite site, CQ linkTagsCq)
        {
            foreach (var linkTagCq in linkTagsCq)
            {
                var filename = linkTagCq["href"].Split('/').Last();
                if (linkTagCq["rel"] == "stylesheet" && RegexHelper.CssRegex().Match(filename).Success)
                {
                    var file = ConvertCssExtToLessExt(filename);
                    S2CFilesList.Add(CreateLessFile(GenerateFilePath(site.ResourcesFilepath, "css", file),
                        linkTagCq["href"]));
                }
            }
        }

        private void ToLessFilesFromImportStatementList(S2cSite site)
        {
            foreach (var url in LessImportsStatementsList)
            {
                string filename = ConvertCssExtToLessExt(PathCleanerHelper.CleanCSSPath(url));
                var path = GenerateFilePath(site.ResourcesFilepath, "css", filename);
                S2CFilesList.Add(CreateLessFile(path, url));
            }
        }

        private string ConvertCssExtToLessExt(string url)
        {
            return RegexHelper.CssRegex().Replace(url, ".less");
        }

        private static S2cFile CreateLessFile(string path, string url)
        {
            if (RegexHelper.LessRegex().Match(path).Success)
            {
                return new S2cFile(path.Split('?').FirstOrDefault(), url);
            }
            return null;
        }

        private void CreateSiteLessFile()
        {
            foreach (var file in S2CFilesList)
            {
                LessFileFilePaths.Add(file.FilePath.Split('\\').Last());
            }

            foreach (string filename in LessFileFilePaths)
            {
                if (RegexHelper.LessRegex().Match(filename).Success)
                {
                    FinalCssList.Add("@import \"" + filename + "\";\n");
                }
            }
        }

        private void JavaScriptSiteScriptTagsToS2CJavaScriptFiles(S2cSite site)
        {
            S2CFilesList = new List<S2cFile>();
            foreach (var tag in SiteDomTagsForProcessing)
            {
                bool validScriptTagConditionOne = (tag["type"] == "text/javascript" && tag["src"] != null);
                bool validScriptTagConditionTwo = (tag["src"] != null &&
                                                   RegexHelper.JavaScriptRegex().Match(tag["src"]).Success);
                if (validScriptTagConditionOne || validScriptTagConditionTwo)
                {

                    var filepath =
                        PathCleanerHelper.CleanJsPath(GenerateFilePath(site.ResourcesFilepath, "scripts",
                            tag["src"].Split('/').Last()));
                    if (RegexHelper.JavaScriptRegex().Match(filepath).Success)
                    {
                        S2CFilesList.Add(new S2cFile(filepath.Split('?').FirstOrDefault(), tag["src"]));
                    }
                }
            }
        }

        private void TransformImageData(S2cSite site)
        {
            S2CFilesList = new List<S2cFile>();
            StripFilesFromImageTags(site);
            StripImagesFromCssFiles(site);
        }

        private static IDomObject FindFaviconTag(S2cSite site)
        {
            IDomObject faviconTag = null;
            foreach (var linkTag in site.PageObject["link"])
            {
                if (linkTag["rel"] == "shortcut icon")
                {
                    faviconTag = linkTag;
                }
            }
            return faviconTag;
        }

        private void StripFilesFromImageTags(S2cSite site)
        {
            foreach (var tag in SiteDomTagsForProcessing)
            {
                if (tag["rel"] == "shortcut icon")
                {
                    S2CFilesList.Add(CreateImageFile(tag["href"], site));
                }
                else
                {
                    S2CFilesList.Add(tag["data-src"] != null
                        ? CreateImageFile(tag["data-src"], site)
                        : CreateImageFile(tag["src"], site));
                }

            }
        }

        private S2cFile CreateImageFile(string tag, S2cSite site)
        {
            var url = (RegexHelper.HttpRegex().Match(tag).Success) ? tag : "http:" + tag;
            return new S2cFile(GenerateFilePath(site.ResourcesFilepath, "img", tag.Split('?').FirstOrDefault()), url);
        }

        private void StripImagesFromCssFiles(S2cSite site)
        {
            var lessFiles = site._lessFilesFactory.S2CFilesList;
            foreach (S2cFile lessFile in lessFiles)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(lessFile.FilePath))
                    {
                        foreach (var url in sr.ReadToEnd().Split(' '))
                        {
                            var imageFilePath = GetImageFileNameFromCssUrl(url);
                            if (imageFilePath == null ||
                                !RegexHelper.FileExtensionRegex().Match(imageFilePath).Success) continue;
                            var imagePath = GenerateFilePath(site.ResourcesFilepath, "img",
                                imageFilePath.Split('/').Last().Split('?').FirstOrDefault());
                            var imageUrl = site.PageUrl + imageFilePath;
                            S2CFilesList.Add(new S2cFile(imagePath, imageUrl));
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }
            }

        }

        private string GetImageFileNameFromCssUrl(string urlImage)
        {
            if (RegexHelper.UrlFormatRegex().Match(urlImage).Success)
            {
                return RegexHelper.UrlFormatRegex().Match(urlImage).Result("${imageUrl}").Replace("\"", "");
            }
            return null;
        }

        // Write Data Methods

        private void WritePaths(S2cSite site)
        {
            Directory.CreateDirectory(site.RouteFilepath);
            foreach (var directoryPath in FormattedSubdirectoryList)
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        private void WriteS2cFileData(List<S2cFile> fileTypeList)
        {
            foreach (var file in fileTypeList)
            {
                File.WriteAllText(file.FilePath, file.FileData);
            }
        }

        private void WriteLessFileData(Dictionary<string, string> filePathDictionary)
        {
            File.WriteAllText(filePathDictionary["insp-custom.less"], "");
            foreach (var filename in FinalCssList.Distinct().Reverse().ToList())
            {
                File.AppendAllText(filePathDictionary["site.less"], filename);
            }

            foreach (var lessFile in S2CFilesList)
            {
                DataSaveHelper.FetchAndSaveFile(lessFile.FilePath, lessFile.FileData);
            }
        }

        private void WriteJavaScriptData(Dictionary<string, string> filePathDictionary,
            Dictionary<string, string> dataDictionary)
        {
            File.WriteAllText(filePathDictionary["adstyles.js"], dataDictionary["adstyles.js"]);

            foreach (var scriptFile in S2CFilesList)
            {
                DataSaveHelper.FetchAndSaveFile(scriptFile.FilePath, scriptFile.FileData);
            }
        }

        private void WriteImageData()
        {
            foreach (var imageFile in S2CFilesList)
            {
                var isBase64Encoded = Regex.Match(imageFile.FilePath, @".base64.");
                if (!isBase64Encoded.Success && RegexHelper.FileExtensionRegex().Match(imageFile.FilePath).Success)
                {
                    DataSaveHelper.FetchAndSaveFile(imageFile.FilePath, imageFile.FileData);
                }
            }
        }
    }
}



