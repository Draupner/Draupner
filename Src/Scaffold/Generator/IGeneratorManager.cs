using System.Collections.Generic;

namespace Scaffold.Generator
{
    public interface IGeneratorManager
    {
        void ExecuteGenerator(string command, List<string> args);
        ICollection<IGenerator> AllGenerators();
    }
}