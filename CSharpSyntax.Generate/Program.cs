using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSyntax.Generate
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            new Generator().Generate(args[0]);
        }
    }
}
