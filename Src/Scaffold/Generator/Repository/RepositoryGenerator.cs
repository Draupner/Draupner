using System.Collections.Generic;
using Scaffold.Configuration;
using Scaffold.Entities;
using Scaffold.Exceptions;
using Scaffold.Generator.Helpers;
using Scaffold.Template;
using Scaffold.VisualStudio;

namespace Scaffold.Generator.Repository
{
    public class RepositoryGenerator : BaseGenerator
    {
        private readonly IEntityManager entityManager;
        private readonly ITemplateEngine templateEngine;
        private readonly IConfiguration configuration;
        private readonly IProjectFileManager projectFileManager;
        private readonly IDependencyInjectionManager _dependencyInjectionManager;

        private const string command = "create-repository";
        private const string description = "\tusage: create-repository [entity]\n\tCreates a repository for an entity";

        public RepositoryGenerator(IEntityManager entityManager, 
            ITemplateEngine templateEngine, 
            IConfiguration configuration,
            IProjectFileManager projectFileManager,
            IDependencyInjectionManager _dependencyInjectionManager)
            : base(command, description)
        {
            this.entityManager = entityManager;
            this.templateEngine = templateEngine;
            this.configuration = configuration;
            this.projectFileManager = projectFileManager;
            this._dependencyInjectionManager = _dependencyInjectionManager;
        }

        public override void Execute(List<string> args)
        {
            ValidateArgs(args);

            Entity entity = ReadEntity(args);

            GenerateRepository(entity);
            GenerateRepositoryInterface(entity);
            GenerateRepositoryTest(entity);

            GenerateDependencyInjection(entity);
        }

        private static void ValidateArgs(List<string> args)
        {
            if (args.Count != 1)
            {
                throw new IllegalGeneratorArgs(description);
            }
        }

        private void GenerateDependencyInjection(Entity entity)
        {
            var repositoryInterface = "I" + entity.Name + "Repository";
            var repositoryImplementation = entity.Name + "Repository";
            var namespaces = new[]{configuration.CoreNameSpace + ".Domain.Repositories", configuration.CoreNameSpace + ".Repositories"};

            _dependencyInjectionManager.AddToCoreDependencyInjection(repositoryInterface, repositoryImplementation, namespaces);
            _dependencyInjectionManager.AddToDependencyInjectionTest(repositoryInterface, new[] { configuration.CoreNameSpace + ".Domain.Repositories" });
        }

        private Entity ReadEntity(List<string> args)
        {
            var entityName = args[0];
            return entityManager.ReadEntity(entityName);
        }

        private void GenerateRepositoryInterface(Entity entity)
        {
            var generatedFile = "Domain\\Repositories\\I" + entity.Name + "Repository.cs";
            const string template = "Scaffold.Generator.Repository.RepositoryInterface.template";

            var templateData = new { Entity = entity, CoreNamespace = configuration.CoreNameSpace };
            templateEngine.GenerateFromTemplate(template, configuration.CoreNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.CoreNameSpace);
        }

        private void GenerateRepository(Entity entity)
        {
            var generatedFile = "Repositories\\" + entity.Name + "Repository.cs";
            const string template = "Scaffold.Generator.Repository.Repository.template";

            var templateData = new { Entity = entity, CoreNamespace = configuration.CoreNameSpace };
            templateEngine.GenerateFromTemplate(template, configuration.CoreNameSpace + "\\" + generatedFile, templateData);
            
            projectFileManager.AddCompileFileToProject(generatedFile, configuration.CoreNameSpace);
        }

        private void GenerateRepositoryTest(Entity entity)
        {
            var generatedFile = "Repositories\\" + entity.Name + "RepositoryTests.cs";
            const string template = "Scaffold.Generator.Repository.RepositoryTest.Template";

            var templateData = new { Entity = entity, TestNamespace = configuration.TestNameSpace, CoreNamespace = configuration.CoreNameSpace };
            templateEngine.GenerateFromTemplate(template, configuration.TestNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.TestNameSpace);
        }
    }
}