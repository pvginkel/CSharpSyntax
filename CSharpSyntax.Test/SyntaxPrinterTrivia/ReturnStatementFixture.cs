using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class ReturnStatementFixture : TestBase
    {
        [Test]
        public void SimpleReturn()
        {
            Test(
@"// Before 1
return;
// After 1
",
                Syntax.ReturnStatement()
            );
        }

        [Test]
        public void WithExpression()
        {
            Test(
@"// Before 1
return 1;
// After 1
",
                Syntax.ReturnStatement(Syntax.LiteralExpression(1))
            );
        }
    }
}
