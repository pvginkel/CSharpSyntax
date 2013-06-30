using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class ForEachStatementFixture : TestBase
    {
        [Test]
        public void Simple()
        {
            Test(
@"foreach (var i in 1)
{
}
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
