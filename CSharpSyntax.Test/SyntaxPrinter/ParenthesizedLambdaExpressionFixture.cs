using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class ParenthesizedLambdaExpressionFixture : TestBase
    {
        [Test]
        public void SingleParameterWithoutType()
        {
            Test(
@"(p) =>
{
}",
                Syntax.ParenthesizedLambdaExpression(
                    body: Syntax.Block(),
                    parameterList: Syntax.ParameterList(
                        Syntax.Parameter("p")
                    )
                )
            );
        }

        [Test]
        public void MultipleParameters()
        {
            Test(
@"(p1, p2) =>
{
}",
                Syntax.ParenthesizedLambdaExpression(
                    body: Syntax.Block(),
                    parameterList: Syntax.ParameterList(
                        Syntax.Parameter("p1"),
                        Syntax.Parameter("p2")
                    )
                )
            );
        }

        [Test]
        public void WithType()
        {
            Test(
@"(int p) =>
{
}",
                Syntax.ParenthesizedLambdaExpression(
                    body: Syntax.Block(),
                    parameterList: Syntax.ParameterList(
                        Syntax.Parameter(
                            identifier: "p",
                            type: Syntax.ParseName("int")
                        )
                    )
                )
            );
        }

        [Test]
        public void WithZeroParameters()
        {
            Test(
@"() =>
{
}",
                Syntax.ParenthesizedLambdaExpression(
                    body: Syntax.Block(),
                    parameterList: Syntax.ParameterList()
                )
            );
        }
    }
}
