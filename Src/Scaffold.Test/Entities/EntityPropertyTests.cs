using NUnit.Framework;
using Scaffold.Entities;

namespace Scaffold.Test.Entities
{
    [TestFixture]
    public class EntityPropertyTests
    {
        [Test]
        public void ShouldCreateEntityProperty()
        {
            var property = new EntityProperty{Name ="Foo", Type = "System.String"};
            Assert.AreEqual("Foo", property.Name);
            Assert.AreEqual("System.String", property.Type);
        }

        [Test]
        public void ShouldGetStringTypeName()
        {
            var property = new EntityProperty { Name = "Foo", Type = "System.String" };
            Assert.AreEqual("string", property.TypeName);
        }

        [Test]
        public void ShouldGetLongTypeName()
        {
            var property = new EntityProperty { Name = "Foo", Type = "System.Int64" };
            Assert.AreEqual("long", property.TypeName);
        }

        [Test]
        public void ShouldGetVariableName()
        {
            var property = new EntityProperty { Name = "Foo", Type = "System.Int64" };
            Assert.AreEqual("foo", property.VariableName);
        }
    }
}