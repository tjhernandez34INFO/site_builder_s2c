using System.Diagnostics;

namespace site_builder_s2c.Helpers
{
    internal static class DataSaveHelper
    {
        public static void FetchAndSaveFile(string path, string url)
        {
            string command = "wget -O " + path + " " + url;
            ProcessStartInfo procStartInfo =
                new ProcessStartInfo("cmd", "/c " + command);
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            Process proc = new Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
        }
    }
}
