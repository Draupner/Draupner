using System.IO;

namespace Scaffold.Template
{
    public interface ITemplateEngine
    {
        void GenerateFromTemplate<T>(string templateName, string outputFile, T templateModel);
        void GenerateFromTemplate<T>(TextReader templateStream, string outputFile, T templateModel);
    }
}