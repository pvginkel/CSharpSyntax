using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSyntax.Generate
{
    internal class SyntaxMethod
    {
        public SyntaxMethod()
        {
            Parameters = new List<SyntaxParameter>();
        }

        public SyntaxMethod(MethodInfo method)
            : this()
        {
            if (method == null)
                throw new ArgumentNullException("method");

            ReturnType = SyntaxType.GetType(method.ReturnType);
            Name = method.Name;

            foreach (var parameter in method.GetParameters())
            {
                Parameters.Add(new SyntaxParameter(parameter));
            }
        }

        public SyntaxType ReturnType { get; set; }

        public string Name { get; set; }

        public List<SyntaxParameter> Parameters { get; private set; }
    }
}
