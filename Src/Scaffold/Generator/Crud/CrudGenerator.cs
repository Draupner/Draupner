using System;
using System.Collections.Generic;
using Scaffold.Exceptions;

namespace Scaffold.Generator.Crud
{
    public class CrudGenerator : BaseGenerator
    {
        private readonly IGeneratorManager generatorManager;
        private const string command = "create-crud";
        private const string description = "\tusage: create-crud [entity]\n\tCreates NHibernate mappings, Repository and CRUD UI for an entity";

        public CrudGenerator(IGeneratorManager generatorManager)
            : base(command, description)
        {
            this.generatorManager = generatorManager;
        }

        public override void Execute(List<string> args)
        {
            ValidateArgs(args);

            generatorManager.ExecuteGenerator("create-nhibernate-mapping", args);
            generatorManager.ExecuteGenerator("create-repository", args);
            generatorManager.ExecuteGenerator("create-ui-crud", args);
        }

        private static void ValidateArgs(List<string> args)
        {
            if (args.Count != 1)
            {
                throw new IllegalGeneratorArgs(description);
            }
        }
    }
}