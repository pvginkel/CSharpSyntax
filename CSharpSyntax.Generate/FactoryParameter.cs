using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CSharpSyntax.Generate
{
    internal class FactoryParameter
    {
        public string Name { get; set; }
        public Type OriginalType { get; private set; }
        public string Type { get; set; }
        public bool IsList { get; private set; }
        public bool IsParams { get; private set; }
        public bool IsOptional { get; private set; }
        public string ParseNameType { get; set; }

        public FactoryParameter(string name, Type originalType, string type, bool isList, bool isParams, bool isOptional)
        {
            Name = name;
            OriginalType = originalType;
            Type = type;
            IsList = isList;
            IsParams = isParams;
            IsOptional = isOptional;
        }

        public FactoryParameter(FactoryParameter other)
        {
            Name = other.Name;
            OriginalType = other.OriginalType;
            Type = other.Type;
            IsList = other.IsList;
            IsParams = other.IsParams;
            IsOptional = other.IsOptional;
            ParseNameType = other.ParseNameType;
        }

        public FactoryParameter(SyntaxParameter parameter)
        {
            Name = parameter.Name;

            IsParams = parameter.IsParams;
            IsOptional = parameter.IsOptional;

            if (
                parameter.ParameterType.IsGenericType &&
                new[] { typeof(SeparatedSyntaxList<>), typeof(SyntaxList<>), typeof(IEnumerable<>) }.Any(p => parameter.ParameterType.GetGenericTypeDefinition() == p)
            )
            {
                IsList = true;
                OriginalType = parameter.ParameterType.GetGenericArguments()[0];
            }
            else
            {
                OriginalType = parameter.ParameterType;
            }

            Type = OriginalType.Name;
        }

        public override bool Equals(object obj)
        {
            var other = (FactoryParameter)obj;

            return
                Name == other.Name &&
                //OriginalType == other.OriginalType &&
                Type == other.Type &&
                IsList == other.IsList /*&&
                IsParams == other.IsParams &&
                IsOptional == other.IsOptional &&
                ParseNameType == other.ParseNameType */;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
