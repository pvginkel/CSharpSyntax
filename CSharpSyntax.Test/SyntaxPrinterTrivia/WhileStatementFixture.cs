using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class WhileStatementFixture : TestBase
    {
        [Test]
        public void EmptyBlock()
        {
            Test(
@"// Before 1
while (1)
{
}
// After 1
",

                Syntax.WhileStatement(
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
while (1)
    // Before 2
    break;
    // After 2
// After 1
",
                Syntax.WhileStatement(
                    condition: Syntax.LiteralExpression(1),
                    statement: Syntax.BreakStatement()
                )
            );
        }
    }
}
