using Scaffold.Configuration;

namespace Scaffold.Generator.Helpers
{
    public interface IAutoMapperHelper
    {
        void AddAutoMapperConfiguration(string entityName, string viewModelName);
    }
}