using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSyntax.Generate
{
    internal interface IGetChildrenGenerator
    {
        void Build(CodeFragmentBuilder builder);
    }
}
