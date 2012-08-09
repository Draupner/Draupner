namespace Scaffold.VisualStudio
{
    public interface IProjectFileManager
    {
        void AddCompileFileToProject(string file, string projectName);
        void AddContentFileToProject(string file, string projectName);
    }
}