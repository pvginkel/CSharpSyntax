using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSyntax.Generate
{
    public class CodeFragment
    {
        private readonly CodeFragmentLine[] _lines;

        public CodeFragment(params CodeFragmentLine[] lines)
        {
            if (lines == null)
                throw new ArgumentNullException("lines");

            _lines = lines;
        }

        public string ToString(int indentation)
        {
            var sb = new StringBuilder();

            foreach (var line in _lines)
            {
                int indent = indentation + line.Indentation;

                if (indent > 0)
                    sb.Append(' ', indent * 4);

                sb.AppendLine(line.Text);
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToString(0);
        }
    }
}
