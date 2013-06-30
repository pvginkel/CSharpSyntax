using System.Collections.ObjectModel;
using System.Text;
using System.Collections.Generic;
using System;

namespace CSharpSyntax.Generate
{
    internal class SyntaxPropertyCollection : KeyedCollection<string, SyntaxProperty>
    {
        protected override string GetKeyForItem(SyntaxProperty item)
        {
            return item.Name;
        }
    }
}