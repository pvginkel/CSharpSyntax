using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class ReturnStatementFixture : TestBase
    {
        [Test]
        public void SimpleReturn()
        {
            Test(
@"return;
",
                Syntax.ReturnStatement()
            );
        }

        [Test]
        public void WithExpression()
        {
            Test(
@"return 1;
",
                Syntax.ReturnStatement(Syntax.LiteralExpression(1))
            );
        }
    }
}
