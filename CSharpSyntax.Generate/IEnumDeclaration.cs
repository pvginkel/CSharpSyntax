using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSyntax.Generate
{
    internal interface IEnumDeclaration
    {
        string Name { get; }

        void Build(CodeFragmentBuilder builder);
    }
}
