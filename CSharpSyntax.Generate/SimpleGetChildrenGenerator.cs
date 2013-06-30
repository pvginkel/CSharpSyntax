using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSyntax.Generate
{
    internal class SimpleGetChildrenGenerator : IGetChildrenGenerator
    {
        public string Name { get; private set; }

        public SimpleGetChildrenGenerator(string name)
        {
            Name = name;
        }

        public void Build(CodeFragmentBuilder builder)
        {
            builder.AppendLine("if ({0} != null)", Name);
            builder.Indent();
            builder.AppendLine("yield return {0};", Name);
            builder.Unindent();
        }
    }
}
