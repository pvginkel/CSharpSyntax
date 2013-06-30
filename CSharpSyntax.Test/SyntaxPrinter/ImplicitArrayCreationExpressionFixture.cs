using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class ImplicitArrayCreationExpressionFixture : TestBase
    {
        [Test]
        public void Simple()
        {
            Test(
@"new[]
{
    1
}",
                Syntax.ImplicitArrayCreationExpression(
                    Syntax.InitializerExpression(
                        Syntax.LiteralExpression(1)
                    )
                )
            );
        }

        [Test]
        public void WithCommas()
        {
            Test(
@"new[, ]
{
    1
}",
                Syntax.ImplicitArrayCreationExpression(
                    1,
                    Syntax.InitializerExpression(
                        Syntax.LiteralExpression(1)
                    )
                )
            );
        }
    }
}
