using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax.Generate
{
    [Flags]
    public enum MemberModifier
    {
        None = 0,
        Public = 1,
        Protected = 2,
        Internal = 4,
        Private = 8,
        Abstract = 16,
        Sealed = 32,
        Virtual = 64,
        Static = 128,
        Override = 256,
        ReadOnly = 512
    }
}
