using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Rhino.Mocks;
using Scaffold.Generator.Helpers;
using Scaffold.Generator.Project;
using Scaffold.Io;
using Scaffold.Template;

namespace Scaffold.Test.Generator.Project
{
    [TestFixture]
    public class ProjectGeneratorTests
    {
        private ITemplateEngine templateEngineMock;
        private ProjectGenerator projectGenerator;
        private string scaffoldingHome;
        private IFileSystem fileSystemMock;

        [SetUp]
        public void SetUp()
        {
            templateEngineMock = MockRepository.GenerateMock<ITemplateEngine>();
            fileSystemMock = MockRepository.GenerateMock<IFileSystem>();

            projectGenerator = new ProjectGenerator(templateEngineMock, fileSystemMock);

            scaffoldingHome = Environment.GetEnvironmentVariable("scaffold_home");
            if(scaffoldingHome ==null)
            {
                throw new Exception("scaffold_home is not set");
            }
        }

        [Test]
        public void ShouldGenerateProject()
        {
            var projectName = "FooProject";

            var args = new List<string> { projectName };
            projectGenerator.Execute(args);

            templateEngineMock.AssertWasCalled(x => x.GenerateFromTemplate(Arg<TextReader>.Is.Anything, Arg<string>.Is.Anything, Arg<ProjectTemplateData>.Is.Anything));
            fileSystemMock.AssertWasCalled(x => x.CopyDirectory(Arg<string>.Is.Equal(scaffoldingHome + "\\Templates\\NewProject\\ProjectName\\packages"), Arg<string>.Is.Equal(projectName + "\\packages"), Arg<Regex>.Is.Anything, Arg<Regex>.Is.Anything));
            fileSystemMock.AssertWasCalled(x => x.CopyDirectory(Arg<string>.Is.Equal(scaffoldingHome + "\\Templates\\NewProject\\ProjectName\\ProjectName.Web\\Scripts"), Arg<string>.Is.Equal(projectName + "\\FooProject.Web\\Scripts"), Arg<Regex>.Is.Anything, Arg<Regex>.Is.Anything));
            fileSystemMock.AssertWasCalled(x => x.CopyDirectory(Arg<string>.Is.Equal(scaffoldingHome + "\\Templates\\NewProject\\ProjectName\\ProjectName.Web\\Content"), Arg<string>.Is.Equal(projectName + "\\FooProject.Web\\Content"), Arg<Regex>.Is.Anything, Arg<Regex>.Is.Anything));
            VerifyDirectoriesGenerated(projectName);
        }

        private void VerifyDirectoriesGenerated(string projectName)
        {
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\packages"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Core"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Core/Common"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Core/Common/Logging"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Core/Common/NHibernate"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Core/Common/Persistence"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Core/Common/Persistence/NHibernate"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Core/Common/Transactions"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Core/Common/Windsor"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Core/Domain"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Core/Domain/Model"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Core/Domain/Repositories"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Core/Properties"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Core/Repositories"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Core/Services"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Test"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Test/Common"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Test/Common/AutoMapper"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Test/Common/Persistence"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Test/Common/Persistence/NHibernate"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Test/Common/Transaction"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Test/Common/Windsor"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Test/Controllers"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Test/Properties"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Test/Repositories"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Test/Service References"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Test/Services"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Web"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Web/Common"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Web/Common/AutoMapper"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Web/Common/Logging"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Web/Common/Mvc"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Web/Common/NHibernate"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Web/Common/Windsor"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Web/Content"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Web/Content/images"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Web/Content/themes"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Web/Content/themes/base"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Web/Content/themes/base/images"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Web/Controllers"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Web/Models"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Web/Properties"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Web/Scripts"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Web/Views"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Web/Views/Home"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Web/Views/Shared"));
            fileSystemMock.AssertWasCalled(x => x.CreateDirectory(projectName + "\\" + projectName + ".Web/Views/Error"));
        }
    }
}