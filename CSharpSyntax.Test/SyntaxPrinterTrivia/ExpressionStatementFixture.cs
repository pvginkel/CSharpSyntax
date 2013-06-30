using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class ExpressionStatementFixture : TestBase
    {
        [Test]
        public void SimpleExpression()
        {
            Test(
@"// Before 1
1;
// After 1
",
                Syntax.ExpressionStatement(Syntax.LiteralExpression(1))
            );
        }
    }
}
