using System;
using System.IO;
using NUnit.Framework;
using Rhino.Mocks;
using Scaffold.Entities;
using Scaffold.Io;
using Scaffold.Template;

namespace Scaffold.Test.Template
{
    [TestFixture]
    public class TemplateEngineTests
    {
        private ITemplateEngine templateEngine;
        private MockRepository mocks;
        private IFileSystem fileSystemMock;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            fileSystemMock = mocks.DynamicMock<IFileSystem>();

            templateEngine = new TemplateEngine(fileSystemMock);
        }

        [Test]
        public void ShouldGenerateFileFromTemplate()
        {
            var templateStream = new StringReader("Hello @Model.Name.");
            var outputFile = "Result" + Guid.NewGuid() + ".cs";
            var templateModel = new {Name = "Foo"};

            Expect.Call(() => fileSystemMock.FileWriteText(outputFile, "Hello Foo."));

            mocks.ReplayAll();

            templateEngine.GenerateFromTemplate(templateStream, outputFile, templateModel);

            mocks.VerifyAll();
        }

        [Test]
        public void ShouldGenerateFileFromTemplateWithoutModel()
        {
            var templateStream = new StringReader("Hello Foo.");
            var outputFile = "Result" + Guid.NewGuid() + ".cs";
            var templateModel = new {};

            Expect.Call(() => fileSystemMock.FileWriteText(outputFile, "Hello Foo."));

            mocks.ReplayAll();

            templateEngine.GenerateFromTemplate(templateStream, outputFile, templateModel);

            mocks.VerifyAll();
        }

        [Test]
        public void ShouldGenerateFileFromTemplateContainedInEmbeddedResource()
        {
            var templateName = "Scaffold.Generator.Repository.RepositoryInterface.template";
            var outputFile = "Result" + Guid.NewGuid() + ".cs";
            var templateModel = new { CoreNamespace = "Foo.Core", Entity = new Entity { Name = "Person" } };

            Expect.Call(() => fileSystemMock.FileWriteText(Arg<string>.Is.Equal(outputFile), Arg<string>.Matches(x => x.StartsWith("using System.Collections.Generic;"))));

            mocks.ReplayAll();

            templateEngine.GenerateFromTemplate(templateName, outputFile, templateModel);

            mocks.VerifyAll();
        }
    }
}