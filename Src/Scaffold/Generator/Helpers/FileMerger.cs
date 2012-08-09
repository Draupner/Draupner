using System;
using System.IO;
using System.Text;
using Scaffold.Io;

namespace Scaffold.Generator.Helpers
{
    public class FileMerger
    {
        private readonly IFileSystem fileSystem;

        public FileMerger(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void InsertFirstLine(string filePath, string insert, string ignorePattern)
        {
            string fileContent = fileSystem.FileReadText(filePath);
            if(fileContent.Contains(ignorePattern))
            {
                return;
            }

            string newFileContent = InsertFirst(fileContent, insert);

            fileSystem.FileWriteText(filePath, newFileContent);
        }

        private string InsertFirst(string fileContent, string insert)
        {
            var result = new StringBuilder();
            result.AppendLine(insert);
            using (var reader = new StringReader(fileContent))
            {
                for (var i = 0; reader.Peek() >= 0; i++)
                {
                    var line = reader.ReadLine();
                    result.AppendLine(line);
                }
            }
            return result.ToString();
            
        }

        public void InsertLineAfterLast(string filePath, string insert, string insertPattern, string ignorePattern)
        {
            string fileContent = fileSystem.FileReadText(filePath);
            if (fileContent.Contains(ignorePattern))
            {
                return;
            }

            string newFileContent = InsertAfterLast(fileContent, insert, insertPattern);

            fileSystem.FileWriteText(filePath, newFileContent);
        }

        public void InsertLineIntoMethod(string filePath, string insert, string methodName, string ignorePattern)
        {
            string fileContent = fileSystem.FileReadText(filePath);
            if (fileContent.Contains(ignorePattern))
            {
                return;
            }

            string newFileContent = InsertIntoMethod(fileContent, insert, methodName);

            fileSystem.FileWriteText(filePath, newFileContent);
        }

        private string InsertIntoMethod(string fileContent, string insert, string methodName)
        {
            int? methodStartLine = null;
            using (var reader = new StringReader(fileContent))
            {
                for (var i = 0; reader.Peek() >= 0; i++)
                {
                    var line = reader.ReadLine();
                    if (line != null && line.Contains(" " + methodName + "("))
                    {
                        if (reader.ReadLine().Contains("{"))
                        {
                            methodStartLine = i + 1;
                            break;
                        }
                        else
                        {
                            throw new Exception("Unable to insert into method. " + methodName+ " method appears to be broken");
                        }
                    }
                }
            }
            if (methodStartLine == null)
            {
                throw new Exception("Cannot insert into method, cannot find start of method ");
            }

            var result = new StringBuilder();
            using (var reader = new StringReader(fileContent))
            {
                for (var i = 0; reader.Peek() >= 0; i++)
                {
                    var line = reader.ReadLine();
                    result.AppendLine(line);
                    if (i == methodStartLine)
                    {
                        result.AppendLine(insert);
                    }
                }
            }

            return result.ToString();
        }

        public void InsertLineBefore(string filePath, string insert, string insertPattern, string ignorePattern)
        {
            string fileContent = fileSystem.FileReadText(filePath);
            if (fileContent.Contains(ignorePattern))
            {
                return;
            }

            string newFileContent = InsertBefore(fileContent, insert, insertPattern);

            fileSystem.FileWriteText(filePath, newFileContent);
        }

        private string InsertBefore(string fileContent, string insert, string insertPattern)
        {
            var result = new StringBuilder();
            var inserted = false;
            using(var reader = new StringReader(fileContent))
            {
                for (var i = 0; reader.Peek() >= 0; i++)
                {
                    var line = reader.ReadLine();
                    if (!inserted && line != null && line.Contains(insertPattern))
                    {
                        inserted = true;
                        result.AppendLine(insert);
                    }
                    result.AppendLine(line);
                }
            }
            return result.ToString();
        }

        private string InsertAfterLast(string fileContent, string insert, string insertPattern)
        {
            int? lastLine = null;
            using (var reader = new StringReader(fileContent))
            {
                for (var i = 0; reader.Peek() >= 0; i++)
                {
                    var line = reader.ReadLine();
                    if (line != null && line.Contains(insertPattern))
                    {
                        lastLine = i;
                    }
                }
            }
            if(lastLine == null)
            {
                throw new Exception("Cannot insert " + fileContent + ", cannot find: " + insertPattern);
            }

            var result = new StringBuilder();
            using (var reader = new StringReader(fileContent))
            {
                for (var i = 0; reader.Peek() >= 0; i++)
                {
                    var line = reader.ReadLine();
                    result.AppendLine(line);
                    if (i == lastLine)
                    {
                        result.AppendLine(insert);
                    }
                }
            }

            return result.ToString();
        }
    }
}