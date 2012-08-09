using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Scaffold.Configuration;
using Scaffold.Entities;
using Scaffold.Generator.NHibernate;
using Scaffold.Io;
using Scaffold.Template;
using Scaffold.VisualStudio;

namespace Scaffold.Test.Generator.NHibernate
{
    [TestFixture]
    public class NHibernateMapGeneratorTests
    {
        private NHibernateMapGenerator generator;

        private IEntityManager entityManagerMock;
        private ITemplateEngine templateEngine;
        private IConfiguration configurationMock;
        private IProjectFileManager projectFileManagerMock;
        private IFileSystem fileSystemMock;

        [SetUp]
        public void SetUp()
        {
            entityManagerMock = MockRepository.GenerateMock<IEntityManager>();
            fileSystemMock = MockRepository.GenerateMock<IFileSystem>();
            configurationMock = MockRepository.GenerateMock<IConfiguration>();
            projectFileManagerMock = MockRepository.GenerateMock<IProjectFileManager>();

            templateEngine = new TemplateEngine(fileSystemMock);

            generator = new NHibernateMapGenerator(entityManagerMock, templateEngine, configurationMock, projectFileManagerMock, fileSystemMock);
        }

        [Test]
        public void ShouldGenerateNHibernateMap()
        {
            var args = new List<string>{"Book"};

            StubReadEntity();
            StubConfiguration();
            StubAutoFixtureCustomization();

            generator.Execute(args);

            VerifyGenerateMapping();
            VerifyGenerateMappingTests();
            VerifyGenerateAutoFixtureCustomization();
        }

        private void StubAutoFixtureCustomization()
        {
            const string fileContent = @"using Ploeh.AutoFixture;

namespace Blah.Test
{
    public class AutoFixtureCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
        }
    }
}";
            fileSystemMock.Stub(x => x.FileReadText("Blah.Test\\AutoFixtureCustomization.cs")).Return(fileContent);
        }

        private void StubConfiguration()
        {
            configurationMock.Stub(x => x.CoreNameSpace).Return("Blah.Core");
            configurationMock.Stub(x => x.TestNameSpace).Return("Blah.Test");
        }

        private void StubReadEntity()
        {
            var entity = new Entity
                             {
                                 Name = "Book",
                                 Properties = new List<EntityProperty>
                                                  {
                                                      new EntityProperty {Name = "Id", Type = "System.Int64"},
                                                      new EntityProperty {Name = "Title", Type = "System.String"},
                                                      new EntityProperty {Name = "Author", Type = "System.String"}
                                                  }
                             };

            entityManagerMock.Stub(x => x.ReadEntity("Book")).Return(entity);
        }

        private void VerifyGenerateMapping()
        {
            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(@"Common\NHibernate\BookMap.cs", "Blah.Core"));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@"Blah.Core\Common\NHibernate\BookMap.cs"), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.NHibernate.BookMap.example");
            Assert.AreEqual(expected, GetGeneratedFile(@"Blah.Core\Common\NHibernate\BookMap.cs"));
        }

        private void VerifyGenerateMappingTests()
        {
            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(@"Common\NHibernate\BookPersistenceTests.cs", "Blah.Test"));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@"Blah.Test\Common\NHibernate\BookPersistenceTests.cs"), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.NHibernate.BookPersistenceTests.example");
            Assert.AreEqual(expected, GetGeneratedFile(@"Blah.Test\Common\NHibernate\BookPersistenceTests.cs"));
        }

        private void VerifyGenerateAutoFixtureCustomization()
        {
            const string expectedfileContent = @"using Ploeh.AutoFixture;

namespace Blah.Test
{
    public class AutoFixtureCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Book>(x => x.Without(y => y.Id));
        }
    }
}
";

            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal("Blah.Test\\AutoFixtureCustomization.cs"), Arg<string>.Is.Anything));
            Assert.AreEqual(expectedfileContent, GetGeneratedFile("Blah.Test\\AutoFixtureCustomization.cs"));
        }

        private string GetGeneratedFile(string fileName)
        {
            IList<object[]> arguments = fileSystemMock.GetArgumentsForCallsMadeOn(x => x.FileWriteText(null, null));
            return (from argument in arguments where ((string) argument[0]) == fileName select (string) argument[1]).FirstOrDefault();
        }
    }
}