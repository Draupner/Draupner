using System.Linq;
using NUnit.Framework;
using Scaffold.Common;
using Scaffold.Configuration;
using Scaffold.Entities;
using Scaffold.Generator;
using Scaffold.Generator.Crud;
using Scaffold.Generator.Database;
using Scaffold.Generator.Entities;
using Scaffold.Generator.Form;
using Scaffold.Generator.Helpers;
using Scaffold.Generator.List;
using Scaffold.Generator.NHibernate;
using Scaffold.Generator.Project;
using Scaffold.Generator.Repository;
using Scaffold.Generator.Security;
using Scaffold.Generator.Service;
using Scaffold.Generator.Ui;
using Scaffold.Io;
using Scaffold.Template;
using Scaffold.VisualStudio;

namespace Scaffold.Test.Common
{
    [TestFixture]
    public class IocTests
    {
        [Test]
        public void ShouldResolveDependencies()
        {
            Ioc.Configure();

            Assert.NotNull(Ioc.Container.Resolve<IGeneratorManager>());
            Assert.NotNull(Ioc.Container.Resolve<IConfiguration>());
            Assert.NotNull(Ioc.Container.Resolve<IEntityReader>());
            Assert.NotNull(Ioc.Container.Resolve<IEntityManager>());
            Assert.NotNull(Ioc.Container.Resolve<ITemplateEngine>());
            Assert.NotNull(Ioc.Container.Resolve<IProjectFileManager>());
            Assert.NotNull(Ioc.Container.Resolve<IFileSystem>());
            CollectionAssert.AreEquivalent(new[]
                                               {
                                                   typeof (UiGenerator), typeof (ProjectGenerator),
                                                   typeof (NHibernateMapGenerator), typeof (RepositoryGenerator),
                                                   typeof (ServiceGenerator), typeof (CrudGenerator), typeof(EntityGenerator), 
                                                   typeof(ListGenerator), typeof(DatabaseGenerator), typeof(SecurityGenerator), typeof(FormGenerator)
                                               },
                                           Ioc.Container.ResolveAll<IGenerator>().Select(x => x.GetType()).ToList());

            Ioc.Shutdown();
        }
    }
}