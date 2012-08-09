using System.IO;

namespace Scaffold.Test
{
    public class EmbeddedResourceReader
    {
        public static string ReadEmbeddedResource(string name)
        {
            var stream = typeof(EmbeddedResourceReader).Assembly.GetManifestResourceStream(name);
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        } 
    }
}