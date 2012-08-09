using System.Collections.Generic;
using Scaffold.Common;

namespace Scaffold.Entities
{
    public class EntityProperty
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public string VariableName
        {
            get { return StringHelper.LowercaseFirst(Name); }
        }
        
        public string TypeName
        {
            get { return typeNames.ContainsKey(Type) ? typeNames[Type] : Type; }
        }
        
        public bool IsNullable
        {
            get
            {
                return !nonNullableTypes.Contains(TypeName); 
            }
        }

        private readonly Dictionary<string, string> typeNames = new Dictionary<string, string>
                                                           {
                                                               {"System.Boolean", "bool"},
                                                               {"System.Byte", "byte"},
                                                               {"System.Char", "char"},
                                                               {"System.Int16", "short"},
                                                               {"System.Int32", "int"},
                                                               {"System.Int64", "long"},
                                                               {"System.UInt16", "ushort"},
                                                               {"System.UInt32", "uint"},
                                                               {"System.UInt64", "ulong"},
                                                               {"System.Single", "float"},
                                                               {"System.Double", "double"},
                                                               {"System.Decimal", "decimal"},
                                                               {"System.DateTime", "DateTime"},
                                                               {"System.DateTimeOffset", "DateTimeOffset"},
                                                               {"System.TimeSpan", "TimeSpan"},
                                                               {"System.String", "string"}
                                                           };
        private readonly IList<string> nonNullableTypes = new List<string>
                                                           {
                                                               "bool",
                                                               "byte",
                                                               "char",
                                                               "short",
                                                               "int",
                                                               "long",
                                                               "float",
                                                               "double",
                                                               "decimal"
                                                           }; 
    }
}