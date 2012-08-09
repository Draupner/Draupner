using NUnit.Framework;
using Scaffold.Common;

namespace Scaffold.Test.Common
{
    [TestFixture]
    public class StringHelperTests
    {
        [Test]
        public void ShouldMakeTheFirstCharacterLowerCase()
        {
            Assert.AreEqual("helloWorld", StringHelper.LowercaseFirst("HelloWorld"));
            Assert.AreEqual("", StringHelper.LowercaseFirst(""));
            Assert.AreEqual(null, StringHelper.LowercaseFirst(null));
            Assert.AreEqual("helloWorld", StringHelper.LowercaseFirst("helloWorld"));
            Assert.AreEqual("shouldMakeTheFirstCharacterLowerCase", StringHelper.LowercaseFirst("ShouldMakeTheFirstCharacterLowerCase"));
            Assert.AreEqual("foo", StringHelper.LowercaseFirst("Foo"));
            Assert.AreEqual("f", StringHelper.LowercaseFirst("F"));
        }
    }
}