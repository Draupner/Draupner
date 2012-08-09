using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Scaffold.Entities;

namespace Scaffold.Test.Entities
{
    [TestFixture]
    public class EntityTests
    {
        [Test]
        public void ShouldCreateEntity()
        {
            var entity = new Entity
                             {
                                 Name = "Foo",
                                 Properties =
                                     new List<EntityProperty> {new EntityProperty {Name = "Bar", Type = "System.String"}}
                             };
            Assert.AreEqual("Foo", entity.Name);
            Assert.AreEqual(1, entity.Properties.Count);
        }

        [Test]
        public void ShouldGetBasicProperties()
        {
            var properties = new List<EntityProperty>
                                 {
                                     CreateProperty("1", "System.String"),
                                     CreateProperty("2", "System.Int32"),
                                     CreateProperty("3", "Scaffold.Test.Entities.Foo"),
                                     CreateProperty("4", "ICollection"),
                                     CreateProperty("5", "System.Boolean")
                                 };
            var entity = new Entity {Name = "Foo", Properties = properties};
            CollectionAssert.AreEqual(new[] {"1", "2", "5"},
                                      entity.BasicProperties.Select(x => x.Name).ToList());
        }

        [Test]
        public void ShouldGetCollectionProperties()
        {
            var properties = new List<EntityProperty>
                                 {
                                     CreateProperty("1", "IList"),
                                     CreateProperty("2", "System.Int32"),
                                     CreateProperty("3", "Scaffold.Test.Entities.Foo"),
                                     CreateProperty("4", "ICollection"),
                                     CreateProperty("5", "System.Boolean")
                                 };
            var entity = new Entity { Name = "Foo", Properties = properties };
            CollectionAssert.AreEqual(new[] { "1", "4"},
                                      entity.CollectionProperties.Select(x => x.Name).ToList());
        }

        [Test]
        public void ShouldGetReferenceProperties()
        {
            var properties = new List<EntityProperty>
                                 {
                                     CreateProperty("1", "IList"),
                                     CreateProperty("2", "System.Int32"),
                                     CreateProperty("3", "Scaffold.Test.Entities.Foo"),
                                     CreateProperty("4", "ICollection"),
                                     CreateProperty("5", "System.Boolean")
                                 };
            var entity = new Entity { Name = "Foo", Properties = properties };
            CollectionAssert.AreEqual(new[] { "3" },
                                      entity.ReferenceProperties.Select(x => x.Name).ToList());
        }

        private EntityProperty CreateProperty(string name, string type)
        {
            return new EntityProperty {Name = name, Type = type};
        }
    }
}