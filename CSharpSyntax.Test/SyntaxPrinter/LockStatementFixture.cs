using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class LockStatementFixture : TestBase
    {
        [Test]
        public void Simple()
        {
            Test(
@"lock (1)
{
}
",
                Syntax.LockStatement(
                    Syntax.LiteralExpression(1),
                    Syntax.Block()
                )
            );
        }
    }
}
