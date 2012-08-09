using System.IO;
using System.Xml.Serialization;
using Scaffold.Exceptions;
using Scaffold.Io;

namespace Scaffold.Configuration
{
    public class ConfigurationReader : IConfiguration
    {
        private readonly IFileSystem fileSystem;
        private const string ConfigurationFile = "Scaffold.xml";
        private Configuration configuration;

        public ConfigurationReader(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        private Configuration ReadConfiguration()
        {
            if(configuration != null)
            {
                return configuration;
            }

            if (!fileSystem.FileExists(ConfigurationFile))
            {
                throw new ConfigurationFileNotFoundException("Configuration file not found");
            }

            using(var reader = fileSystem.FileTextStream(ConfigurationFile))
            {
                var serializer = new XmlSerializer(typeof(Configuration));
                configuration = (Configuration)serializer.Deserialize(reader);                
            }
            return configuration;
        }

        public string ProjectName
        {
            get { return ReadConfiguration().ProjectName; }
        }

        public string CoreNameSpace
        {
            get { return ReadConfiguration().CoreNameSpace; }
        }

        public string WebNameSpace
        {
            get { return ReadConfiguration().WebNameSpace; }
        }

        public string TestNameSpace
        {
            get { return ReadConfiguration().TestNameSpace; }
        }
    }
}
