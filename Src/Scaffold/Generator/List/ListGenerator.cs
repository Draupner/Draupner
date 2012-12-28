using System;
using System.Collections.Generic;
using System.IO;
using Scaffold.Configuration;
using Scaffold.Entities;
using Scaffold.Exceptions;
using Scaffold.Io;
using Scaffold.Template;
using Scaffold.VisualStudio;

namespace Scaffold.Generator.List
{
    public class ListGenerator : BaseGenerator
    {
        private readonly IEntityManager entityManager;
        private readonly IConfiguration configuration;
        private readonly ITemplateEngine templateEngine;
        private readonly IProjectFileManager projectFileManager;
        private readonly IFileSystem fileSystem;
        private const string command = "create-ui-list";
        private const string description = "\tusage: create-ui-list [entity] [list name]\n\tCreates a UI list for an entity";

        public ListGenerator(IEntityManager entityManager, IConfiguration configuration, ITemplateEngine templateEngine, IProjectFileManager projectFileManager, IFileSystem fileSystem)
            : base(command, description)
        {
            this.entityManager = entityManager;
            this.configuration = configuration;
            this.templateEngine = templateEngine;
            this.projectFileManager = projectFileManager;
            this.fileSystem = fileSystem;
        }

        public override void Execute(List<string> args)
        {
            ValidateArgs(args);

            Entity entity = ReadEntity(args);
            string listName = args[1];

            CreateViewFolder(listName);
            GenerateController(entity, listName);
            GenerateViewModel(entity, listName);
            GenerateIndexView(entity, listName);
            GenerateControllerTest(entity, listName);
        }

        private static void ValidateArgs(List<string> args)
        {
            if (args.Count != 2)
            {
                throw new IllegalGeneratorArgs(description);
            }
        }

        private void CreateViewFolder(string listName)
        {
            fileSystem.CreateDirectory(configuration.WebNameSpace + "\\Views\\" + listName);
        }

        private void GenerateIndexView(Entity entity, string listName)
        {
            var generatedFile = "Views\\" + listName + "\\Index.cshtml";
            const string template = "Scaffold.Generator.List.Index.template";

            var templateData =
                new
                {
                    ListName = listName,
                    Entity = entity,
                    WebNamespace = configuration.WebNameSpace,
                    CoreNamespace = configuration.CoreNameSpace
                };
            templateEngine.GenerateFromTemplate(template, configuration.WebNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddContentFileToProject(generatedFile, configuration.WebNameSpace);
        }

        private void GenerateViewModel(Entity entity, string listName)
        {
            var generatedFile = "Models\\" + listName + "ViewModel.cs";
            const string template = "Scaffold.Generator.List.ViewModel.template";

            var templateData =
                new
                {
                    ListName = listName,
                    Entity = entity,
                    WebNamespace = configuration.WebNameSpace,
                    CoreNamespace = configuration.CoreNameSpace
                };
            templateEngine.GenerateFromTemplate(template, configuration.WebNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.WebNameSpace);
        }

        private void GenerateController(Entity entity, string listName)
        {
            var generatedFile = "Controllers\\" + listName + "Controller.cs";
            const string template = "Scaffold.Generator.List.Controller.template";

            var templateData =
                new
                {
                    ListName = listName,
                    Entity = entity,
                    WebNamespace = configuration.WebNameSpace,
                    CoreNamespace = configuration.CoreNameSpace
                };
            templateEngine.GenerateFromTemplate(template, configuration.WebNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.WebNameSpace);
        }

        private void GenerateControllerTest(Entity entity, string listName)
        {
            var generatedFile = "Controllers\\" + listName + "ControllerTests.cs";
            const string template = "Scaffold.Generator.List.ControllerTest.template";

            var templateData =
                new
                {
                    ListName = listName,
                    Entity = entity,
                    TestNamespace = configuration.TestNameSpace,
                    WebNamespace = configuration.WebNameSpace,
                    CoreNamespace = configuration.CoreNameSpace
                };
            templateEngine.GenerateFromTemplate(template, configuration.TestNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.TestNameSpace);
        }

        private Entity ReadEntity(List<string> args)
        {
            var entityName = args[0];
            return entityManager.ReadEntity(entityName);
        }
    }
}