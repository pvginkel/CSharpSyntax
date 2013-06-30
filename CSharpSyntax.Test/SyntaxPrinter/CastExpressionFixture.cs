using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class CastExpressionFixture : TestBase
    {
        [Test]
        public void SimpleCast()
        {
            Test(
@"(int)7",
                new CastExpressionSyntax
                {
                    Type = Syntax.ParseName("int"),
                    Expression = Syntax.LiteralExpression(7)
                }
            );
        }
    }
}
