using System.IO;
using System.Text.RegularExpressions;

namespace Scaffold.Io
{
    public interface IFileSystem
    {
        bool FileExists(string path);
        string FileReadText(string path);
        void FileWriteText(string path, string text);
        TextReader FileTextStream(string path);
        void CopyDirectory(string sourcePath, string destPath, Regex fileIgnore, Regex directoryIgnore);

        void CreateDirectory(string path);
    }
}