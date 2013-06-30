using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class YieldStatementFixture : TestBase
    {
        [Test]
        public void Break()
        {
            Test(
@"// Before 1
yield break;
// After 1
",
                Syntax.YieldStatement(ReturnOrBreak.Break)
            );
        }

        [Test]
        public void Return()
        {
            Test(
@"// Before 1
yield return 1;
// After 1
",
                Syntax.YieldStatement(ReturnOrBreak.Return, Syntax.LiteralExpression(1))
            );
        }
    }
}
