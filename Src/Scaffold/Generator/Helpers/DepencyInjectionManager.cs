using System;
using Scaffold.Configuration;
using Scaffold.Io;

namespace Scaffold.Generator.Helpers
{
    public class DepencyInjectionManager : IDepencyInjectionManager
    {
        private readonly IConfiguration configuration;
        private readonly FileMerger fileMerger;

        public DepencyInjectionManager(IFileSystem fileSystem, IConfiguration configuration)
        {
            this.configuration = configuration;
            fileMerger = new FileMerger(fileSystem);
        }

        public void AddToCoreDependencyInjection(string interfaceName, string implementationName, string[] namespaces)
        {
            var filePath = CoreDependencyInjectionPath(configuration);
            AddNamespace(filePath, namespaces);
            AddDependencyInjection(filePath, interfaceName, implementationName);
        }

        public void AddToWebDependencyInjection(string interfaceName, string implementationName, string[] namespaces)
        {
            var filePath = WebDependencyInjectionPath(configuration);
            AddNamespace(filePath, namespaces);
            AddDependencyInjection(filePath, interfaceName, implementationName);
        }

        public void AddToDependencyInjectionTest(string interfaceName, string[] namespaces)
        {
            string filePath = DependencyInjectionTests(configuration);
            AddNamespace(filePath, namespaces);
            AddToDependencyInjectionTests(filePath, interfaceName);
        }

        private void AddDependencyInjection(string filePath, string interfaceName, string implementationName)
        {
            Console.WriteLine("Adding to dependency injection: " + interfaceName + " , " + implementationName);

            var registerSourceCode = "            container.Register(Component.For<" + interfaceName + ">().ImplementedBy<" + implementationName + ">());";
            fileMerger.InsertLineAfterLast(filePath, registerSourceCode, "container.Register", implementationName);
        }

        private void AddToDependencyInjectionTests(string filePath, string interfaceName)
        {
            var resolveSourceCode = "            Assert.NotNull(container.Resolve<" + interfaceName + ">());";
            fileMerger.InsertLineAfterLast(filePath, resolveSourceCode, ".Resolve<", interfaceName);
        }

        private void AddNamespace(string filePath, string[] namespaces)
        {
            foreach (var ns in namespaces)
            {
                fileMerger.InsertFirstLine(filePath, "using " + ns + ";", "using " + ns + ";");
            }
        }

        private static string CoreDependencyInjectionPath(IConfiguration configuration)
        {
            return configuration.CoreNameSpace + "\\Common\\Windsor\\CoreWindsorInstaller.cs";
        }

        private static string WebDependencyInjectionPath(IConfiguration configuration)
        {
            return configuration.WebNameSpace + "\\Common\\Windsor\\WebWindsorInstaller.cs";
        }

        private static string DependencyInjectionTests(IConfiguration configuration)
        {
            return configuration.TestNameSpace + "\\Common\\Windsor\\WindsorConfigurationTests.cs";
        }
    }
}