using System.Collections.Generic;
using Scaffold.Configuration;
using Scaffold.Entities;
using Scaffold.Exceptions;
using Scaffold.Generator.Helpers;
using Scaffold.Io;
using Scaffold.Template;
using Scaffold.VisualStudio;

namespace Scaffold.Generator.NHibernate
{
    public class NHibernateMapGenerator : BaseGenerator
    {
        private readonly IEntityManager entityManager;
        private readonly ITemplateEngine templateEngine;
        private readonly IConfiguration configuration;
        private readonly IProjectFileManager projectFileManager;
        private readonly IFileSystem fileSystem;

        private const string command = "create-nhibernate-mapping";
        private const string description = "\tusage: create-nhibernate-mapping [entity]\n\tCreates NHibernate mappings for an entity";

        public NHibernateMapGenerator(IEntityManager entityManager,
                                      ITemplateEngine templateEngine,
                                      IConfiguration configuration,
                                      IProjectFileManager projectFileManager,
                                      IFileSystem fileSystem)
            : base(command, description)
        {
            this.entityManager = entityManager;
            this.templateEngine = templateEngine;
            this.configuration = configuration;
            this.projectFileManager = projectFileManager;
            this.fileSystem = fileSystem;
        }

        public override void Execute(List<string> args)
        {
            ValidateArgs(args);

            Entity entity = ReadEntity(args);

            GenerateNHibernateMapping(entity);
            GeneratePersistenceTest(entity);
            GenerateAutoFixtureCustomization(entity);
        }

        private void GenerateAutoFixtureCustomization(Entity entity)
        {
            var filePath = configuration.TestNameSpace + "\\AutoFixtureCustomization.cs";
            var fileMerger = new FileMerger(fileSystem);
            
            var codeSnippet = AutoFixtureCustomizationCodeSnippet(entity);

            fileMerger.InsertLineIntoMethod(filePath, codeSnippet, "Customize", "Customize<" + entity.Name + ">");
            
            string @namespace = "using " + configuration.CoreNameSpace + ".Domain.Model;";
            fileMerger.InsertFirstLine(filePath, @namespace, @namespace);
        }

        private static string AutoFixtureCustomizationCodeSnippet(Entity entity)
        {
            var codeSnippet = "            fixture.Customize<" + entity.Name + ">(x => x.Without(y => y.Id)";
            foreach (var property in entity.ReferenceProperties)
            {
                codeSnippet += ".Without(y => y." + property.Name + ")";
            }
            foreach (var property in entity.CollectionProperties)
            {
                codeSnippet += ".Without(y => y." + property.Name + ")";
            }
            codeSnippet += ");";
            return codeSnippet;
        }

        private static void ValidateArgs(List<string> args)
        {
            if (args.Count != 1)
            {
                throw new IllegalGeneratorArgs(description);
            }
        }

        private void GeneratePersistenceTest(Entity entity)
        {
            var generatedFile = "Common\\NHibernate\\" + entity.Name + "PersistenceTests.cs";
            const string template = "Scaffold.Generator.NHibernate.PersistenceTest.template";

            var templateData =
                new
                {
                    Entity = entity,
                    CoreNamespace = configuration.CoreNameSpace,
                    TestNamespace = configuration.TestNameSpace,
                };
            templateEngine.GenerateFromTemplate(template, configuration.TestNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.TestNameSpace);
        }

        private void GenerateNHibernateMapping(Entity entity)
        {
            var generatedFile = "Common\\NHibernate\\" + entity.Name + "Map.cs";
            const string template = "Scaffold.Generator.NHibernate.NHibernateMap.template";

            var templateData =
                new
                    {
                        Entity = entity,
                        CoreNamespace = configuration.CoreNameSpace,
                    };
            templateEngine.GenerateFromTemplate(template, configuration.CoreNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.CoreNameSpace);
        }

        private Entity ReadEntity(List<string> args)
        {
            var entityName = args[0];
            return entityManager.ReadEntity(entityName);
        }
    }
}