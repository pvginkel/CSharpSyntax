using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class ForEachStatementFixture : TestBase
    {
        [Test]
        public void Simple()
        {
            Test(
@"// Before 1
foreach (var i in 1)
{
}
// After 1
",
                Syntax.ForEachStatement(
                    Syntax.ParseName("var"),
                    "i",
                    Syntax.LiteralExpression(1),
                    Syntax.Block()
                )
            );
        }
    }
}
