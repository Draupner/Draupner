using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using Rhino.Mocks;
using Scaffold.Common;
using Scaffold.Exceptions;
using Scaffold.Generator;

namespace Scaffold.Test.Generator
{
    [TestFixture]
    public class GeneratorManagerTests
    {
        private GeneratorManager generatorManager;
        private MockRepository mocks;
        private IGenerator fooGenerator;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            fooGenerator = mocks.StrictMock<IGenerator>();

            Ioc.Container = new WindsorContainer();
            Ioc.Container.Register(Component.For<IGenerator>().Instance(fooGenerator));
            Ioc.Container.Register(Component.For<IGenerator>().ImplementedBy<BasGenerator>());

            generatorManager = new GeneratorManager();
        }

        [Test]
        public void ShouldExecuteCommand()
        {
            var args = new List<string>();
            const string command = "create-foo";

            Expect.Call(fooGenerator.CanHandle(command)).Return(true);
            Expect.Call(() => fooGenerator.Execute(args));

            mocks.ReplayAll();

            generatorManager.ExecuteGenerator(command, args);

            mocks.VerifyAll();
        }

        [Test, ExpectedException(typeof(GeneratorNotFoundException))]
        public void ShouldThrowExceptionWhenExecutingCommandWhichDoesNotExists()
        {
            var args = new List<string>();
            const string command = "create-blah";

            Expect.Call(fooGenerator.CanHandle(command)).Return(false);

            mocks.ReplayAll();

            generatorManager.ExecuteGenerator(command, args);

            mocks.VerifyAll();
        }
    }

    public class BasGenerator : BaseGenerator
    {
        public BasGenerator() : base("create-bar", "usage: create-bar")
        {
        }

        public override void Execute(List<string> args)
        {
        }
    }
}