namespace Scaffold.Configuration
{
    public class Configuration : IConfiguration
    {
        public string ProjectName { get; set; }
        public string CoreNameSpace { get; set; }
        public string WebNameSpace { get; set; }
        public string TestNameSpace { get; set; }
    }
}
