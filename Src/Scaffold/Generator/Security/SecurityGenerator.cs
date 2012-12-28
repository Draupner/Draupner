using System.Collections.Generic;
using Scaffold.Configuration;
using Scaffold.Generator.Helpers;
using Scaffold.Io;
using Scaffold.Template;
using Scaffold.VisualStudio;

namespace Scaffold.Generator.Security
{
    public class SecurityGenerator : BaseGenerator
    {
        private readonly IConfiguration configuration;
        private readonly ITemplateEngine templateEngine;
        private readonly IProjectFileManager projectFileManager;
        private readonly IDependencyInjectionManager _dependencyInjectionManager;
        private readonly IFileSystem fileSystem;
        private readonly IWebConfigHelper webConfigHelper;

        private const string command = "create-authentication";
        private const string description = "\tusage: create-authentication\n\tCreates authentication";

        public SecurityGenerator(IConfiguration configuration, ITemplateEngine templateEngine, IProjectFileManager projectFileManager, IDependencyInjectionManager _dependencyInjectionManager, IFileSystem fileSystem, IWebConfigHelper webConfigHelper) : base(command, description)
        {
            this.configuration = configuration;
            this.templateEngine = templateEngine;
            this.projectFileManager = projectFileManager;
            this._dependencyInjectionManager = _dependencyInjectionManager;
            this.fileSystem = fileSystem;
            this.webConfigHelper = webConfigHelper;
        }

        public override void Execute(List<string> args)
        {
            CreateDirectories();
            
            GenerateUserEntity();
            GenerateRoleEntity();
            GenerateUserRepositoryInterface();
            GenerateUserRepository();
            GenerateRoleRepositoryInterface();
            GenerateRoleRepository();
            GenerateMembershipProvider();
            GenerateRoleProvider();
            GenerateAccountController();
            GenerateLogOnViewModels();
            GenerateChangePasswordViewModels();
            GenerateLogOnView();
            GenerateChangePasswordView();
            GenerateChangePasswordSuccessView();
            AddProvidersToWebConfig();
            GenerateUserMapping();
            GenerateRoleMapping();
            GenerateAuthentication();
            GenerateAuthenticationInterface();

            AddAuthenticationToDependencyInjection();
            AddUserRepositoryToDependencyInjection();
            AddRoleRepositoryToDependencyInjection();

            GenerateMemshipProviderTests();
            GenerateRoleProviderTests();
            GenerateUserRepositoryTests();
            GenerateRoleRepositoryTests();
            GenerateUserPersistenceTests();
            GenerateRolePersistenceTests();
            GenerateAccountControllerTests();
        }

        private void AddProvidersToWebConfig()
        {
            webConfigHelper.AddMembershipProviderToWebConfig();
            webConfigHelper.AddRoleProviderToWebConfig();
        }

        private void CreateDirectories()
        {
            fileSystem.CreateDirectory(configuration.WebNameSpace + "/Common/Security");
            fileSystem.CreateDirectory(configuration.WebNameSpace + "/Views/Account");
            fileSystem.CreateDirectory(configuration.TestNameSpace + "/Common/Security");
        }

        private void GenerateUserEntity()
        {
            const string generatedFile = "Domain\\Model\\User.cs";
            const string template = "Scaffold.Generator.Security.User.template";

            var templateData = new { CoreNamespace = configuration.CoreNameSpace };
            templateEngine.GenerateFromTemplate(template, configuration.CoreNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.CoreNameSpace);
        }

        private void GenerateRoleEntity()
        {
            const string generatedFile = "Domain\\Model\\Role.cs";
            const string template = "Scaffold.Generator.Security.Role.template";

            var templateData = new { CoreNamespace = configuration.CoreNameSpace };
            templateEngine.GenerateFromTemplate(template, configuration.CoreNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.CoreNameSpace);
        }

        private void GenerateUserRepositoryInterface()
        {
            const string generatedFile = "Domain\\Repositories\\IUserRepository.cs";
            const string template = "Scaffold.Generator.Security.IUserRepository.template";

            var templateData = new { CoreNamespace = configuration.CoreNameSpace };
            templateEngine.GenerateFromTemplate(template, configuration.CoreNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.CoreNameSpace);
        }

        private void GenerateUserRepository()
        {
            const string generatedFile = "Repositories\\UserRepository.cs";
            const string template = "Scaffold.Generator.Security.UserRepository.template";

            var templateData = new { CoreNamespace = configuration.CoreNameSpace };
            templateEngine.GenerateFromTemplate(template, configuration.CoreNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.CoreNameSpace);
        }

        private void GenerateRoleRepositoryInterface()
        {
            const string generatedFile = "Domain\\Repositories\\IRoleRepository.cs";
            const string template = "Scaffold.Generator.Security.IRoleRepository.template";

            var templateData = new { CoreNamespace = configuration.CoreNameSpace };
            templateEngine.GenerateFromTemplate(template, configuration.CoreNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.CoreNameSpace);
        }

        private void GenerateRoleRepository()
        {
            const string generatedFile = "Repositories\\RoleRepository.cs";
            const string template = "Scaffold.Generator.Security.RoleRepository.template";

            var templateData = new { CoreNamespace = configuration.CoreNameSpace };
            templateEngine.GenerateFromTemplate(template, configuration.CoreNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.CoreNameSpace);
        }

        private void GenerateMembershipProvider()
        {
            const string generatedFile = "Common\\Security\\NHibernateMembershipProvider.cs";
            const string template = "Scaffold.Generator.Security.NHibernateMembershipProvider.template";

            var templateData = new { CoreNamespace = configuration.CoreNameSpace, WebNamespace = configuration.WebNameSpace };
            templateEngine.GenerateFromTemplate(template, configuration.WebNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.WebNameSpace);
        }

        private void GenerateRoleProvider()
        {
            const string generatedFile = "Common\\Security\\NHibernateRoleProvider.cs";
            const string template = "Scaffold.Generator.Security.NHibernateRoleProvider.template";

            var templateData = new { CoreNamespace = configuration.CoreNameSpace, WebNamespace = configuration.WebNameSpace };
            templateEngine.GenerateFromTemplate(template, configuration.WebNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.WebNameSpace);
        }

        private void GenerateAccountController()
        {
            const string generatedFile = "Controllers\\AccountController.cs";
            const string template = "Scaffold.Generator.Security.AccountController.template";

            var templateData = new { CoreNamespace = configuration.CoreNameSpace, WebNamespace = configuration.WebNameSpace };
            templateEngine.GenerateFromTemplate(template, configuration.WebNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.WebNameSpace);
        }

        private void GenerateLogOnViewModels()
        {
            const string generatedFile = "Models\\LogOnViewModel.cs";
            const string template = "Scaffold.Generator.Security.LogOnViewModel.template";

            var templateData = new { CoreNamespace = configuration.CoreNameSpace, WebNamespace = configuration.WebNameSpace };
            templateEngine.GenerateFromTemplate(template, configuration.WebNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.WebNameSpace);
        }

        private void GenerateChangePasswordViewModels()
        {
            const string generatedFile = "Models\\ChangePasswordViewModel.cs";
            const string template = "Scaffold.Generator.Security.ChangePasswordViewModel.template";

            var templateData = new { CoreNamespace = configuration.CoreNameSpace, WebNamespace = configuration.WebNameSpace };
            templateEngine.GenerateFromTemplate(template, configuration.WebNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.WebNameSpace);
        }

        private void GenerateLogOnView()
        {
            const string generatedFile = "Views\\Account\\LogOn.cshtml";
            const string template = "Scaffold.Generator.Security.LogOnView.template";

            var templateData = new { CoreNamespace = configuration.CoreNameSpace, WebNamespace = configuration.WebNameSpace };
            templateEngine.GenerateFromTemplate(template, configuration.WebNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddContentFileToProject(generatedFile, configuration.WebNameSpace);
        }

        private void GenerateChangePasswordView()
        {
            const string generatedFile = "Views\\Account\\ChangePassword.cshtml";
            const string template = "Scaffold.Generator.Security.ChangePassword.template";

            var templateData = new { CoreNamespace = configuration.CoreNameSpace, WebNamespace = configuration.WebNameSpace };
            templateEngine.GenerateFromTemplate(template, configuration.WebNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddContentFileToProject(generatedFile, configuration.WebNameSpace);
        }

        private void GenerateChangePasswordSuccessView()
        {
            const string generatedFile = "Views\\Account\\ChangePasswordSuccess.cshtml";
            const string template = "Scaffold.Generator.Security.ChangePasswordSuccess.template";

            var templateData = new { CoreNamespace = configuration.CoreNameSpace, WebNamespace = configuration.WebNameSpace };
            templateEngine.GenerateFromTemplate(template, configuration.WebNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddContentFileToProject(generatedFile, configuration.WebNameSpace);
        }

        private void GenerateUserMapping()
        {
            const string generatedFile = "Common\\NHibernate\\UserMap.cs";
            const string template = "Scaffold.Generator.Security.UserMap.template";

            var templateData =
                new
                {
                    CoreNamespace = configuration.CoreNameSpace,
                };
            templateEngine.GenerateFromTemplate(template, configuration.CoreNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.CoreNameSpace);
        }

        private void GenerateRoleMapping()
        {
            const string generatedFile = "Common\\NHibernate\\RoleMap.cs";
            const string template = "Scaffold.Generator.Security.RoleMap.template";

            var templateData =
                new
                {
                    CoreNamespace = configuration.CoreNameSpace,
                };
            templateEngine.GenerateFromTemplate(template, configuration.CoreNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.CoreNameSpace);
        }

        private void GenerateMemshipProviderTests()
        {
            const string generatedFile = "Common\\Security\\MembershipProviderTests.cs";
            const string template = "Scaffold.Generator.Security.MembershipProviderTests.template";

            var templateData =
                new
                {
                    CoreNamespace = configuration.CoreNameSpace,
                    WebNamespace = configuration.WebNameSpace,
                    TestNamespace = configuration.TestNameSpace,
                };
            templateEngine.GenerateFromTemplate(template, configuration.TestNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.TestNameSpace);
        }

        private void GenerateRoleProviderTests()
        {
            const string generatedFile = "Common\\Security\\RoleProviderTests.cs";
            const string template = "Scaffold.Generator.Security.RoleProviderTests.template";

            var templateData =
                new
                {
                    CoreNamespace = configuration.CoreNameSpace,
                    WebNamespace = configuration.WebNameSpace,
                    TestNamespace = configuration.TestNameSpace,
                };
            templateEngine.GenerateFromTemplate(template, configuration.TestNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.TestNameSpace);            
        }

        private void GenerateUserRepositoryTests()
        {
            const string generatedFile = "Repositories\\UserRepositoryTests.cs";
            const string template = "Scaffold.Generator.Security.UserRepositoryTests.template";

            var templateData =
                new
                {
                    CoreNamespace = configuration.CoreNameSpace,
                    WebNamespace = configuration.WebNameSpace,
                    TestNamespace = configuration.TestNameSpace,
                };
            templateEngine.GenerateFromTemplate(template, configuration.TestNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.TestNameSpace);
        }

        private void GenerateRoleRepositoryTests()
        {
            const string generatedFile = "Repositories\\RoleRepositoryTests.cs";
            const string template = "Scaffold.Generator.Security.RoleRepositoryTests.template";

            var templateData =
                new
                {
                    CoreNamespace = configuration.CoreNameSpace,
                    WebNamespace = configuration.WebNameSpace,
                    TestNamespace = configuration.TestNameSpace,
                };
            templateEngine.GenerateFromTemplate(template, configuration.TestNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.TestNameSpace);
        }

        private void GenerateUserPersistenceTests()
        {
            const string generatedFile = "Common\\NHibernate\\UserPersistenceTests.cs";
            const string template = "Scaffold.Generator.Security.UserPersistenceTests.template";

            var templateData =
                new
                {
                    CoreNamespace = configuration.CoreNameSpace,
                    WebNamespace = configuration.WebNameSpace,
                    TestNamespace = configuration.TestNameSpace,
                };
            templateEngine.GenerateFromTemplate(template, configuration.TestNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.TestNameSpace);
        }

        private void GenerateRolePersistenceTests()
        {
            const string generatedFile = "Common\\NHibernate\\RolePersistenceTests.cs";
            const string template = "Scaffold.Generator.Security.RolePersistenceTests.template";

            var templateData =
                new
                {
                    CoreNamespace = configuration.CoreNameSpace,
                    WebNamespace = configuration.WebNameSpace,
                    TestNamespace = configuration.TestNameSpace,
                };
            templateEngine.GenerateFromTemplate(template, configuration.TestNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.TestNameSpace);
        }

        private void GenerateAccountControllerTests()
        {
            const string generatedFile = "Controllers\\AccountControllerTests.cs";
            const string template = "Scaffold.Generator.Security.AccountControllerTests.template";

            var templateData =
                new
                {
                    CoreNamespace = configuration.CoreNameSpace,
                    WebNamespace = configuration.WebNameSpace,
                    TestNamespace = configuration.TestNameSpace,
                };
            templateEngine.GenerateFromTemplate(template, configuration.TestNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.TestNameSpace);
        }

        private void GenerateAuthentication()
        {
            const string generatedFile = "Common\\Security\\Authentication.cs";
            const string template = "Scaffold.Generator.Security.Authentication.template";

            var templateData =
                new
                {
                    CoreNamespace = configuration.CoreNameSpace,
                    WebNamespace = configuration.WebNameSpace,
                    TestNamespace = configuration.TestNameSpace,
                };
            templateEngine.GenerateFromTemplate(template, configuration.WebNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.WebNameSpace);
        }

        private void GenerateAuthenticationInterface()
        {
            const string generatedFile = "Common\\Security\\IAuthentication.cs";
            const string template = "Scaffold.Generator.Security.AuthenticationInterface.template";

            var templateData =
                new
                {
                    CoreNamespace = configuration.CoreNameSpace,
                    WebNamespace = configuration.WebNameSpace,
                    TestNamespace = configuration.TestNameSpace,
                };
            templateEngine.GenerateFromTemplate(template, configuration.WebNameSpace + "\\" + generatedFile, templateData);

            projectFileManager.AddCompileFileToProject(generatedFile, configuration.WebNameSpace);
        }

        private void AddAuthenticationToDependencyInjection()
        {
            const string interfaceName = "IAuthentication";
            const string implementationName = "Authentication";
            var namespaces = new[] { configuration.WebNameSpace + ".Common.Security" };

            _dependencyInjectionManager.AddToWebDependencyInjection(interfaceName, implementationName, namespaces);
            _dependencyInjectionManager.AddToDependencyInjectionTest(interfaceName, namespaces);
        }

        private void AddUserRepositoryToDependencyInjection()
        {
            const string interfaceName = "IUserRepository";
            const string implementationName = "UserRepository";

            _dependencyInjectionManager.AddToCoreDependencyInjection(interfaceName, implementationName, new[] { configuration.CoreNameSpace + ".Repositories", configuration.CoreNameSpace + ".Domain.Repositories" });
            _dependencyInjectionManager.AddToDependencyInjectionTest(interfaceName, new[] { configuration.CoreNameSpace + ".Domain.Repositories" });
        }

        private void AddRoleRepositoryToDependencyInjection()
        {
            const string interfaceName = "IRoleRepository";
            const string implementationName = "RoleRepository";

            _dependencyInjectionManager.AddToCoreDependencyInjection(interfaceName, implementationName, new[] { configuration.CoreNameSpace + ".Repositories", configuration.CoreNameSpace + ".Domain.Repositories" });
            _dependencyInjectionManager.AddToDependencyInjectionTest(interfaceName, new[] { configuration.CoreNameSpace + ".Domain.Repositories" });
        }
    }
}