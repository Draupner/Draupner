using System;
using System.IO;
using NUnit.Framework;
using Rhino.Mocks;
using Scaffold.Configuration;
using Scaffold.Entities;
using Scaffold.Exceptions;
using Scaffold.Io;

namespace Scaffold.Test.Entities
{
    [TestFixture]
    public class EntityManagerTests
    {
        private MockRepository mocks;
        private IEntityReader entityReaderMock;
        private IConfiguration configurationMock;
        private EntityManager entityManager;
        private IFileSystem fileSystemMock;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            entityReaderMock = mocks.DynamicMock<IEntityReader>();
            configurationMock = mocks.DynamicMock<IConfiguration>();
            fileSystemMock = mocks.DynamicMock<IFileSystem>();

            entityManager = new EntityManager(entityReaderMock, configurationMock, fileSystemMock);
        }

        [Test]
        public void ShouldReadEntity()
        {
            const string entityName = "Person";
            const string coreNamespace = "Foo.Core";

            var entity = new Entity();

            Expect.Call(configurationMock.CoreNameSpace).Return(coreNamespace);
            Expect.Call(fileSystemMock.FileExists("Foo.Core\\Domain\\Model\\Person.cs")).Return(true);
            Expect.Call(entityReaderMock.ReadEntity("Foo.Core\\Domain\\Model\\Person.cs")).Return(entity);

            mocks.ReplayAll();

            var result = entityManager.ReadEntity(entityName);

            mocks.VerifyAll();

            Assert.AreEqual(entity, result);
        }

        [Test, ExpectedException(typeof(EntityNotFoundException))]
        public void ShouldThrowExceptionWhenNoEntityIsFound()
        {
            Expect.Call(configurationMock.CoreNameSpace).Return("Foo.Core");
            Expect.Call(fileSystemMock.FileExists("\\Foo.Core\\Domain\\Model\\Person.cs")).Return(false);

            mocks.ReplayAll();

            entityManager.ReadEntity("Person");

            mocks.VerifyAll();
        }
    }
}