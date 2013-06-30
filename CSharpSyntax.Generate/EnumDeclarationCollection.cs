using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSyntax.Generate
{
    internal class EnumDeclarationCollection : KeyedCollection<string, IEnumDeclaration>
    {
        protected override string GetKeyForItem(IEnumDeclaration item)
        {
            return item.Name;
        }
    }
}
