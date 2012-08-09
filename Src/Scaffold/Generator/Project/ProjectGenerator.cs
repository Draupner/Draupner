using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Scaffold.Exceptions;
using Scaffold.Io;
using Scaffold.Template;

namespace Scaffold.Generator.Project
{
    public class ProjectGenerator : BaseGenerator
    {
        private readonly ITemplateEngine templateEngine;
        private readonly IFileSystem fileSystem;
        private readonly string scaffoldingHome;

        private const string command = "create-project";
        private const string description = "\tusage: create-project [project name]\n\tCreates a new project";

        public ProjectGenerator(ITemplateEngine templateEngine, IFileSystem fileSystem)
            : base(command, description)
        {
            scaffoldingHome = Environment.GetEnvironmentVariable("scaffold_home");
            this.templateEngine = templateEngine;
            this.fileSystem = fileSystem;
        }

        public override void Execute(List<string> args)
        {
            ValidateArgs(args);
            var projectName = args[0];

            string coreNameSpace = projectName + ".Core";
            string webNameSpace = projectName + ".Web";
            string testNameSpace = projectName + ".Test";
            var templateData = new ProjectTemplateData
                                   {
                                       CoreNamespace = coreNameSpace,
                                       WebNamespace = webNameSpace,
                                       TestNamespace = testNameSpace,
                                       ProjectName = projectName,
                                       DatabaseName = projectName.Replace(".", "")
                                   };

            CreateDirectories(projectName);
            var ignoreTemplate = new Regex("^.*\\.template");
            var ignoreSvn = new Regex("\\.svn");
            fileSystem.CopyDirectory(scaffoldingHome + "\\Templates\\NewProject\\ProjectName\\packages", projectName + "\\packages", ignoreTemplate, ignoreSvn);
            fileSystem.CopyDirectory(scaffoldingHome + "\\Templates\\NewProject\\ProjectName\\Tools", projectName + "\\Tools", ignoreTemplate, ignoreSvn);
            fileSystem.CopyDirectory(scaffoldingHome + "\\Templates\\NewProject\\ProjectName\\ProjectName.Web\\Scripts", projectName + "\\" + webNameSpace + "\\Scripts", ignoreTemplate, ignoreSvn);
            fileSystem.CopyDirectory(scaffoldingHome + "\\Templates\\NewProject\\ProjectName\\ProjectName.Web\\Content", projectName + "\\" + webNameSpace + "\\Content", ignoreTemplate, ignoreSvn);

            GenerateFromTemplates(projectName, templateData);
        }

        private static void ValidateArgs(List<string> args)
        {
            if (args.Count != 1)
            {
                throw new IllegalGeneratorArgs(description);
            }
        }

        private void GenerateFromTemplates(string projectName, ProjectTemplateData templateData)
        {
            var templateBasePath = scaffoldingHome + "\\Templates\\NewProject\\ProjectName";
            var templateFiles = ListTemplates(templateBasePath);
            foreach (var templateFile in templateFiles)
            {
                var outputFile = projectName + templateFile.Replace(templateBasePath, "")
                    .Replace("ProjectName", projectName)
                    .Replace(".template", "")
                    .Replace("CoreNamespace", templateData.CoreNamespace)
                    .Replace("WebNamespace", templateData.WebNamespace)
                    .Replace("TestNamespace", templateData.TestNamespace);
                GenerateFromTemplate(templateData, templateFile, outputFile);                
            }
        }

        private void GenerateFromTemplate(ProjectTemplateData templateData, string template, string outputFile)
        {
            using (var reader = new StreamReader(template))
            {
                templateEngine.GenerateFromTemplate(reader, outputFile, templateData);
            }
        }

        private void CreateDirectories(string projectName)
        {
            fileSystem.CreateDirectory(projectName);
            fileSystem.CreateDirectory(projectName + "\\packages");
            fileSystem.CreateDirectory(projectName + "\\Tools");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Core");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Core/Common");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Core/Common/Logging");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Core/Common/NHibernate");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Core/Common/Persistence");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Core/Common/Persistence/NHibernate");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Core/Common/Transactions");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Core/Common/Windsor");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Core/Domain");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Core/Domain/Model");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Core/Domain/Repositories");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Core/Properties");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Core/Repositories");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Core/Services");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Core/Services/Impl");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Test");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Test/Common");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Test/Common/NHibernate");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Test/Common/AutoMapper");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Test/Common/Persistence");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Test/Common/Persistence/NHibernate");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Test/Common/Transaction");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Test/Common/Windsor");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Test/Controllers");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Test/Properties");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Test/Repositories");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Test/Service References");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Test/Services");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Web");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Web/Common");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Web/Common/AutoMapper");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Web/Common/Logging");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Web/Common/Mvc");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Web/Common/NHibernate");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Web/Common/Transaction");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Web/Common/Windsor");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Web/Content");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Web/Content/images");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Web/Content/themes");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Web/Content/themes/base");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Web/Content/themes/base/images");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Web/Controllers");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Web/Models");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Web/Properties");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Web/Scripts");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Web/Views");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Web/Views/Home");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Web/Views/Shared");
            fileSystem.CreateDirectory(projectName + "\\" + projectName + ".Web/Views/Error");
        }

        private static IList<string> ListTemplates(string path)
        {
            var templates = new List<string>();
            foreach (string file in Directory.GetFiles(path))
            {
                if (file.EndsWith(".template"))
                {
                    templates.Add(file);
                }
            }

            foreach (string folder in Directory.GetDirectories(path))
            {
                var folderPath = folder;
                templates.AddRange(ListTemplates(folderPath));
            }
            return templates;
        }
    }
}
