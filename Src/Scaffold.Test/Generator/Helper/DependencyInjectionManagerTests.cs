using System;
using NUnit.Framework;
using Rhino.Mocks;
using Scaffold.Configuration;
using Scaffold.Generator.Helpers;
using Scaffold.Io;

namespace Scaffold.Test.Generator.Helper
{
    [TestFixture]
    public class DependencyInjectionManagerTests
    {
        private DependencyInjectionManager dependencyInjectionManager;
        private IFileSystem fileSystemMock;
        private IConfiguration configurationMock;

        [SetUp]
        public void SetUp()
        {
            fileSystemMock = MockRepository.GenerateMock<IFileSystem>();
            configurationMock = MockRepository.GenerateMock<IConfiguration>();

            dependencyInjectionManager = new DependencyInjectionManager(fileSystemMock, configurationMock);
        }

        [Test]
        public void ShouldAddCoreDependencies()
        {
            string dependencyInjectionFileContent = @"using Blah.Core.Services;
using Blah.Core.Services.Impl;

namespace Blah.Core.Common.Windsor
{
    public class CoreWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IUnitOfWork>().ImplementedBy<SharedNHibernateUnitOfWork>());
            container.Register(Component.For<IUnitOfWorkFactory>().ImplementedBy<NHibernateUnitOfWorkFactory>());
        }
    }
}";

            const string expectedDependencyInjectionFileContent = @"using Blah.Core.Repositories;
using Blah.Core.Domain.Repositories;
using Blah.Core.Services;
using Blah.Core.Services.Impl;

namespace Blah.Core.Common.Windsor
{
    public class CoreWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IUnitOfWork>().ImplementedBy<SharedNHibernateUnitOfWork>());
            container.Register(Component.For<IUnitOfWorkFactory>().ImplementedBy<NHibernateUnitOfWorkFactory>());
            container.Register(Component.For<IBookRepository>().ImplementedBy<BookRepository>());
        }
    }
}
";

            configurationMock.Stub(x => x.WebNameSpace).Return("Blah.Web");
            configurationMock.Stub(x => x.CoreNameSpace).Return("Blah.Core");
            configurationMock.Stub(x => x.TestNameSpace).Return("Blah.Test");
 
            fileSystemMock.Stub(x => x.FileReadText("Blah.Core\\Common\\Windsor\\CoreWindsorInstaller.cs")).Do(new Func<string, string>(filePath => dependencyInjectionFileContent));
            fileSystemMock.Stub(x => x.FileWriteText(Arg<string>.Is.Equal("Blah.Core\\Common\\Windsor\\CoreWindsorInstaller.cs"), Arg<string>.Is.Anything)).Do(new Action<string, string>((filePath, fileContent) => dependencyInjectionFileContent = fileContent));

            dependencyInjectionManager.AddToCoreDependencyInjection("IBookRepository", "BookRepository", new[] { "Blah.Core.Domain.Repositories", "Blah.Core.Repositories" });

            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal("Blah.Core\\Common\\Windsor\\CoreWindsorInstaller.cs"), Arg<string>.Is.Anything));

            Assert.AreEqual(expectedDependencyInjectionFileContent, dependencyInjectionFileContent);
        }

        [Test]
        public void ShouldAddWebDependencies()
        {
            string dependencyInjectionFileContent = @"using Blah.Core.Services;
using Blah.Core.Services.Impl;

namespace Blah.Core.Common.Windsor
{
    public class CoreWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IUnitOfWork>().ImplementedBy<SharedNHibernateUnitOfWork>());
            container.Register(Component.For<IUnitOfWorkFactory>().ImplementedBy<NHibernateUnitOfWorkFactory>());
        }
    }
}";

            const string expectedDependencyInjectionFileContent = @"using Blah.Core.Repositories;
using Blah.Core.Domain.Repositories;
using Blah.Core.Services;
using Blah.Core.Services.Impl;

namespace Blah.Core.Common.Windsor
{
    public class CoreWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IUnitOfWork>().ImplementedBy<SharedNHibernateUnitOfWork>());
            container.Register(Component.For<IUnitOfWorkFactory>().ImplementedBy<NHibernateUnitOfWorkFactory>());
            container.Register(Component.For<IBookRepository>().ImplementedBy<BookRepository>());
        }
    }
}
";

            configurationMock.Stub(x => x.WebNameSpace).Return("Blah.Web");
            configurationMock.Stub(x => x.CoreNameSpace).Return("Blah.Core");
            configurationMock.Stub(x => x.TestNameSpace).Return("Blah.Test");
            fileSystemMock.Stub(x => x.FileReadText("Blah.Web\\Common\\Windsor\\WebWindsorInstaller.cs")).Do(new Func<string, string>(filePath => dependencyInjectionFileContent));
            fileSystemMock.Stub(x => x.FileWriteText(Arg<string>.Is.Equal("Blah.Web\\Common\\Windsor\\WebWindsorInstaller.cs"), Arg<string>.Is.Anything)).Do(new Action<string, string>((filePath, fileContent) => dependencyInjectionFileContent = fileContent));

            dependencyInjectionManager.AddToWebDependencyInjection("IBookRepository", "BookRepository", new[] { "Blah.Core.Domain.Repositories", "Blah.Core.Repositories" });

            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal("Blah.Web\\Common\\Windsor\\WebWindsorInstaller.cs"), Arg<string>.Is.Anything));

            Assert.AreEqual(expectedDependencyInjectionFileContent, dependencyInjectionFileContent);
        }

        [Test]
        public void ShouldAddDependenciesTest()
        {
            string dependencyInjectionFileContent = @"using Blah.Web.Common.Security;
using Blah.Core.Services;
using Blah.Web.Controllers;

namespace Blah.Test.Common.Windsor
{
    public class WindsorConfigurationTests : IDisposable 
    {
        [Fact]
        public void ShouldRegisterDependencies()
        {
            Assert.NotNull(container.Resolve<IUnitOfWork>());
            Assert.NotNull(container.Resolve<IUnitOfWorkFactory>());
        }

    }
}
";

            const string expectedDependencyInjectionFileContent = @"using Blah.Core.Domain.Repositories;
using Blah.Web.Common.Security;
using Blah.Core.Services;
using Blah.Web.Controllers;

namespace Blah.Test.Common.Windsor
{
    public class WindsorConfigurationTests : IDisposable 
    {
        [Fact]
        public void ShouldRegisterDependencies()
        {
            Assert.NotNull(container.Resolve<IUnitOfWork>());
            Assert.NotNull(container.Resolve<IUnitOfWorkFactory>());
            Assert.NotNull(container.Resolve<IBookRepository>());
        }

    }
}
";

            configurationMock.Stub(x => x.WebNameSpace).Return("Blah.Web");
            configurationMock.Stub(x => x.CoreNameSpace).Return("Blah.Core");
            configurationMock.Stub(x => x.TestNameSpace).Return("Blah.Test");
            fileSystemMock.Stub(x => x.FileReadText("Blah.Test\\Common\\Windsor\\WindsorConfigurationTests.cs")).Do(new Func<string, string>(filePath => dependencyInjectionFileContent));
            fileSystemMock.Stub(x => x.FileWriteText(Arg<string>.Is.Equal("Blah.Test\\Common\\Windsor\\WindsorConfigurationTests.cs"), Arg<string>.Is.Anything)).Do(new Action<string, string>((filePath, fileContent) => dependencyInjectionFileContent = fileContent));

            dependencyInjectionManager.AddToDependencyInjectionTest("IBookRepository", new[] { "Blah.Core.Domain.Repositories" });

            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal("Blah.Test\\Common\\Windsor\\WindsorConfigurationTests.cs"), Arg<string>.Is.Anything));

            Assert.AreEqual(expectedDependencyInjectionFileContent, dependencyInjectionFileContent);
        }
    }
}