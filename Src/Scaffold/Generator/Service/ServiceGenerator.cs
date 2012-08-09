using System;
using System.Collections.Generic;
using Scaffold.Configuration;
using Scaffold.Exceptions;
using Scaffold.Generator.Helpers;
using Scaffold.Template;
using Scaffold.VisualStudio;

namespace Scaffold.Generator.Service
{
    public class ServiceGenerator : BaseGenerator
    {
        private readonly ITemplateEngine templateEngine;
        private readonly IConfiguration configuration;
        private readonly IProjectFileManager projectFileManager;
        private readonly IDepencyInjectionManager depencyInjectionManager;

        private const string command = "create-service";
        private const string description = "\tusage: create-service [service name]\n\tCreates a new service";

        public ServiceGenerator(ITemplateEngine templateEngine, 
            IConfiguration configuration,
            IProjectFileManager projectFileManager, IDepencyInjectionManager depencyInjectionManager)
            : base(command, description)
        {
            this.templateEngine = templateEngine;
            this.configuration = configuration;
            this.projectFileManager = projectFileManager;
            this.depencyInjectionManager = depencyInjectionManager;
        }

        public override void Execute(List<string> args)
        {
            ValidateArgs(args);

            string serviceName = args[0];

            GenerateService(serviceName);
            GenerateServiceInterface(serviceName);
            GenerateServiceTest(serviceName);

            GenerateDependencyInjection(serviceName);
        }

        private static void ValidateArgs(List<string> args)
        {
            if (args.Count != 1)
            {
                throw new IllegalGeneratorArgs(description);
            }
        }

        private void GenerateDependencyInjection(string serviceName)
        {
            var serviceInterface = "I" + serviceName;
            var serviceImplementation = serviceName;
            var namespaces = new[] { configuration.CoreNameSpace + ".Services", configuration.CoreNameSpace + ".Services.Impl" };

            depencyInjectionManager.AddToCoreDependencyInjection(serviceInterface, serviceImplementation, namespaces);
            depencyInjectionManager.AddToDependencyInjectionTest(serviceInterface, new[] { configuration.CoreNameSpace + ".Services" });
        }

        private void GenerateServiceInterface(string serviceName)
        {
            var generatedFile = "Services\\I" + serviceName + ".cs";
            const string template = "Scaffold.Generator.Service.ServiceInterface.template";

            var templateData = new { ServiceName = serviceName, CoreNamespace = configuration.CoreNameSpace };
            templateEngine.GenerateFromTemplate(template, configuration.CoreNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.CoreNameSpace);
        }

        private void GenerateService(string serviceName)
        {
            var generatedFile = "Services\\Impl\\" + serviceName + ".cs";
            const string template = "Scaffold.Generator.Service.Service.template";

            var templateData = new { ServiceName = serviceName, CoreNamespace = configuration.CoreNameSpace };
            templateEngine.GenerateFromTemplate(template, configuration.CoreNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.CoreNameSpace);
        }

        private void GenerateServiceTest(string serviceName)
        {
            var generatedFile = "Services\\" + serviceName + "Tests.cs";
            const string template = "Scaffold.Generator.Service.ServiceTest.template";

            var templateData = new { ServiceName = serviceName, TestNamespace = configuration.TestNameSpace, CoreNamespace = configuration.CoreNameSpace };
            templateEngine.GenerateFromTemplate(template, configuration.TestNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.TestNameSpace);
        }
    }
}