using System.Collections.Generic;
using System.Linq;
using Scaffold.Common;

namespace Scaffold.Entities
{
    public class Entity
    {
        public Entity()
        {
            Properties = new List<EntityProperty>();
        }

        public string Name { get; set; }
        public IList<EntityProperty> Properties { get; set; }

        public string VariableName
        {
            get { return StringHelper.LowercaseFirst(Name);  }
        }

        private static readonly List<string> BasicTypes = new List<string>
                                                              {
                                                                  "System.Int16",
                                                                  "System.Int32",
                                                                  "System.Int64",
                                                                  "System.UInt16",
                                                                  "System.UInt32",
                                                                  "System.UInt64",
                                                                  "System.Boolean",
                                                                  "System.String",
                                                                  "DateTime",
                                                                  "DateTimeOffset",
                                                                  "System.Byte",
                                                                  "System.Char",
                                                                  "System.Single",
                                                                  "System.Double",
                                                                  "System.Decimal",
                                                                  "TimeSpan"
                                                              };

        private static readonly List<string> CollectionTypes = new List<string>
                                                                   {
                                                                       "ICollection",
                                                                       "ISet",
                                                                       "IList",
                                                                   };

        public IList<EntityProperty> BasicProperties
        {
            get
            {
                return (Properties
                    .Where(property => BasicTypes.Contains(property.Type))
                    .Select(property => property))
                    .ToList();                
            }
        }

        public IList<EntityProperty> ReferenceProperties
        {
            get
            {
                return (Properties
                    .Where(property => !CollectionTypes.Contains(property.Type) && !BasicTypes.Contains(property.Type))
                    .Select(property => property))
                    .ToList();                
            }
        }

        public IList<EntityProperty> CollectionProperties
        {
            get
            {
                return (Properties
                    .Where(property => CollectionTypes.Contains(property.Type))
                    .Select(property => property))
                    .ToList();                
            }
        }
    }
}
