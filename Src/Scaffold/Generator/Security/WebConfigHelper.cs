using System;
using Scaffold.Configuration;
using Scaffold.Generator.Helpers;
using Scaffold.Io;

namespace Scaffold.Generator.Security
{
    public class WebConfigHelper : IWebConfigHelper
    {
        private readonly IConfiguration configuration;
        private readonly FileMerger fileMerger;

        public WebConfigHelper(IFileSystem fileSystem, IConfiguration configuration)
        {
            this.configuration = configuration;
            fileMerger = new FileMerger(fileSystem);
        }

        public void AddMembershipProviderToWebConfig()
        {
            Console.WriteLine("Adding membership provider to web.config");

            string memshipSnippet =
@"    <membership defaultProvider='NHibernateMembershipProvider' userIsOnlineTimeWindow='20'>
      <providers>
        <clear />
        <add name='NHibernateMembershipProvider' type='" + configuration.WebNameSpace + @".Common.Security.NHibernateMembershipProvider, " + configuration.WebNameSpace + @"' enablePasswordRetrieval='false' enablePasswordReset='true' requiresQuestionAndAnswer='false' requiresUniqueEmail='false' maxInvalidPasswordAttempts='3' minRequiredPasswordLength='6' minRequiredNonAlphanumericCharacters='0' passwordStrengthRegularExpression='' passwordAttemptWindow='10' passwordFormat='Hashed' applicationName='" + configuration.ProjectName + @"' />
      </providers>
    </membership>";
            fileMerger.InsertLineBefore(WebConfigPath(configuration), memshipSnippet, "</system.web>", "</membership>");

        }

        public void AddRoleProviderToWebConfig()
        {
            Console.WriteLine("Adding role provider to web.config");

            string roleSnippet =
@"    <roleManager enabled='true' defaultProvider='NHibernateRoleProvider'>
      <providers>
        <clear />
        <add name='NHibernateRoleProvider' type='" + configuration.WebNameSpace + @".Common.Security.NHibernateRoleProvider," + configuration.WebNameSpace + @"' applicationName='" + configuration.ProjectName + @"' />
      </providers>
    </roleManager>";
            fileMerger.InsertLineBefore(WebConfigPath(configuration), roleSnippet, "</system.web>", "</roleManager>");
        }

        private static string WebConfigPath(IConfiguration configuration)
        {
            return configuration.WebNameSpace + "\\web.config";
        }
   }
}