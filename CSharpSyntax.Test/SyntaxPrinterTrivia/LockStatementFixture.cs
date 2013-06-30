using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class LockStatementFixture : TestBase
    {
        [Test]
        public void Simple()
        {
            Test(
@"// Before 1
lock (1)
{
}
// After 1
",
                Syntax.LockStatement(
                    Syntax.LiteralExpression(1),
                    Syntax.Block()
                )
            );
        }
    }
}
