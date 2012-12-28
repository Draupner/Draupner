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
        private IDependencyInjectionManager _dependencyInjectionManagerMock;
        private IFileSystem fileSystemMock;
        private IEntityManager entityManagerMock;
        private IAutoMapperHelper autoMapperHelperMock;

        [SetUp]
        public void SetUp()
        {
            configurationMock = MockRepository.GenerateMock<IConfiguration>();
            projectFileManagerMock = MockRepository.GenerateMock<IProjectFileManager>();
            _dependencyInjectionManagerMock = MockRepository.GenerateMock<IDependencyInjectionManager>();
            fileSystemMock = MockRepository.GenerateMock<IFileSystem>();
            entityManagerMock = MockRepository.GenerateMock<IEntityManager>();
            autoMapperHelperMock = MockRepository.GenerateMock<IAutoMapperHelper>();

            var templateEngine = new TemplateEngine(fileSystemMock);
            uiGenerator = new UiGenerator(entityManagerMock, templateEngine, configurationMock, projectFileManagerMock, _dependencyInjectionManagerMock, autoMapperHelperMock, fileSystemMock);
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
            const string generatedFile = @"Controllers\BookController.cs";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(generatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + generatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Ui.BookController.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + generatedFile));
        }

        private void VerifyGenerateViewModel()
        {
            const string generatedFile = @"Models\BookViewModel.cs";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(generatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + generatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Ui.BookViewModel.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + generatedFile));
        }

        private void VerifyGenerateIndexView()
        {
            const string generatedFile = @"Views\Book\Index.cshtml";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddContentFileToProject(generatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + generatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Ui.Index.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + generatedFile));
        }

        private void VerifyGenerateCreateView()
        {
            const string generatedFile = @"Views\Book\Create.cshtml";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddContentFileToProject(generatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + generatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Ui.Create.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + generatedFile));
        }

        private void VerifyGenerateDetailsView()
        {
            const string generatedFile = @"Views\Book\Details.cshtml";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddContentFileToProject(generatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + generatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Ui.Details.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + generatedFile));
        }

        private void VerifyGenerateEditView()
        {
            const string generatedFile = @"Views\Book\Edit.cshtml";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddContentFileToProject(generatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + generatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Ui.Edit.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + generatedFile));
        }

        private void VerifyGenerateControllerTests()
        {
            const string generatedFile = @"Controllers\BookControllerTests.cs";
            const string @namespace = "Blah.Test";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(generatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + generatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Ui.BookControllerTests.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + generatedFile));
        }

        private void VerifyGenerateAutoMapperTests()
        {
            const string generatedFile = @"Common\AutoMapper\BookAutoMapperTests.cs";
            const string @namespace = "Blah.Test";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(generatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + generatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Ui.BookAutoMapperTests.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + generatedFile));
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