using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSyntax.Generate
{
    internal class EnumDeclaration : IEnumDeclaration
    {
        public string Name { get; private set; }

        public string[] FieldNames { get; private set; }

        public EnumDeclaration(string name, string[] fieldNames)
        {
            Name = name;
            FieldNames = fieldNames;
        }

        public void Build(CodeFragmentBuilder builder)
        {
            builder.AppendLine("public enum {0}", Name);
            builder.AppendLine("{");
            builder.Indent();

            var names = new List<string>(FieldNames);

            names.Sort();

            int none = names.IndexOf("None");

            if (none != -1)
            {
                names.RemoveAt(none);
                names.Insert(0, "None");
            }

            for (int i = 0; i < names.Count; i++)
            {
                builder.Append(names[i]);
                if (i != names.Count - 1)
                    builder.Append(",");
                builder.AppendLine();
            }

            builder.Unindent();
            builder.AppendLine("}");
        }
    }
}
