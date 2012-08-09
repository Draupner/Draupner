using System.Collections.Generic;

namespace Scaffold.Generator
{
    public interface IGenerator
    {
        string Command { get; }
        string Description { get; }
        bool CanHandle(string command);
        void Execute(List<string> args);
    }
}