using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSyntax.Generate
{
    internal class FlagsEnumDeclaration : IEnumDeclaration
    {
        public string Name { get; private set; }

        public string[] FieldNames { get; private set; }

        public bool AddAll { get; private set; }

        public FlagsEnumDeclaration(string name, string[] fieldNames, bool addAll)
        {
            Name = name;
            FieldNames = fieldNames;
            AddAll = addAll;
        }

        public void Build(CodeFragmentBuilder builder)
        {
            builder.AppendLine("[Flags]");
            builder.AppendLine("public enum {0}", Name);
            builder.AppendLine("{");
            builder.Indent();

            builder.AppendLine("None = 0,");

            for (int i = 0; i < FieldNames.Length; i++)
            {
                builder.Append(FieldNames[i]);
                builder.Append(" = 1 << ");
                builder.Append(i.ToString(CultureInfo.InvariantCulture));
                if (i != FieldNames.Length - 1 || AddAll)
                    builder.Append(",");
                builder.AppendLine();
            }

            if (AddAll)
            {
                builder.Append("All = (1 << ");
                builder.Append(FieldNames.Length.ToString(CultureInfo.InvariantCulture));
                builder.AppendLine(") - 1");
            }

            builder.Unindent();
            builder.AppendLine("}");
        }
    }
}
