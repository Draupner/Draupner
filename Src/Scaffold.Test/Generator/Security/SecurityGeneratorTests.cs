using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Scaffold.Configuration;
using Scaffold.Generator.Helpers;
using Scaffold.Generator.Security;
using Scaffold.Io;
using Scaffold.Template;
using Scaffold.VisualStudio;

namespace Scaffold.Test.Generator.Security
{
    [TestFixture]
    public class SecurityGeneratorTests
    {
        private SecurityGenerator securityGenerator;
        private IConfiguration configurationMock;
        private IProjectFileManager projectFileManagerMock;
        private IDepencyInjectionManager depencyInjectionManagerMock;
        private IFileSystem fileSystemMock;
        private IWebConfigHelper webConfigHelperMock;

        [SetUp]
        public void SetUp()
        {
            configurationMock = MockRepository.GenerateMock<IConfiguration>();
            projectFileManagerMock = MockRepository.GenerateMock<IProjectFileManager>();
            depencyInjectionManagerMock = MockRepository.GenerateMock<IDepencyInjectionManager>();
            fileSystemMock = MockRepository.GenerateMock<IFileSystem>();
            webConfigHelperMock = MockRepository.GenerateMock<IWebConfigHelper>();

            var templateEngine = new TemplateEngine(fileSystemMock);
            securityGenerator = new SecurityGenerator(configurationMock, templateEngine, projectFileManagerMock,
                                                      depencyInjectionManagerMock, fileSystemMock, webConfigHelperMock);
        }

        [Test]
        public void ShouldGenerateSecurity()
        {
            configurationMock.Stub(x => x.TestNameSpace).Return("Blah.Test");
            configurationMock.Stub(x => x.CoreNameSpace).Return("Blah.Core");
            configurationMock.Stub(x => x.WebNameSpace).Return("Blah.Web");

            securityGenerator.Execute(new List<string>());

            VerifyGenerateUserEntity();
            VerifyGenerateRoleEntity();
            VerifyGenerateUserRepositoryInterface();
            VerifyGenerateRoleRepositoryInterface();
            VerifyGenerateUserRepository();
            VerifyGenerateRoleRepository();
            VerifyGenerateMemshipProvider();
            VerifyGenerateRoleProvider();
            VerifyGenerateAccountController();
            VerifyGenerateLogOnViewModel();
            VerifyGenerateChangePasswordViewModel();
            VerifyGenerateLogOnView();
            VerifyGenerateChangePasswordView();
            VerifyGenerateChangePasswordSuccessView();
            VerifyGenerateUserMap();
            VerifyGenerateRoleMap();
            VerifyGenerateAuthentication();
            VerifyGenerateIAuthentication();
            VerifyGenerateMembershipProviderTests();
            VerifyGenerateRoleProviderTests();
            VerifyGenerateUserRepositoryTests();
            VerifyGenerateRoleRepositoryTests();
            VerifyGenerateUserPersistenceTests();
            VerifyGenerateRolePersistenceTests();
            VerifyGenerateAccountControllerTests();

            VerifyProvidersAddedToWebConfig();
        }

        private void VerifyGenerateAccountControllerTests()
        {
            const string genereatedFile = @"Controllers\AccountControllerTests.cs";
            const string @namespace = "Blah.Test";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.AccountControllerTests.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateRolePersistenceTests()
        {
            const string genereatedFile = @"Common\NHibernate\RolePersistenceTests.cs";
            const string @namespace = "Blah.Test";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.RolePersistenceTests.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateUserPersistenceTests()
        {
            const string genereatedFile = @"Common\NHibernate\UserPersistenceTests.cs";
            const string @namespace = "Blah.Test";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.UserPersistenceTests.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateRoleRepositoryTests()
        {
            const string genereatedFile = @"Repositories\RoleRepositoryTests.cs";
            const string @namespace = "Blah.Test";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.RoleRepositoryTests.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateUserRepositoryTests()
        {
            const string genereatedFile = @"Repositories\UserRepositoryTests.cs";
            const string @namespace = "Blah.Test";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.UserRepositoryTests.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateRoleProviderTests()
        {
            const string genereatedFile = @"Common\Security\RoleProviderTests.cs";
            const string @namespace = "Blah.Test";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.RoleProviderTests.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateMembershipProviderTests()
        {
            const string genereatedFile = @"Common\Security\MembershipProviderTests.cs";
            const string @namespace = "Blah.Test";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.MembershipProviderTests.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateIAuthentication()
        {
            const string genereatedFile = @"Common\Security\IAuthentication.cs";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.IAuthentication.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateAuthentication()
        {
            const string genereatedFile = @"Common\Security\Authentication.cs";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.Authentication.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateRoleMap()
        {
            const string genereatedFile = @"Common\NHibernate\RoleMap.cs";
            const string @namespace = "Blah.Core";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.RoleMap.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateUserMap()
        {
            const string genereatedFile = @"Common\NHibernate\UserMap.cs";
            const string @namespace = "Blah.Core";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.UserMap.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateChangePasswordSuccessView()
        {
            const string genereatedFile = @"Views\Account\ChangePasswordSuccess.cshtml";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddContentFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.ChangePasswordSuccessView.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateChangePasswordView()
        {
            const string genereatedFile = @"Views\Account\ChangePassword.cshtml";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddContentFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.ChangePasswordView.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateLogOnView()
        {
            const string genereatedFile = @"Views\Account\LogOn.cshtml";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddContentFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.LogOnView.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateChangePasswordViewModel()
        {
            const string genereatedFile = @"Models\ChangePasswordViewModel.cs";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.ChangePasswordViewModel.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateLogOnViewModel()
        {
            const string genereatedFile = @"Models\LogOnViewModel.cs";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.LogOnViewModel.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateAccountController()
        {
            const string genereatedFile = @"Controllers\AccountController.cs";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.AccountController.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateRoleProvider()
        {
            const string genereatedFile = @"Common\Security\NHibernateRoleProvider.cs";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.RoleProvider.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateMemshipProvider()
        {
            const string genereatedFile = @"Common\Security\NHibernateMembershipProvider.cs";
            const string @namespace = "Blah.Web";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.MemberShipProvider.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateRoleRepository()
        {
            const string genereatedFile = @"Repositories\RoleRepository.cs";
            const string @namespace = "Blah.Core";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.RoleRepository.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateUserRepository()
        {
            const string genereatedFile = @"Repositories\UserRepository.cs";
            const string @namespace = "Blah.Core";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.UserRepository.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateRoleRepositoryInterface()
        {
            const string genereatedFile = @"Domain\Repositories\IRoleRepository.cs";
            const string @namespace = "Blah.Core";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.IRoleRepository.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateUserRepositoryInterface()
        {
            const string genereatedFile = @"Domain\Repositories\IUserRepository.cs";
            const string @namespace = "Blah.Core";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.IUserRepository.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateRoleEntity()
        {
            const string genereatedFile = @"Domain\Model\Role.cs";
            const string @namespace = "Blah.Core";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.Role.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyGenerateUserEntity()
        {
            const string genereatedFile = @"Domain\Model\User.cs";
            const string @namespace = "Blah.Core";

            projectFileManagerMock.AssertWasCalled(x => x.AddCompileFileToProject(genereatedFile, @namespace));
            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal(@namespace + @"\" + genereatedFile), Arg<string>.Is.Anything));

            string expected = EmbeddedResourceReader.ReadEmbeddedResource("Scaffold.Test.Generator.Security.User.expected");
            Assert.AreEqual(expected, GetGeneratedFile(@namespace + @"\" + genereatedFile));
        }

        private void VerifyProvidersAddedToWebConfig()
        {
            webConfigHelperMock.AssertWasCalled(x => x.AddMembershipProviderToWebConfig());
            webConfigHelperMock.AssertWasCalled(x => x.AddRoleProviderToWebConfig());
        }

        private string GetGeneratedFile(string fileName)
        {
            IList<object[]> arguments = fileSystemMock.GetArgumentsForCallsMadeOn(x => x.FileWriteText(null, null));
            return (from argument in arguments where ((string)argument[0]) == fileName select (string)argument[1]).FirstOrDefault();
        }
    }
}