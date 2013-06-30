using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class WhileStatementFixture : TestBase
    {
        [Test]
        public void EmptyBlock()
        {
            Test(
@"while (1)
{
}
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
@"while (1)
    break;
",
                Syntax.WhileStatement(
                    condition: Syntax.LiteralExpression(1),
                    statement: Syntax.BreakStatement()
                )
            );
        }
    }
}
