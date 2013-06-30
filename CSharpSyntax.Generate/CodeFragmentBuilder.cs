using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSyntax.Generate
{
    public class CodeFragmentBuilder
    {
        private readonly List<CodeFragmentLine> _lines = new List<CodeFragmentLine>();
        private readonly StringBuilder _sb = new StringBuilder();
        private int _indent;

        public CodeFragment GetFragment()
        {
            if (_sb.Length > 0)
                AppendLine();

            return new CodeFragment(_lines.ToArray());
        }

        public void Indent()
        {
            _indent++;
        }

        public void Unindent()
        {
            _indent--;
        }

        public void Append(string text)
        {
            Append(text, null);
        }

        public void Append(string text, params object[] args)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            if (args != null && args.Length > 0)
                text = String.Format(text, args);

            _sb.Append(text);
        }

        public void AppendLine()
        {
            AppendLine(null, null);
        }

        public void AppendLine(string text)
        {
            AppendLine(text, null);
        }

        public void AppendLine(string text, params object[] args)
        {
            if (text != null)
                Append(text, args);

            _lines.Add(new CodeFragmentLine(_indent, _sb.ToString()));

            _sb.Clear();
        }
    }
}
