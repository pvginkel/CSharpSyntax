using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class ThrowStatementFixture : TestBase
    {
        [Test]
        public void WithoutExpression()
        {
            Test(
@"throw;
",
                Syntax.ThrowStatement()
            );
        }

        [Test]
        public void WithExpression()
        {
            Test(
@"throw 1;
",
                Syntax.ThrowStatement(Syntax.LiteralExpression(1))
            );
        }
    }
}
