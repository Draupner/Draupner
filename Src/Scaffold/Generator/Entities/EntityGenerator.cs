using System;
using System.Collections.Generic;
using Scaffold.Configuration;
using Scaffold.Template;
using Scaffold.VisualStudio;

namespace Scaffold.Generator.Entities
{
    public class EntityGenerator : BaseGenerator
    {
        private readonly ITemplateEngine templateEngine;
        private readonly IConfiguration configuration;
        private readonly IProjectFileManager projectFileManager;
        private const string command = "create-entity";
        private const string description = "\tusage: create-entity [entity name]\n\tCreates a new entity";

        public EntityGenerator(ITemplateEngine templateEngine, IConfiguration configuration, IProjectFileManager projectFileManager)
            : base(command, description)
        {
            this.templateEngine = templateEngine;
            this.configuration = configuration;
            this.projectFileManager = projectFileManager;
        }

        public override void Execute(List<string> args)
        {
            if (args.Count != 1)
            {
                Console.WriteLine("Incorrect number of arguments");
                Console.WriteLine(description);
                return;
            }

            string entityName = args[0];

            GenerateEntity(entityName);
        }

        private void GenerateEntity(string entityName)
        {
            var generatedFile = "Domain\\Model\\" + entityName + ".cs";
            const string template = "Scaffold.Generator.Entities.Entity.template";

            var templateData = new { EntityName = entityName, CoreNamespace = configuration.CoreNameSpace };
            templateEngine.GenerateFromTemplate(template, configuration.CoreNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.CoreNameSpace);
        }
    }
}