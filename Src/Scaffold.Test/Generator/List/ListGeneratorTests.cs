using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Scaffold.Configuration;
using Scaffold.Entities;
using Scaffold.Generator.List;
using Scaffold.Io;
using Scaffold.Template;
using Scaffold.VisualStudio;

namespace Scaffold.Test.Generator.List
{
    [TestFixture]
    public class ListGeneratorTests
    {
        private ListGenerator listGenerator;
        private IEntityManager entityManagerMock;
        private ITemplateEngine templateEngine;
        private IConfiguration configurationMock;
        private IProjectFileManager projectFileManagerMock;
        private IFileSystem fileSystemMock;

        [SetUp]
        public void SetUp()
        {
            entityManagerMock = MockRepository.GenerateMock<IEntityManager>();
            configurationMock = MockRepository.GenerateMock<IConfiguration>();
            projectFileManagerMock = MockRepository.GenerateMock<IProjectFileManager>();
            fileSystemMock = MockRepository.GenerateMock<IFileSystem>();

            templateEngine = new TemplateEngine(fileSystemMock);

            listGenerator = new ListGenerator(entityManagerMock, configurationMock, templateEngine, projectFileManagerMock, fileSystemMock);
        }

        [Test]
        public void ShouldGenerateList()
        {
            var entity = new Entity
            {
                Name = "Book",
                Properties = new List<EntityProperty>
                                                  {
                                                      new EntityProperty{Name =  "Id", Type = "System.Int64"},
                                                      new EntityProperty{Name =  "Name", Type = "System.String"}
                                                  }

            };
            configurationMock.Stub(x => x.TestNameSpace).Return("Blah.Test");
            configurationMock.Stub(x => x.CoreNameSpace).Return("Blah.Core");
            configurationMock.Stub(x => x.WebNameSpace).Return("Blah.Web");

            entityManagerMock.Stub(x => x.ReadEntity("Book")).Return(entity);

            listGenerator.Execute(new List<string>{"Book", "MyBook"});

            VerifyGenerateController();
            VerifyGenerateControllerTests();
            VerifyGenerateViewModel();
            VerifyGenerateView();
            VerifyCreateDirectory();
        }

        private void VerifyCreateDirectory()
        {
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory("Blah.Web\\Views\\MyBook"));
        }

        private void VerifyGenerateController()
        {
            const string genereatedFile = @"Controllers\MyBookController.cs";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.List.MyBookController.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateControllerTests()
        {
            const string genereatedFile = @"Controllers\MyBookControllerTests.cs";
            const string @namespace = "Blah.Test";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.List.MyBookControllerTests.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateViewModel()
        {
            const string genereatedFile = @"Models\MyBookViewModel.cs";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.List.MyBookViewModel.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateView()
        {
            const string genereatedFile = @"Views\MyBook\Index.cshtml";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddContentFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.List.Index.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private string GetGeneratedFile(string fileName)
        {
            IList<object[]> arguments = fileSystemMock.GetArgumentsForCallsMadeOn(x => x.FileWriteText(null, null));
            return (from argument in arguments where ((string)argument[0]) == fileName select (string)argument[1]).FirstOrDefault();
        }
    }
}