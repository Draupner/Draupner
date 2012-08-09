using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Scaffold.Configuration;
using Scaffold.Entities;
using Scaffold.Generator.Helpers;
using Scaffold.Generator.Repository;
using Scaffold.Io;
using Scaffold.Template;
using Scaffold.VisualStudio;

namespace Scaffold.Test.Generator.Repository
{
    [TestFixture]
    public class RepositoryGeneratorTests
    {
        private RepositoryGenerator repositoryGenerator;
        private IEntityManager entityManagerMock;
        private ITemplateEngine templateEngineMock;
        private IConfiguration configurationMock;
        private IProjectFileManager projectFileManagerMock;
        private IDepencyInjectionManager depencyInjectionManagerMock;
        private IFileSystem fileSystemMock;

        [SetUp]
        public void SetUp()
        {
            entityManagerMock = MockRepository.GenerateMock<IEntityManager>();
            configurationMock = MockRepository.GenerateMock<IConfiguration>();
            projectFileManagerMock = MockRepository.GenerateMock<IProjectFileManager>();
            depencyInjectionManagerMock = MockRepository.GenerateMock<IDepencyInjectionManager>();
            fileSystemMock = MockRepository.GenerateMock<IFileSystem>();

            templateEngineMock = new TemplateEngine(fileSystemMock);

            repositoryGenerator = new RepositoryGenerator(entityManagerMock, templateEngineMock, configurationMock, projectFileManagerMock, depencyInjectionManagerMock);
        }

        [Test]
        public void ShouldGenerateRepository()
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
            entityManagerMock.Stub(x => x.ReadEntity("Book")).Return(entity);

            repositoryGenerator.Execute(new List<string>{"Book"});

            VerifyGenerateRepository();
            VerifyGenerateRepositoryInterface();
            VerifyGenerateRepositoryTests();
        }

        private void VerifyGenerateRepository()
        {
            const string genereatedFile = @"Repositories\BookRepository.cs";
            const string @namespace = "Blah.Core";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Repository.BookRepository.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateRepositoryTests()
        {
            const string genereatedFile = @"Repositories\BookRepositoryTests.cs";
            const string @namespace = "Blah.Test";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Repository.BookRepositoryTests.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateRepositoryInterface()
        {
            const string genereatedFile = @"Domain\Repositories\IBookRepository.cs";
            const string @namespace = "Blah.Core";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Repository.IBookRepository.example");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private string GetGeneratedFile(string fileName)
        {
            IList<object[]> arguments = fileSystemMock.GetArgumentsForCallsMadeOn(x => x.FileWriteText(null, null));
            return (from argument in arguments where ((string)argument[0]) == fileName select (string)argument[1]).FirstOrDefault();
        }
    }
}