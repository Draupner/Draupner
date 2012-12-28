using System;
using Scaffold.Common;
using Scaffold.Exceptions;
using Scaffold.Generator;
using System.Linq;

namespace Scaffold
{
    public class Program
    {
        private static IGeneratorManager generatorManager;

        static void Main(string[] args)
        {
            try
            {
                Ioc.Configure();
                generatorManager = Ioc.Container.Resolve<IGeneratorManager>();

                if (args.Length == 0)
                {
                    ShowHelp();
                    return;
                }

                string command = FindCommand(args);
                var generatorArgs = args.Skip(1).ToList();

                generatorManager.ExecuteGenerator(command, generatorArgs);
            } 
            catch(EntityNotFoundException e)
            {
                Console.WriteLine(e.Message);
            } 
            catch(GeneratorNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ConfigurationFileNotFoundException e)
            {
                Console.WriteLine(e.Message + ". Are you sure this is a scaffolded project?");
            } 
            catch(EntityParsingException e)
            {
                Console.WriteLine("Error parsing exception: " + e.Message);                
            } 
            catch(IllegalGeneratorArgs e)
            {
                Console.WriteLine("Incorrect number of arguments");
                Console.WriteLine(e.Message);
            }
        }

        private static void ShowHelp()
        {
            const string help = "Usage: scaffold COMMAND [options]\n\nCommands:\n";

            Console.WriteLine(help);
            foreach (var generator in generatorManager.AllGenerators())
            {
                Console.WriteLine(generator.Command + ": ");
                Console.WriteLine(generator.Description);
                Console.WriteLine();
            }
        }

        private static string FindCommand(string[] args)
        {
            return args[0];
        }
    }
}
