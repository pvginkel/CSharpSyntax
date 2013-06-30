using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CSharpSyntax.Generate
{
    internal class Factory
    {
        public string Name { get; set; }
        public List<FactoryParameter> Parameters { get; private set; }

        public Factory(SyntaxMethod method)
        {
            Name = method.Name;

            Parameters = new List<FactoryParameter>();

            foreach (var parameter in method.Parameters)
            {
                if (
                    parameter.ParameterType == typeof(SyntaxToken) &&
                    parameter.Name == "identifier")
                {
                    Parameters.Add(new FactoryParameter(
                        parameter.Name,
                        typeof(string),
                        typeof(string).Name,
                        false,
                        false,
                        false
                    ));
                }
                else if (
                    method.ReturnType.Name == "IdentifierNameSyntax" &&
                    parameter.Name == "name"
                )
                {
                    Parameters.Add(new FactoryParameter(
                        "identifier",
                        typeof(string),
                        typeof(string).Name,
                        false,
                        false,
                        false
                    ));
                }
                else if (
                    method.ReturnType.Name == "OperatorDeclarationSyntax" &&
                    parameter.Name == "operatorToken"
                )
                {
                    Parameters.Add(new FactoryParameter(
                        "operator",
                        typeof(SyntaxToken),
                        "Operator",
                        false,
                        false,
                        false
                    ));
                }
                else if (
                    method.ReturnType.Name == "ConversionOperatorDeclarationSyntax" &&
                    parameter.Name == "implicitOrExplicitKeyword"
                )
                {
                    Parameters.Add(new FactoryParameter(
                        "kind",
                        typeof(SyntaxToken),
                        "ImplicitOrExplicit",
                        false,
                        false,
                        false
                    ));
                }
                else if (parameter.ParameterType == typeof(AttributeTargetSpecifierSyntax))
                {
                    Parameters.Add(new FactoryParameter(
                        parameter.Name,
                        typeof(AttributeTargetSpecifierSyntax),
                        "AttributeTarget",
                        false,
                        false,
                        false
                    ));
                }
                else if (!Ignore(parameter))
                {
                    Parameters.Add(new FactoryParameter(parameter));
                }
            }
        }

        private bool Ignore(SyntaxParameter parameter)
        {
            return
                typeof(SyntaxToken).IsAssignableFrom(parameter.ParameterType) ||
                Generator.IgnoredTypes.Any(p1 => p1.IsAssignableFrom(parameter.ParameterType));
        }

        public Factory(string name)
        {
            Name = name;
            Parameters = new List<FactoryParameter>();
        }

        public override bool Equals(object obj)
        {
            var other = (Factory)obj;

            if (Name != other.Name || Parameters.Count != other.Parameters.Count)
                return false;

            for (int i = 0; i < Parameters.Count; i++)
            {
                if (!Parameters[i].Equals(other.Parameters[i]))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
