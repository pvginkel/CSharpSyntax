using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class ThrowStatementFixture : TestBase
    {
        [Test]
        public void WithoutExpression()
        {
            Test(
@"// Before 1
throw;
// After 1
",
                Syntax.ThrowStatement()
            );
        }

        [Test]
        public void WithExpression()
        {
            Test(
@"// Before 1
throw 1;
// After 1
",
                Syntax.ThrowStatement(Syntax.LiteralExpression(1))
            );
        }
    }
}
