using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSyntax.Generate
{
    internal class ListGetChildrenGenerator : IGetChildrenGenerator
    {
        public string Name { get; private set; }

        public ListGetChildrenGenerator(string name)
        {
            Name = name;
        }

        public void Build(CodeFragmentBuilder builder)
        {
            builder.AppendLine("foreach (var item in {0})", Name);
            builder.AppendLine("{");
            builder.Indent();

            builder.AppendLine("yield return item;");

            builder.Unindent();
            builder.AppendLine("}");
        }
    }
}
