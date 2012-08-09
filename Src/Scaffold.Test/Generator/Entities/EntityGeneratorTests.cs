using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using Scaffold.Configuration;
using Scaffold.Generator.Entities;
using Scaffold.Io;
using Scaffold.Template;
using Scaffold.VisualStudio;

namespace Scaffold.Test.Generator.Entities
{
    [TestFixture]
    public class EntityGeneratorTests
    {
        private EntityGenerator entityGenerator;
        private ITemplateEngine templateEngineMock;
        private IConfiguration configurationMock;
        private IProjectFileManager projectFileManagerMock;
        private IFileSystem fileSystemMock;

        [SetUp]
        public void SetUp()
        {
            configurationMock = MockRepository.GenerateMock<IConfiguration>();
            projectFileManagerMock = MockRepository.GenerateMock<IProjectFileManager>();
            fileSystemMock = MockRepository.GenerateMock<IFileSystem>();

            templateEngineMock = new TemplateEngine(fileSystemMock);

            entityGenerator = new EntityGenerator(templateEngineMock, configurationMock, projectFileManagerMock);
        }

        [Test]
        public void ShouldGenerateEntity()
        {
            configurationMock.Stub(x => x.CoreNameSpace).Return("Blah.Core");

            entityGenerator.Execute(new List<string> { "Book" });

            VerifyGenerateEntity();
        }

        private void VerifyGenerateEntity()
        {
            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(@"Domain\Model\Book.cs", "Blah.Core"));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@"Blah.Core\Domain\Model\Book.cs"), Arg<string>.Is.Anything));

            string generatedRepository = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Entities.Book.example");
            IList<object[]> arguments = fileSystemMock.GetArgumentsForCallsMadeOn(x => x.FileWriteText(null, null));
            Assert.AreEqual(1, arguments.Count);
            Assert.AreEqual(generatedRepository, arguments[0][1]);
        }
    }
}