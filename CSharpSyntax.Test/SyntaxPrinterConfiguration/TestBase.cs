using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSyntax.Test.SyntaxPrinterConfiguration
{
    public abstract class TestBase : Test.TestBase
    {
        protected void Test(string expected, SyntaxNode node, Action<Printer.Configuration.SyntaxPrinterConfiguration> configure)
        {
            Test(expected, new[] { node }, configure);
        }

        protected virtual void Test(string expected, IEnumerable<SyntaxNode> nodes, Action<Printer.Configuration.SyntaxPrinterConfiguration> configure)
        {
            var configuration = new Printer.Configuration.SyntaxPrinterConfiguration();

            configure(configuration);

            Test(expected, nodes, configuration);
        }
    }
}
