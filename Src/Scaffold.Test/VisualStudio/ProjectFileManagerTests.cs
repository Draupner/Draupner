using System.IO;
using System.Reflection;
using NUnit.Framework;
using Rhino.Mocks;
using Scaffold.Io;
using Scaffold.VisualStudio;

namespace Scaffold.Test.VisualStudio
{
    [TestFixture]
    public class ProjectFileManagerTests
    {
        private MockRepository mocks;
        private IFileSystem fileSystemMock;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            fileSystemMock = mocks.DynamicMock<IFileSystem>();
        }

        [Test]
        public void ShouldAddFileToProjectFile()
        {
            const string projectName = "FooProject";
            const string projectFile = projectName + "\\" + projectName + ".csproj";

            string projectFileContent = ReadExampleProjectFile();

            Expect.Call(fileSystemMock.FileReadText(projectFile)).Return(projectFileContent);
            Expect.Call(() => fileSystemMock.FileWriteText(projectFile, null))
                .IgnoreArguments()
                .WhenCalled(invocation => Assert.True(((string)invocation.Arguments[1]).Contains("<Compile Include=\"Bar.cs\" />")));

            mocks.ReplayAll();

            var projectFileManager = new ProjectFileManager(fileSystemMock);
            projectFileManager.AddCompileFileToProject("Bar.cs", projectName);

            mocks.VerifyAll();
        }

        private string ReadExampleProjectFile()
        {
            var stream = GetType().Assembly.GetManifestResourceStream("Scaffold.Test.VisualStudio.Project.csproj.example");
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}