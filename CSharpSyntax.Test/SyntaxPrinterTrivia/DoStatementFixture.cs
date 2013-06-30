using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class DoStatementFixture : TestBase
    {
        [Test]
        public void EmptyBlock()
        {
            Test(
@"// Before 1
do
{
}
while (1);
// After 1
",
                
                Syntax.DoStatement(
                    condition: Syntax.LiteralExpression(1),
                    statement: Syntax.Block()
                )
            );
        }

        [Test]
        public void SingleStatement()
        {
            Test(
@"// Before 1
do
    // Before 2
    break;
    // After 2
while (1);
// After 1
",
                Syntax.DoStatement(
                    condition: Syntax.LiteralExpression(1),
                    statement: Syntax.BreakStatement()
                )
            );
        }
    }
}
