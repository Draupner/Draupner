using Scaffold.Configuration;

namespace Scaffold.Generator.Security
{
    public interface IWebConfigHelper
    {
        void AddMembershipProviderToWebConfig();
        void AddRoleProviderToWebConfig();
    }
}