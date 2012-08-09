using System;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Scaffold.Io;

namespace Scaffold.Test.Io
{
    [TestFixture]
    public class FileSystemTests
    {
        private IFileSystem fileSystem;

        [SetUp]
        public void SetUp()
        {
            if(Directory.Exists("Foo"))
            {
                Directory.Delete("Foo", true);                
            }
            if(Directory.Exists("Copy"))
            {
                Directory.Delete("Copy", true);                
            }

            fileSystem = new FileSystem();

            CreateDirectoryWithFiles();
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete("Foo", true);
            Directory.Delete("Copy", true);
        }

        [Test]
        public void ShouldCopyDirectory()
        {
            fileSystem.CopyDirectory("Foo", "Copy", null, null);

            Assert.True(File.Exists("Copy\\Test1.txt"));
            Assert.True(File.Exists("Copy\\Bar\\Test2.template"));
            Assert.True(File.Exists("Copy\\Bar\\Test3.txt"));
        }

        [Test]
        public void ShouldCopyDirectoryButIgnoreFile()
        {
            fileSystem.CopyDirectory("Foo", "Copy", new Regex("^.*\\.template"), null);

            Assert.True(File.Exists("Copy\\Test1.txt"));
            Assert.True(!File.Exists("Copy\\Bar\\Test2.template"));
            Assert.True(File.Exists("Copy\\Bar\\Test3.txt"));
        }

        [Test]
        public void ShouldCopyDirectoryButIgnoreDirectory()
        {
            fileSystem.CopyDirectory("Foo", "Copy", null, new Regex("Bar"));

            Assert.True(File.Exists("Copy\\Test1.txt"));
            Assert.True(!File.Exists("Copy\\Bar\\Test2.template"));
            Assert.True(!File.Exists("Copy\\Bar\\Test3.txt"));
        }

        private void CreateDirectoryWithFiles()
        {
            Directory.CreateDirectory("Foo");
            WriteFile("Foo\\Test1.txt");
            Directory.CreateDirectory("Foo\\Bar");
            WriteFile("Foo\\Bar\\Test2.template");
            WriteFile("Foo\\Bar\\Test3.txt");
        }

        private void WriteFile(string fileName)
        {
            using(var writer = new StreamWriter(fileName))
            {
                writer.Write(Guid.NewGuid().ToString());
            }
        }
    }
}