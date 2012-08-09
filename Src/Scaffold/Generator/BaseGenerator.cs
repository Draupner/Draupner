using System;
using System.Collections.Generic;

namespace Scaffold.Generator
{
    public abstract class BaseGenerator : IGenerator
    {
        private readonly string command;
        private readonly string description;

        protected BaseGenerator(string command, string description)
        {
            this.command = command;
            this.description = description;
        }

        public string Command
        {
            get { return command; }
        }

        public string Description
        {
            get { return description; }
        }

        public bool CanHandle(string command)
        {
            return this.command == command;
        }

        public abstract void Execute(List<string> args);
    }
}