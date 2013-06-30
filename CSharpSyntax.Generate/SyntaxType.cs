using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSyntax.Generate
{
    internal class SyntaxType
    {
        private static readonly Dictionary<Type, SyntaxType> _types = new Dictionary<Type, SyntaxType>();

        public static SyntaxType GetType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            SyntaxType result;
            if (_types.TryGetValue(type, out result))
                return result;

            return new SyntaxType(type);
        }

        public SyntaxType()
        {
            Properties = new SyntaxPropertyCollection();
        }

        private SyntaxType(Type type)
            : this()
        {
            _types.Add(type, this);

            IsAbstract = type.IsAbstract;
            Name = type.Name;

            foreach (var property in type.GetProperties().Where(p =>
                p.DeclaringType == type && p.GetMethod.GetBaseDefinition() == p.GetMethod
            ))
            {
                Properties.Add(new SyntaxProperty(property));
            }
            
            if (type.BaseType != null)
                BaseType = GetType(type.BaseType);
        }

        public bool IsAbstract { get; set; }

        public string Name { get; set; }

        public SyntaxType BaseType { get; set; }

        public SyntaxPropertyCollection Properties { get; private set; }

        public bool IsAssignableFrom(SyntaxType type)
        {
            while (type != null)
            {
                if (type == this)
                    return true;

                type = type.BaseType;
            }

            return false;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
