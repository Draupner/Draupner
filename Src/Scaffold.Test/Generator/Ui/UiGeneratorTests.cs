using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Scaffold.Configuration;
using Scaffold.Entities;
using Scaffold.Generator.Helpers;
using Scaffold.Generator.Ui;
using Scaffold.Io;
using Scaffold.Template;
using Scaffold.VisualStudio;

namespace Scaffold.Test.Generator.Ui
{
    [TestFixture]
    public class UiGeneratorTests
    {
        private UiGenerator uiGenerator;
        private IConfiguration configurationMock;
        private IProjectFileManager projectFileManagerMock;
        private IDepencyInjectionManager depencyInjectionManagerMock;
        private IFileSystem fileSystemMock;
        private IEntityManager entityManagerMock;
        private IAutoMapperHelper autoMapperHelperMock;

        [SetUp]
        public void SetUp()
        {
            configurationMock = MockRepository.GenerateMock<IConfiguration>();
            projectFileManagerMock = MockRepository.GenerateMock<IProjectFileManager>();
            depencyInjectionManagerMock = MockRepository.GenerateMock<IDepencyInjectionManager>();
            fileSystemMock = MockRepository.GenerateMock<IFileSystem>();
            entityManagerMock = MockRepository.GenerateMock<IEntityManager>();
            autoMapperHelperMock = MockRepository.GenerateMock<IAutoMapperHelper>();

            var templateEngine = new TemplateEngine(fileSystemMock);
            uiGenerator = new UiGenerator(entityManagerMock, templateEngine, configurationMock, projectFileManagerMock, depencyInjectionManagerMock, autoMapperHelperMock, fileSystemMock);
        }

        [Test]
        public void ShouldGenerateUi()
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

            uiGenerator.Execute(new List<string>{"Book"});

            VerifyGenerateController();
            VerifyGenerateViewModel();
            VerifyCreateViewFolder();
            VerifyGenerateIndexView();
            VerifyGenerateCreateView();
            VerifyGenerateDetailsView();
            VerifyGenerateEditView();
            VerifyGenerateControllerTests();
            VerifyGenerateAutoMapperTests();
            VerifyAddToAutoMapper();
        }

        private void VerifyCreateViewFolder()
        {
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory("Blah.Web\\Views\\Book"));
        }

        private void VerifyGenerateController()
        {
            const string genereatedFile = @"Controllers\BookController.cs";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Ui.BookController.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateViewModel()
        {
            const string genereatedFile = @"Models\BookViewModel.cs";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Ui.BookViewModel.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateIndexView()
        {
            const string genereatedFile = @"Views\Book\Index.cshtml";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddContentFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Ui.Index.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateCreateView()
        {
            const string genereatedFile = @"Views\Book\Create.cshtml";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddContentFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Ui.Create.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateDetailsView()
        {
            const string genereatedFile = @"Views\Book\Details.cshtml";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddContentFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Ui.Details.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateEditView()
        {
            const string genereatedFile = @"Views\Book\Edit.cshtml";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddContentFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Ui.Edit.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateControllerTests()
        {
            const string genereatedFile = @"Controllers\BookControllerTests.cs";
            const string @namespace = "Blah.Test";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Ui.BookControllerTests.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateAutoMapperTests()
        {
            const string genereatedFile = @"Common\AutoMapper\BookAutoMapperTests.cs";
            const string @namespace = "Blah.Test";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Ui.BookAutoMapperTests.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyAddToAutoMapper()
        {
            autoMapperHelperMock.AssertWasCalled(x => x.AddAutoMapperConfiguration("Book", "BookViewModel"));
        }

        private string GetGeneratedFile(string fileName)
        {
            IList<object[]> arguments = fileSystemMock.GetArgumentsForCallsMadeOn(x => x.FileWriteText(null, null));
            return (from argument in arguments where ((string)argument[0]) == fileName select (string)argument[1]).FirstOrDefault();
        }
    }
}