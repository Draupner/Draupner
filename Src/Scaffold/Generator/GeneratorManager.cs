using System.Collections.Generic;
using System.Linq;
using Scaffold.Common;
using Scaffold.Exceptions;

namespace Scaffold.Generator
{
    public class GeneratorManager : IGeneratorManager
    {
        private static IList<IGenerator> generators;

        public GeneratorManager()
        {
            generators = Ioc.Container.ResolveAll<IGenerator>();
        }

        public void ExecuteGenerator(string command, List<string> args)
        {
            IGenerator genereator = FindGenerator(command);

            genereator.Execute(args);
        }

        private static IGenerator FindGenerator(string command)
        {
            foreach (var generator in generators)
            {
                if (generator.CanHandle(command))
                {
                    return generator;
                }
            }
            throw new GeneratorNotFoundException("Command does not exists: " + command);
        }

        public ICollection<IGenerator> AllGenerators()
        {
            return generators;
        }
    }
}
