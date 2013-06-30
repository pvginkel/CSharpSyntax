using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSyntax.Generate
{
    internal class SyntaxProperty
    {
        public SyntaxProperty()
        {
        }

        public SyntaxProperty(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            DeclaringType = SyntaxType.GetType(property.DeclaringType);
            Name = property.Name;
            PropertyType = property.PropertyType;
        }

        public SyntaxType DeclaringType { get; set; }

        public string Name { get; set; }

        public Type PropertyType { get; set; }
    }
}
