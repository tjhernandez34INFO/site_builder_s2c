namespace site_builder_s2c.Models
{
    public class S2cFile
    {
        public string FilePath { get; set; }
        public string FileData { get; set; }

        public S2cFile(string filepath, string data)
        {
            FilePath = filepath;
            FileData = data;
        }
    }
}

