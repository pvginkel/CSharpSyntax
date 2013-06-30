using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class YieldStatementFixture : TestBase
    {
        [Test]
        public void Break()
        {
            Test(
@"yield break;
",
                Syntax.YieldStatement(ReturnOrBreak.Break)
            );
        }

        [Test]
        public void Return()
        {
            Test(
@"yield return 1;
",
                Syntax.YieldStatement(ReturnOrBreak.Return, Syntax.LiteralExpression(1))
            );
        }
    }
}
