using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSyntax.Generate
{
    internal class SyntaxParameter
    {
        public SyntaxParameter()
        {
        }

        public SyntaxParameter(ParameterInfo parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException("parameter");

            Name = parameter.Name;
            ParameterType = parameter.ParameterType;
            IsOptional = parameter.IsOptional;
            IsParams = parameter.GetCustomAttribute<ParamArrayAttribute>() != null;
        }

        public string Name { get; set; }

        public Type ParameterType { get; set; }

        public bool IsOptional { get; set; }

        public bool IsParams { get; set; }
    }
}
