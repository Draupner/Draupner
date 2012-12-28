using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Scaffold.Entities;
using Scaffold.Exceptions;
using Scaffold.Io;

namespace Scaffold.Test.Entities
{
    [TestFixture]
    public class EntityReaderTests
    {
        private IEntityReader entityReader;
        private MockRepository mocks;
        private IFileSystem fileSystemMock;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();

            fileSystemMock = mocks.DynamicMock<IFileSystem>();

            entityReader = new EntityReader(fileSystemMock);
        }

        [Test]
        public void ShouldReadEntity()
        {
            const string entitySourceCode = @"namespace Foo.Core.Domain.Model
                                                {
                                                    public class Person
                                                    {
                                                        public virtual long Id { get; set; }
                                                        public virtual string Name { get; set; }
                                                        public virtual ICollection<Role> Roles { get; set; }
                                                        public virtual Role CurrentRole { get; set; }
                                                    }
                                                }";
            const string path = "\\Foo.Core\\Domain\\Model\\Person.cs";

            Expect.Call(fileSystemMock.FileExists(path)).Return(true);
            Expect.Call(fileSystemMock.FileReadText(path)).Return(entitySourceCode);

            mocks.ReplayAll();

            var entity = entityReader.ReadEntity(path);

            mocks.VerifyAll();

            Assert.AreEqual("Person", entity.Name);
            Assert.AreEqual(4, entity.Properties.Count);
            Assert.AreEqual(2, entity.BasicProperties.Count);
            Assert.AreEqual(1, entity.CollectionProperties.Count);
            Assert.AreEqual(1, entity.ReferenceProperties.Count);

            CollectionAssert.AreEquivalent(new []{"Id", "Name", "Roles", "CurrentRole"}, entity.Properties.Select(x => x.Name).ToList());
        }

        [Test]
        public void ShouldReturnNullWhenNoEntityFileIsFound()
        {
            Assert.IsNull(entityReader.ReadEntity("Person"));
        }
        
        [Test, ExpectedException(typeof(EntityParsingException))]
        public void ShouldThrowWhenParsingEntityFails()
        {
            const string entitySourceCode = @"namespace Foo.Core.Domain.Model
                                                {
                                                    public class
                                                    {
                                                        public virtual long Id { get; set; }
                                                        public virtual string Name { get; set; }
                                                        public virtual ICollection<Role> Roles { get; set; }
                                                        public virtual Role CurrentRole { get; set; }
                                                    }
                                                }";
            const string path = "\\Foo.Core\\Domain\\Model\\Person.cs";

            Expect.Call(fileSystemMock.FileExists(path)).Return(true);
            Expect.Call(fileSystemMock.FileReadText(path)).Return(entitySourceCode);

            mocks.ReplayAll();

            entityReader.ReadEntity("\\Foo.Core\\Domain\\Model\\Person.cs");

            mocks.VerifyAll();
        }
    }
}