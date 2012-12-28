using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Scaffold.Configuration;
using Scaffold.Generator.Helpers;
using Scaffold.Generator.Service;
using Scaffold.Io;
using Scaffold.Template;
using Scaffold.VisualStudio;

namespace Scaffold.Test.Generator.Service
{
    [TestFixture]
    public class ServiceGeneratorTests
    {
        private ServiceGenerator serviceGenerator;
        private IConfiguration configurationMock;
        private IProjectFileManager projectFileManagerMock;
        private IDependencyInjectionManager _dependencyInjectionManagerMock;
        private IFileSystem fileSystemMock;

        [SetUp]
        public void SetUp()
        {
            configurationMock = MockRepository.GenerateMock<IConfiguration>();
            projectFileManagerMock = MockRepository.GenerateMock<IProjectFileManager>();
            _dependencyInjectionManagerMock = MockRepository.GenerateMock<IDependencyInjectionManager>();
            fileSystemMock = MockRepository.GenerateMock<IFileSystem>();

            var templateEngine = new TemplateEngine(fileSystemMock);
            serviceGenerator = new ServiceGenerator(templateEngine, configurationMock, projectFileManagerMock, _dependencyInjectionManagerMock);
        }

        [Test]
        public void ShouldGenerateService()
        {
            configurationMock.Stub(x => x.TestNameSpace).Return("Blah.Test");
            configurationMock.Stub(x => x.CoreNameSpace).Return("Blah.Core");
            
            serviceGenerator.Execute(new List<string>{"MailService"});

            VerifyGenerateService();
            VerifyGenerateServiceInterface();
            VerifyGenerateServiceTests();
            VerifyAddToDependencyInjecttion();
        }

        private void VerifyAddToDependencyInjecttion()
        {
            _dependencyInjectionManagerMock.AssertWasCalled(x => x.AddToCoreDependencyInjection("IMailService", "MailService", new[] { "Blah.Core.Services", "Blah.Core.Services.Impl" }));
            _dependencyInjectionManagerMock.AssertWasCalled(x => x.AddToDependencyInjectionTest("IMailService", new[] { "Blah.Core.Services" }));
        }

        private void VerifyGenerateService()
        {
            const string generatedFile = @"Services\Impl\MailService.cs";
            const string @namespace = "Blah.Core";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(generatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + generatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Service.MailService.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + generatedFile));
        }
        private void VerifyGenerateServiceInterface()
        {
            const string generatedFile = @"Services\IMailService.cs";
            const string @namespace = "Blah.Core";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(generatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + generatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Service.IMailService.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + generatedFile));
        }

        private void VerifyGenerateServiceTests()
        {
            const string generatedFile = @"Services\MailServiceTests.cs";
            const string @namespace = "Blah.Test";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(generatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + generatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Service.MailServiceTests.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + generatedFile));
        }

        private string GetGeneratedFile(string fileName)
        {
            IList<object[]> arguments = fileSystemMock.GetArgumentsForCallsMadeOn(x => x.FileWriteText(null, null));
            return (from argument in arguments where ((string)argument[0]) == fileName select (string)argument[1]).FirstOrDefault();
        }
    }
}