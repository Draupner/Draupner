using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using Scaffold.Generator.Helpers;
using Scaffold.Io;

namespace Scaffold.Test.Generator.Helper
{
    [TestFixture]
    public class FileMergerTests
    {
        private FileMerger fileMerger;
        private IFileSystem fileSystemMock;

        [SetUp]
        public void SetUp()
        {
            fileSystemMock = MockRepository.GenerateMock<IFileSystem>();
            fileMerger = new FileMerger(fileSystemMock);
        }

        [Test]
        public void ShouldInsertBefore()
        {
            const string fileContent = @"public class Blah {
    public void Register() {
        Bar();
        Bar();
    }
}
";
            const string expectedfileContent = @"public class Blah {
    public void Register() {
        Foo();
        Bar();
        Bar();
    }
}
";
            fileSystemMock.Stub(x => x.FileReadText("file.txt")).Return(fileContent);

            fileMerger.InsertLineBefore("file.txt", "        Foo();", "Bar();", "Foo()");

            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal("file.txt"), Arg<string>.Is.Anything));
            IList<object[]> arguments = fileSystemMock.GetArgumentsForCallsMadeOn(x => x.FileWriteText(null, null));
            string newFileContent = (string) arguments[0][1];
            Assert.AreEqual(expectedfileContent, newFileContent);
        }

        [Test]
        public void ShouldNotInsertBeforeSinceIgnored()
        {
            const string fileContent = @"public class Blah {
    public void Register() {
        Foo();
        Bar();
        Bar();
    }
}
";
            fileSystemMock.Stub(x => x.FileReadText("file.txt")).Return(fileContent);

            fileMerger.InsertLineBefore("file.txt", "        Foo();", "Bar();", "Foo()");

            fileSystemMock.AssertWasNotCalled(x => x.FileWriteText(Arg<string>.Is.Equal("file.txt"), Arg<string>.Is.Anything));
        }

        [Test]
        public void ShouldInsertFirst()
        {
            const string fileContent = @"public class Blah {
    public void Register() {
        Bar();
        Bar();
    }
}
";
            const string expectedfileContent = @"using System.Linq;
public class Blah {
    public void Register() {
        Bar();
        Bar();
    }
}
";
            fileSystemMock.Stub(x => x.FileReadText("file.txt")).Return(fileContent);

            fileMerger.InsertFirstLine("file.txt", "using System.Linq;", "using System.Linq;");

            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal("file.txt"), Arg<string>.Is.Anything));
            IList<object[]> arguments = fileSystemMock.GetArgumentsForCallsMadeOn(x => x.FileWriteText(null, null));
            string newFileContent = (string)arguments[0][1];
            Assert.AreEqual(expectedfileContent, newFileContent);
        }

        [Test]
        public void ShouldNotInsertFirstSinceIgnored()
        {
            const string fileContent = @"using System.Linq;
public class Blah {
    public void Register() {
        Bar();
        Bar();
    }
}
";
            fileSystemMock.Stub(x => x.FileReadText("file.txt")).Return(fileContent);

            fileMerger.InsertFirstLine("file.txt", "using System.Linq;", "using System.Linq;");

            fileSystemMock.AssertWasNotCalled(x => x.FileWriteText(Arg<string>.Is.Equal("file.txt"), Arg<string>.Is.Anything));
        }

        [Test]
        public void ShouldInsertAfterLast()
        {
            const string fileContent = @"public class Blah {
    public void Register() {
        Bar();
        Bar();
    }
}
";
            const string expectedfileContent = @"public class Blah {
    public void Register() {
        Bar();
        Bar();
        Foo();
    }
}
";
            fileSystemMock.Stub(x => x.FileReadText("file.txt")).Return(fileContent);

            fileMerger.InsertLineAfterLast("file.txt", "        Foo();", "Bar();", "Foo()");

            fileSystemMock.AssertWasCalled(x => x.FileWriteText(Arg<string>.Is.Equal("file.txt"), Arg<string>.Is.Anything));
            IList<object[]> arguments = fileSystemMock.GetArgumentsForCallsMadeOn(x => x.FileWriteText(null, null));
            string newFileContent = (string)arguments[0][1];
            Assert.AreEqual(expectedfileContent, newFileContent);
        }
    }
}