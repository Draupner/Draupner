using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
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

namespace Scaffold.Common
{
    public class Ioc
    {
        public static IWindsorContainer Container;

        public static void Configure()
        {
            Container = new WindsorContainer()
                .Install(FromAssembly.This());
        }

        public static void Shutdown()
        {
            Container.Dispose();
        }
    }

    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IGeneratorManager>().ImplementedBy<GeneratorManager>());
            container.Register(Component.For<IConfiguration>().ImplementedBy<ConfigurationReader>());
            container.Register(Component.For<IEntityReader>().ImplementedBy<EntityReader>());
            container.Register(Component.For<IEntityManager>().ImplementedBy<EntityManager>());
            container.Register(Component.For<ITemplateEngine>().ImplementedBy<TemplateEngine>());
            container.Register(Component.For<IProjectFileManager>().ImplementedBy<ProjectFileManager>());
            container.Register(Component.For<IFileSystem>().ImplementedBy<FileSystem>());
            container.Register(Component.For<IDepencyInjectionManager>().ImplementedBy<DepencyInjectionManager>());
            container.Register(Component.For<IAutoMapperHelper>().ImplementedBy<AutoMapperHelper>());
            container.Register(Component.For<IWebConfigHelper>().ImplementedBy<WebConfigHelper>());

            container.Register(Component.For<IGenerator>().ImplementedBy<RepositoryGenerator>());
            container.Register(Component.For<IGenerator>().ImplementedBy<NHibernateMapGenerator>());
            container.Register(Component.For<IGenerator>().ImplementedBy<ProjectGenerator>());
            container.Register(Component.For<IGenerator>().ImplementedBy<ServiceGenerator>());
            container.Register(Component.For<IGenerator>().ImplementedBy<UiGenerator>());
            container.Register(Component.For<IGenerator>().ImplementedBy<CrudGenerator>());
            container.Register(Component.For<IGenerator>().ImplementedBy<EntityGenerator>());
            container.Register(Component.For<IGenerator>().ImplementedBy<ListGenerator>());
            container.Register(Component.For<IGenerator>().ImplementedBy<DatabaseGenerator>());
            container.Register(Component.For<IGenerator>().ImplementedBy<SecurityGenerator>());
            container.Register(Component.For<IGenerator>().ImplementedBy<FormGenerator>());
        }
    }
}
