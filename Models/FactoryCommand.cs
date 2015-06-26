namespace site_builder_s2c.Models
{
    class FactoryCommand : Command
    {
        private Factory _factory;
        private S2cSite _site;
        private string _filetype;

        public FactoryCommand(Factory factory, S2cSite site, string @filetype)
        {
            _factory = factory;
            _site = site;
            _filetype = @filetype;
        }

        public string Filetype
        {
            set { _filetype = value; }
        }

         public override void Execute()
        {
            _factory.GenerateFilePaths(_site, _filetype);
            _factory.FetchData(_site, _filetype);
            _factory.TransformData(_site, _filetype);
            _factory.WriteData(_site, _filetype);
        }
    }
}
