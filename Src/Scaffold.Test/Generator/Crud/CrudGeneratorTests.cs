using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using Scaffold.Generator;
using Scaffold.Generator.Crud;

namespace Scaffold.Test.Generator.Crud
{
    [TestFixture]
    public class CrudGeneratorTests
    {
        private CrudGenerator crudGenerator;
        private IGeneratorManager generatorManagerMock;

        [SetUp]
        public void SetUp()
        {
            generatorManagerMock = MockRepository.GenerateMock<IGeneratorManager>();

            crudGenerator = new CrudGenerator(generatorManagerMock);
        }

        [Test]
        public void ShouldGenerateCrud()
        {
            var args = new List<string>{"Book"};

            crudGenerator.Execute(args);

            generatorManagerMock.AssertWasCalled(x => x.ExecuteGenerator("create-nhibernate-mapping", args));
            generatorManagerMock.AssertWasCalled(x => x.ExecuteGenerator("create-repository", args));
            generatorManagerMock.AssertWasCalled(x => x.ExecuteGenerator("create-ui-crud", args));
        }
    }
}