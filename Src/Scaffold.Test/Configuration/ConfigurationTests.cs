using System.IO;
using NUnit.Framework;
using Rhino.Mocks;
using Scaffold.Configuration;
using Scaffold.Exceptions;
using Scaffold.Io;

namespace Scaffold.Test.Configuration
{
    [TestFixture]
    public class ConfigurationTests
    {
        private IConfiguration configuration;
        private MockRepository mocks;
        private IFileSystem fileSystemMock;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();

            fileSystemMock = mocks.DynamicMock<IFileSystem>();

            configuration = new ConfigurationReader(fileSystemMock);
        }

        [Test]
        public void ShouldReadConfiguration()
        {
            const string configurationFile = @"<Configuration>
	<CoreNameSpace>Foo.Core</CoreNameSpace>
	<WebNameSpace>Foo.Web</WebNameSpace>
	<TestNameSpace>Foo.Test</TestNameSpace>
	<ProjectName>Foo</ProjectName>
</Configuration>";

            Expect.Call(fileSystemMock.FileExists("Scaffold.xml")).Return(true);
            Expect.Call(fileSystemMock.FileTextStream("Scaffold.xml")).Return(new StringReader(configurationFile));

            mocks.ReplayAll();

            Assert.AreEqual("Foo.Core", configuration.CoreNameSpace);
            Assert.AreEqual("Foo.Test", configuration.TestNameSpace);
            Assert.AreEqual("Foo.Web", configuration.WebNameSpace);
            Assert.AreEqual("Foo", configuration.ProjectName);

            mocks.VerifyAll();
        }

        [Test, ExpectedException(typeof(ConfigurationFileNotFoundException))]
        public void ShouldNotBeAbleToFindConfigurationFile()
        {
            Expect.Call(fileSystemMock.FileExists("Scaffold.xml")).Return(false);

            mocks.ReplayAll();

            string coreNameSpace = configuration.CoreNameSpace;

            mocks.VerifyAll();
        }
    }
}