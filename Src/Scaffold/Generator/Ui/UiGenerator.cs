using System.Collections.Generic;
using System.IO;
using Scaffold.Configuration;
using Scaffold.Entities;
using Scaffold.Exceptions;
using Scaffold.Generator.Helpers;
using Scaffold.Io;
using Scaffold.Template;
using Scaffold.VisualStudio;

namespace Scaffold.Generator.Ui
{
    public class UiGenerator : BaseGenerator
    {
        private readonly IEntityManager entityManager;
        private readonly ITemplateEngine templateEngine;
        private readonly IConfiguration configuration;
        private readonly IProjectFileManager projectFileManager;
        private readonly IDependencyInjectionManager _dependencyInjectionManager;
        private readonly IAutoMapperHelper autoMapperHelper;
        private readonly IFileSystem fileSystem;

        private const string command = "create-ui-crud";
        private const string description = "\tusage: create-ui-crud [entity]\n\tCreates a CRUD UI for an entity";

        public UiGenerator(IEntityManager entityManager, 
            ITemplateEngine templateEngine, 
            IConfiguration configuration,
            IProjectFileManager projectFileManager, 
            IDependencyInjectionManager _dependencyInjectionManager,
            IAutoMapperHelper autoMapperHelper,
            IFileSystem fileSystem)
            : base(command, description)
        {
            this.entityManager = entityManager;
            this.templateEngine = templateEngine;
            this.configuration = configuration;
            this.projectFileManager = projectFileManager;
            this._dependencyInjectionManager = _dependencyInjectionManager;
            this.autoMapperHelper = autoMapperHelper;
            this.fileSystem = fileSystem;
        }

        public override void Execute(List<string> args)
        {
            ValidateArgs(args);

            Entity entity = ReadEntity(args);

            GenerateController(entity);
            GenerateViewModel(entity);
            CreateViewFolder(entity);
            GenerateIndexView(entity);
            GenerateCreateView(entity);
            GenerateDetailsView(entity);
            GenerateEditView(entity);
            GenerateControllerTest(entity);
            GenerateAutoMapperTest(entity);
            GenerateAutoMapperConfiguration(entity);
        }

        private void GenerateAutoMapperConfiguration(Entity entity)
        {
            autoMapperHelper.AddAutoMapperConfiguration(entity.Name, entity.Name + "ViewModel");
        }

        private static void ValidateArgs(List<string> args)
        {
            if (args.Count != 1)
            {
                throw new IllegalGeneratorArgs(description);
            }
        }

        private void GenerateEditView(Entity entity)
        {
            var generatedFile = "Views\\" + entity.Name + "\\Edit.cshtml";
            const string template = "Scaffold.Generator.Ui.Edit.template";

            var templateData =
                new
                {
                    Entity = entity,
                    WebNamespace = configuration.WebNameSpace,
                    CoreNamespace = configuration.CoreNameSpace
                };
            templateEngine.GenerateFromTemplate(template, configuration.WebNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddContentFileToProject(generatedFile, configuration.WebNameSpace);
        }

        private void GenerateDetailsView(Entity entity)
        {
            var generatedFile = "Views\\" + entity.Name + "\\Details.cshtml";
            const string template = "Scaffold.Generator.Ui.Details.template";

            var templateData =
                new
                {
                    Entity = entity,
                    WebNamespace = configuration.WebNameSpace,
                    CoreNamespace = configuration.CoreNameSpace
                };
            templateEngine.GenerateFromTemplate(template, configuration.WebNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddContentFileToProject(generatedFile, configuration.WebNameSpace);
        }

        private void GenerateCreateView(Entity entity)
        {
            var generatedFile = "Views\\" + entity.Name + "\\Create.cshtml";
            const string template = "Scaffold.Generator.Ui.Create.template";

            var templateData =
                new
                {
                    Entity = entity,
                    WebNamespace = configuration.WebNameSpace,
                    CoreNamespace = configuration.CoreNameSpace
                };
            templateEngine.GenerateFromTemplate(template, configuration.WebNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddContentFileToProject(generatedFile, configuration.WebNameSpace);
        }

        private void GenerateIndexView(Entity entity)
        {
            var generatedFile = "Views\\" + entity.Name + "\\Index.cshtml";
            const string template = "Scaffold.Generator.Ui.Index.template";

            var templateData =
                new
                {
                    Entity = entity,
                    WebNamespace = configuration.WebNameSpace,
                    CoreNamespace = configuration.CoreNameSpace
                };
            templateEngine.GenerateFromTemplate(template, configuration.WebNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddContentFileToProject(generatedFile, configuration.WebNameSpace);
        }

        private void CreateViewFolder(Entity entity)
        {
            fileSystem.CreateDirectory(configuration.WebNameSpace + "\\Views\\" + entity.Name);
        }

        private void GenerateViewModel(Entity entity)
        {
            var generatedFile = "Models\\" + entity.Name + "ViewModel.cs";
            const string template = "Scaffold.Generator.Ui.ViewModel.template";

            var templateData =
                new
                {
                    Entity = entity,
                    WebNamespace = configuration.WebNameSpace,
                    CoreNamespace = configuration.CoreNameSpace
                };
            templateEngine.GenerateFromTemplate(template, configuration.WebNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.WebNameSpace);
        }

        private void GenerateController(Entity entity)
        {
            string controllerName = entity.Name + "Controller";
            var generatedFile = "Controllers\\" + controllerName + ".cs";
            const string template = "Scaffold.Generator.Ui.Controller.template";

            var templateData =
                new
                    {
                        Entity = entity,
                        WebNamespace = configuration.WebNameSpace,
                        CoreNamespace = configuration.CoreNameSpace
                    };
            templateEngine.GenerateFromTemplate(template, configuration.WebNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.WebNameSpace);
            _dependencyInjectionManager.AddToDependencyInjectionTest(controllerName, new[] { configuration.WebNameSpace + ".Controllers" });
        }

        private void GenerateControllerTest(Entity entity)
        {
            var generatedFile = "Controllers\\" + entity.Name + "ControllerTests.cs";
            const string template = "Scaffold.Generator.Ui.ControllerTest.template";

            var templateData =
                new
                {
                    Entity = entity,
                    TestNamespace = configuration.TestNameSpace,
                    WebNamespace = configuration.WebNameSpace,
                    CoreNamespace = configuration.CoreNameSpace
                };
            templateEngine.GenerateFromTemplate(template, configuration.TestNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.TestNameSpace);
        }

        private void GenerateAutoMapperTest(Entity entity)
        {
            var generatedFile = "Common\\AutoMapper\\" + entity.Name + "AutoMapperTests.cs";
            const string template = "Scaffold.Generator.Ui.AutoMapperTest.template";

            var templateData =
                new
                {
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