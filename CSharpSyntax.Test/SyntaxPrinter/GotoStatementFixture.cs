using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class GotoStatementFixture : TestBase
    {
        [Test]
        public void Goto()
        {
            Test(
@"goto 1;
",
                Syntax.GotoStatement(
                    expression: Syntax.LiteralExpression(1)
                )
            );
        }

        [Test]
        public void GotoCase()
        {
            Test(
@"goto case 1;
",
                Syntax.GotoStatement(
                    CaseOrDefault.Case,
                    Syntax.LiteralExpression(1)
                )
            );
        }

        [Test]
        public void GotoDefault()
        {
            Test(
@"goto default;
",
                Syntax.GotoStatement(
                    CaseOrDefault.Default
                )
            );
        }
    }
}
