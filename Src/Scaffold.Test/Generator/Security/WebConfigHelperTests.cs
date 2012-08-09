using NUnit.Framework;
using Rhino.Mocks;
using Scaffold.Configuration;
using Scaffold.Generator.Security;
using Scaffold.Io;

namespace Scaffold.Test.Generator.Security
{
    [TestFixture]
    public class WebConfigHelperTests
    {
        private IWebConfigHelper webConfigHelper;
        private IFileSystem fileSystemMock;
        private IConfiguration configurationMock;

        [SetUp]
        public void SetUp()
        {
            fileSystemMock = MockRepository.GenerateMock<IFileSystem>();
            configurationMock = MockRepository.GenerateMock<IConfiguration>();

            webConfigHelper = new WebConfigHelper(fileSystemMock, configurationMock);
        }

        [Test]
        public void ShouldAddMemberShipProviderToWebConfig()
        {
            const string webConfigContent = @"<configuration>
  <system.web>
  </system.web>
<configuration>";

            const string expectedWebConfigContent = @"<configuration>
  <system.web>
    <membership defaultProvider='NHibernateMembershipProvider' userIsOnlineTimeWindow='20'>
      <providers>
        <clear />
        <add name='NHibernateMembershipProvider' type='Blah.Web.Common.Security.NHibernateMembershipProvider, Blah.Web' enablePasswordRetrieval='false' enablePasswordReset='true' requiresQuestionAndAnswer='false' requiresUniqueEmail='false' maxInvalidPasswordAttempts='3' minRequiredPasswordLength='6' minRequiredNonAlphanumericCharacters='0' passwordStrengthRegularExpression='' passwordAttemptWindow='10' passwordFormat='Hashed' applicationName='Blah' />
      </providers>
    </membership>
  </system.web>
<configuration>
";

            configurationMock.Stub(x => x.WebNameSpace).Return("Blah.Web");
            configurationMock.Stub(x => x.ProjectName).Return("Blah");
            fileSystemMock.Stub(x => x.FileReadText("Blah.Web\\web.config")).Return(webConfigContent);

            webConfigHelper.AddMembershipProviderToWebConfig();

            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal("Blah.Web\\web.config"), Arg<string>.Is.Anything));
            var arguments = fileSystemMock.GetArgumentsForCallsMadeOn(x => x.FileWriteText(null, null));
            var newWebConfigContent = (string) arguments[0][1];
            Assert.AreEqual(expectedWebConfigContent, newWebConfigContent);
        }

        [Test]
        public void ShouldAddRoleProviderToWebConfig()
        {
            const string webConfigContent = @"<configuration>
  <system.web>
  </system.web>
<configuration>";

            const string expectedWebConfigContent = @"<configuration>
  <system.web>
    <roleManager enabled='true' defaultProvider='NHibernateRoleProvider'>
      <providers>
        <clear />
        <add name='NHibernateRoleProvider' type='Blah.Web.Common.Security.NHibernateRoleProvider,Blah.Web' applicationName='Blah' />
      </providers>
    </roleManager>
  </system.web>
<configuration>
";

            configurationMock.Stub(x => x.WebNameSpace).Return("Blah.Web");
            configurationMock.Stub(x => x.ProjectName).Return("Blah");
            fileSystemMock.Stub(x => x.FileReadText("Blah.Web\\web.config")).Return(webConfigContent);

            webConfigHelper.AddRoleProviderToWebConfig();

            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal("Blah.Web\\web.config"), Arg<string>.Is.Anything));
            var arguments = fileSystemMock.GetArgumentsForCallsMadeOn(x => x.FileWriteText(null, null));
            var newWebConfigContent = (string)arguments[0][1];
            Assert.AreEqual(expectedWebConfigContent, newWebConfigContent);
        }
    }
}