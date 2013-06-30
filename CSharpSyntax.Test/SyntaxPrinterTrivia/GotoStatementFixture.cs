using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class GotoStatementFixture : TestBase
    {
        [Test]
        public void Goto()
        {
            Test(
@"// Before 1
goto 1;
// After 1
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
@"// Before 1
goto case 1;
// After 1
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
@"// Before 1
goto default;
// After 1
",
                Syntax.GotoStatement(
                    CaseOrDefault.Default
                )
            );
        }
    }
}
