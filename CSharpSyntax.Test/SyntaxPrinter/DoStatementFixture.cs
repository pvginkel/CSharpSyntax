using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class DoStatementFixture : TestBase
    {
        [Test]
        public void EmptyBlock()
        {
            Test(
@"do
{
}
while (1);
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
@"do
    break;
while (1);
",
                Syntax.DoStatement(
                    condition: Syntax.LiteralExpression(1),
                    statement: Syntax.BreakStatement()
                )
            );
        }
    }
}
