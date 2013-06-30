using System.Text;
using System.Collections.Generic;
using System;

namespace CSharpSyntax.Generate
{
    public class CodeFragmentLine
    {
        public int Indentation { get; private set; }
        public string Text { get; private set; }

        public CodeFragmentLine(int indentation, string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            Indentation = indentation;
            Text = text;
        }
    }
}