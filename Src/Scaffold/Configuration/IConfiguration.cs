namespace Scaffold.Configuration
{
    public interface IConfiguration
    {
        string ProjectName { get; }
        string CoreNameSpace { get; }
        string WebNameSpace { get; }
        string TestNameSpace { get; }
    }
}