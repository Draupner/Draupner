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
        private IDepencyInjectionManager depencyInjectionManagerMock;
        private IFileSystem fileSystemMock;

        [SetUp]
        public void SetUp()
        {
            configurationMock = MockRepository.GenerateMock<IConfiguration>();
            projectFileManagerMock = MockRepository.GenerateMock<IProjectFileManager>();
            depencyInjectionManagerMock = MockRepository.GenerateMock<IDepencyInjectionManager>();
            fileSystemMock = MockRepository.GenerateMock<IFileSystem>();

            var templateEngine = new TemplateEngine(fileSystemMock);
            serviceGenerator = new ServiceGenerator(templateEngine, configurationMock, projectFileManagerMock, depencyInjectionManagerMock);
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
            depencyInjectionManagerMock.AssertWasCalled(x => x.AddToCoreDependencyInjection("IMailService", "MailService", new[] { "Blah.Core.Services", "Blah.Core.Services.Impl" }));
            depencyInjectionManagerMock.AssertWasCalled(x => x.AddToDependencyInjectionTest("IMailService", new[] { "Blah.Core.Services" }));
        }

        private void VerifyGenerateService()
        {
            const string genereatedFile = @"Services\Impl\MailService.cs";
            const string @namespace = "Blah.Core";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Service.MailService.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }
        private void VerifyGenerateServiceInterface()
        {
            const string genereatedFile = @"Services\IMailService.cs";
            const string @namespace = "Blah.Core";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Service.IMailService.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateServiceTests()
        {
            const string genereatedFile = @"Services\MailServiceTests.cs";
            const string @namespace = "Blah.Test";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Service.MailServiceTests.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private string GetGeneratedFile(string fileName)
        {
            IList<object[]> arguments = fileSystemMock.GetArgumentsForCallsMadeOn(x => x.FileWriteText(null, null));
            return (from argument in arguments where ((string)argument[0]) == fileName select (string)argument[1]).FirstOrDefault();
        }
    }
}