using Scaffold.Configuration;

namespace Scaffold.Generator.Helpers
{
    public interface IDependencyInjectionManager
    {
        void AddToCoreDependencyInjection(string interfaceName, string implementationName, string[] namespaces);
        void AddToWebDependencyInjection(string interfaceName, string implementationName, string[] namespaces);
        void AddToDependencyInjectionTest(string interfaceName, string[] namespaces);
    }
}