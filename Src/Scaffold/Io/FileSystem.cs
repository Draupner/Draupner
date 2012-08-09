using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Scaffold.Io
{
    public class FileSystem : IFileSystem
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public string FileReadText(string path)
        {
            return File.ReadAllText(path);
        }

        public void FileWriteText(string path, string text)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.Write(text);
            }
        }

        public TextReader FileTextStream(string path)
        {
            return new StreamReader(path);
        }

        public void CopyDirectory(string sourcePath, string destPath, Regex fileIgnore, Regex directoryIgnore)
        {
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            foreach (string file in Directory.GetFiles(sourcePath))
            {
                var fileName = Path.GetFileName(file);

                if (!IsIgnored(fileName, fileIgnore))
                {
                    string destination = Path.Combine(destPath, fileName);
                    Console.WriteLine("Writing: " + destination);
                    File.Copy(file, destination, true);
                }
            }

            foreach (string folder in Directory.GetDirectories(sourcePath))
            {
                var directoryName = Path.GetFileName(folder);
                if (!IsIgnored(directoryName, directoryIgnore))
                {
                    string destination = Path.Combine(destPath, directoryName);
                    CopyDirectory(folder, destination, fileIgnore, directoryIgnore);
                }
            }
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        private static bool IsIgnored(string name, Regex ignore)
        {
            return name == null || (ignore != null && ignore.IsMatch(name));
        }
    }
}