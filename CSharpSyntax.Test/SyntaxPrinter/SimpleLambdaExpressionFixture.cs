using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class SimpleLambdaExpressionFixture : TestBase
    {
        [Test]
        public void SingleParameterWithoutType()
        {
            Test(
@"p =>
{
}",
                Syntax.SimpleLambdaExpression(
                    body: Syntax.Block(),
                    parameter: Syntax.Parameter("p")
                )
            );
        }

        [Test]
        [ExpectedException]
        public void WithType()
        {
            Test(
@"(int p) =>
{
}",
                Syntax.SimpleLambdaExpression(
                    body: Syntax.Block(),
                    parameter: Syntax.Parameter(
                        identifier: "p",
                        type: Syntax.ParseName("int")
                    )
                )
            );
        }
    }
}
