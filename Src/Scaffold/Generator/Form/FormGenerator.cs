using System.Collections.Generic;
using Scaffold.Common;
using Scaffold.Configuration;
using Scaffold.Entities;
using Scaffold.Exceptions;
using Scaffold.Generator.Helpers;
using Scaffold.Io;
using Scaffold.Template;
using Scaffold.VisualStudio;

namespace Scaffold.Generator.Form
{
    public class FormGenerator : BaseGenerator
    {
        private readonly IEntityManager entityManager;
        private readonly IConfiguration configuration;
        private readonly ITemplateEngine templateEngine;
        private readonly IProjectFileManager projectFileManager;
        private readonly IFileSystem fileSystem;
        private readonly IAutoMapperHelper autoMapperHelper;

        private const string command = "create-ui-form";
        private const string description = "\tusage: create-ui-form [entity] [form name]\n\tCreates an UI form for an entity";

        public FormGenerator(IEntityManager entityManager, IConfiguration configuration, ITemplateEngine templateEngine, IProjectFileManager projectFileManager, IFileSystem fileSystem, IAutoMapperHelper autoMapperHelper) : base(command, description)
        {
            this.entityManager = entityManager;
            this.configuration = configuration;
            this.templateEngine = templateEngine;
            this.projectFileManager = projectFileManager;
            this.fileSystem = fileSystem;
            this.autoMapperHelper = autoMapperHelper;
        }

        public override void Execute(List<string> args)
        {
            ValidateArgs(args);

            var entity = ReadEntity(args);
            var formName = args[1];

            GenerateViewModel(entity, formName);
            GenerateController(entity, formName);
            GenerateIndexView(entity, formName);
            GenerateAutoMapperConfiguration(entity, formName);
            GenerateControllerTests(entity, formName);
            GenerateAutoMapperTests(entity, formName);
        }

        private void GenerateAutoMapperConfiguration(Entity entity, string formName)
        {
            autoMapperHelper.AddAutoMapperConfiguration(entity.Name, formName + "ViewModel");
        }

        private static void ValidateArgs(List<string> args)
        {
            if (args == null || args.Count != 2)
            {
                throw new IllegalGeneratorArgs(description);
            }
        }

        private void GenerateViewModel(Entity entity, string formName)
        {
            var generatedFile = "Models\\" + formName + "ViewModel.cs";
            const string template = "Scaffold.Generator.Form.ViewModel.template";

            var templateData =
                new
                {
                    FormName = formName,
                    Entity = entity,
                    WebNamespace = configuration.WebNameSpace,
                    CoreNamespace = configuration.CoreNameSpace
                };
            templateEngine.GenerateFromTemplate(template, configuration.WebNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.WebNameSpace);
        }

        private void GenerateController(Entity entity, string formName)
        {
            var generatedFile = "Controllers\\" + formName + "Controller.cs";
            const string template = "Scaffold.Generator.Form.Controller.template";

            var templateData =
                new
                {
                    FormVariableName = StringHelper.LowercaseFirst(formName),
                    FormName = formName,
                    Entity = entity,
                    WebNamespace = configuration.WebNameSpace,
                    CoreNamespace = configuration.CoreNameSpace
                };
            templateEngine.GenerateFromTemplate(template, configuration.WebNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.WebNameSpace);
        }

        private void GenerateIndexView(Entity entity, string formName)
        {
            fileSystem.CreateDirectory(configuration.WebNameSpace + "\\Views\\" + formName);

            var generatedFile = "Views\\" + formName + "\\Index.cshtml";
            const string template = "Scaffold.Generator.Form.Index.template";

            var templateData =
                new
                {
                    FormName = formName,
                    Entity = entity,
                    WebNamespace = configuration.WebNameSpace,
                    CoreNamespace = configuration.CoreNameSpace
                };
            templateEngine.GenerateFromTemplate(template, configuration.WebNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddContentFileToProject(generatedFile, configuration.WebNameSpace);
        }

        private void GenerateControllerTests(Entity entity, string formName)
        {
            var generatedFile = "Controllers\\" + formName + "ControllerTests.cs";
            const string template = "Scaffold.Generator.Form.ControllerTests.template";

            var templateData =
                new
                {
                    Entity = entity,
                    FormName = formName,
                    FormVariableName = StringHelper.LowercaseFirst(formName),
                    CoreNamespace = configuration.CoreNameSpace,
                    WebNamespace = configuration.WebNameSpace,
                    TestNamespace = configuration.TestNameSpace
                };
            templateEngine.GenerateFromTemplate(template, configuration.TestNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.TestNameSpace);
        }

        private void GenerateAutoMapperTests(Entity entity, string formName)
        {
            var generatedFile = "Common\\AutoMapper\\" + formName + "AutoMapperTests.cs";
            const string template = "Scaffold.Generator.Form.AutoMapperTests.template";

            var templateData =
                new
                {
                    Entity = entity,
                    FormName = formName,
                    FormVariableName = StringHelper.LowercaseFirst(formName),
                    CoreNamespace = configuration.CoreNameSpace,
                    WebNamespace = configuration.WebNameSpace,
                    TestNamespace = configuration.TestNameSpace
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