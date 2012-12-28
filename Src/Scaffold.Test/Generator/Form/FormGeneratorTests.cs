using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Scaffold.Configuration;
using Scaffold.Entities;
using Scaffold.Generator.Form;
using Scaffold.Generator.Helpers;
using Scaffold.Io;
using Scaffold.Template;
using Scaffold.VisualStudio;

namespace Scaffold.Test.Generator.Form
{
    [TestFixture]
    public class FormGeneratorTests
    {
        private FormGenerator formGenerator;
        private IEntityManager entityManagerMock;
        private ITemplateEngine templateEngine;
        private IConfiguration configurationMock;
        private IProjectFileManager projectFileManagerMock;
        private IFileSystem fileSystemMock;
        private IAutoMapperHelper autoMapperHelperMock;

        [SetUp]
        public void SetUp()
        {
            entityManagerMock = MockRepository.GenerateMock<IEntityManager>();
            configurationMock = MockRepository.GenerateMock<IConfiguration>();
            projectFileManagerMock = MockRepository.GenerateMock<IProjectFileManager>();
            fileSystemMock = MockRepository.GenerateMock<IFileSystem>();
            autoMapperHelperMock = MockRepository.GenerateMock<IAutoMapperHelper>();

            templateEngine = new TemplateEngine(fileSystemMock);

            formGenerator = new FormGenerator(entityManagerMock, configurationMock, templateEngine, projectFileManagerMock, fileSystemMock, autoMapperHelperMock);
        }

        [Test]
        public void ShouldGenerateForm()
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

            formGenerator.Execute(new List<string>{"Book", "CreateBook"});

            VerifyGenerateController();
            VerifyGenerateControllerTests();
            VerifyGenerateViewModel();
            VerifyGenerateView();
            VerifyAutoMapperTests();
        }

        private void VerifyAutoMapperTests()
        {
            const string generatedFile = @"Common\AutoMapper\CreateBookAutoMapperTests.cs";
            const string @namespace = "Blah.Test";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(generatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + generatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Form.AutoMapperTests.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + generatedFile));      
        }

        private void VerifyGenerateView()
        {
            const string generatedFile = @"Views\CreateBook\Index.cshtml";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddContentFileToProject(generatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + generatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Form.Index.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + generatedFile));            
        }

        private void VerifyGenerateViewModel()
        {
            const string generatedFile = @"Models\CreateBookViewModel.cs";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(generatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + generatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Form.ViewModel.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + generatedFile));
        }

        private void VerifyGenerateControllerTests()
        {
            const string generatedFile = @"Controllers\CreateBookControllerTests.cs";
            const string @namespace = "Blah.Test";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(generatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + generatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Form.ControllerTests.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + generatedFile));
        }

        private void VerifyGenerateController()
        {
            const string generatedFile = @"Controllers\CreateBookController.cs";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(generatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + generatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Form.Controller.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + generatedFile));
        }

        private string GetGeneratedFile(string fileName)
        {
            IList<object[]> arguments = fileSystemMock.GetArgumentsForCallsMadeOn(x => x.FileWriteText(null, null));
            return (from argument in arguments where ((string)argument[0]) == fileName select (string)argument[1]).FirstOrDefault();
        }
    }
}