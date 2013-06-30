using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpSyntax.Printer;
using NUnit.Framework;

namespace CSharpSyntax.Test
{
    public abstract class TestBase
    {
        protected void Test(string expected, SyntaxNode node)
        {
            Test(expected, new[] { node }, new Printer.Configuration.SyntaxPrinterConfiguration());
        }

        protected virtual void Test(string expected, IEnumerable<SyntaxNode> nodes, Printer.Configuration.SyntaxPrinterConfiguration configuration)
        {
            using (var writer = new StringWriter())
            {
                foreach (var node in nodes)
                {
                    using (var printer = new Printer.SyntaxPrinter(new SyntaxWriter(writer, configuration)))
                    {
                        printer.Visit(node);
                    }
                }

                Assert.AreEqual(expected, writer.GetStringBuilder().ToString());
            }
        }
    }
}
