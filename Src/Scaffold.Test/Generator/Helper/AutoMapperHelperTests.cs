using System;
using NUnit.Framework;
using Rhino.Mocks;
using Scaffold.Configuration;
using Scaffold.Generator.Helpers;
using Scaffold.Io;

namespace Scaffold.Test.Generator.Helper
{
    [TestFixture]
    public class AutoMapperHelperTests
    {
        private AutoMapperHelper autoMapperHelper;
        private IFileSystem fileSystemMock;
        private IConfiguration configurationMock;

        [SetUp]
        public void SetUp()
        {
            fileSystemMock = MockRepository.GenerateMock<IFileSystem>();
            configurationMock = MockRepository.GenerateMock<IConfiguration>();

            autoMapperHelper = new AutoMapperHelper(fileSystemMock, configurationMock);
        }

        [Test]
        public void ShouldAddToAutoMapperConfiguration()
        {
            string fileContent = @"using System.Linq;
using Blah.Web.Common.Security;

namespace Blah.Web.Common.AutoMapper
{
    public class AutoMapperConfiguration
    {
        public void Configure()
        {
            Mapper.CreateMap<Role, RoleViewModel>();
        }
    }
}";

            const string expectedFileContent = @"using Blah.Web.Models;
using Blah.Core.Domain.Model;
using System.Linq;
using Blah.Web.Common.Security;

namespace Blah.Web.Common.AutoMapper
{
    public class AutoMapperConfiguration
    {
        public void Configure()
        {
            Mapper.CreateMap<Page<Book>, IPagination<BookViewModel>>().ConvertUsing(x => new CustomPagination<BookViewModel>(Mapper.Map<IEnumerable<Book>, IEnumerable<BookViewModel>>(x.Items), x.PageNumber, x.PageSize, x.TotalItemCount));
            Mapper.CreateMap<BookViewModel, Book>();
            Mapper.CreateMap<Book, BookViewModel>();
            Mapper.CreateMap<Role, RoleViewModel>();
        }
    }
}
";
            
            configurationMock.Stub(x => x.WebNameSpace).Return("Blah.Web");
            configurationMock.Stub(x => x.CoreNameSpace).Return("Blah.Core");
            configurationMock.Stub(x => x.TestNameSpace).Return("Blah.Test");

            fileSystemMock.Stub(x => x.FileReadText("Blah.Web\\Common\\AutoMapper\\AutoMapperConfiguration.cs")).Do(new Func<string, string>(filePath => fileContent));
            fileSystemMock.Stub(x => x.FileWriteText(Arg<string>.Is.Equal("Blah.Web\\Common\\AutoMapper\\AutoMapperConfiguration.cs"), Arg<string>.Is.Anything)).Do(new Action<string, string>((filePath, newFileContent) => fileContent = newFileContent));

            autoMapperHelper.AddAutoMapperConfiguration("Book", "BookViewModel");

            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal("Blah.Web\\Common\\AutoMapper\\AutoMapperConfiguration.cs"), Arg<string>.Is.Anything));

            Assert.AreEqual(expectedFileContent, fileContent);

        }
    }
}