using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class AsyncFixture : TestBase
    {
        [Test]
        public void Await()
        {
            Test(
@"await 1",
                Syntax.AwaitExpression(Syntax.LiteralExpression(1))
            );
        }

        [Test]
        public void AnonymousMethod()
        {
            Test(
@"async delegate()
{
}",
                Syntax.AnonymousMethodExpression(
                    Modifiers.Async,
                    block: Syntax.Block(),
                    parameterList: Syntax.ParameterList()
                )
            );
        }

        [Test]
        public void SimpleLambda()
        {
            Test(
@"async p =>
{
}",
                Syntax.SimpleLambdaExpression(
                    Modifiers.Async,
                    body: Syntax.Block(),
                    parameter: Syntax.Parameter("p")
                )
            );
        }

        [Test]
        public void ParenthesizedLambda()
        {
            Test(
@"async (p) =>
{
}",
                Syntax.ParenthesizedLambdaExpression(
                    Modifiers.Async,
                    body: Syntax.Block(),
                    parameterList: Syntax.ParameterList(
                        Syntax.Parameter("p")
                    )
                )
            );
        }

        [Test]
        public void Method()
        {
            Test(
@"async void Method()
{
}
",
                Syntax.MethodDeclaration(
                    modifiers: Modifiers.Async,
                    identifier: "Method",
                    returnType: Syntax.ParseName("void"),
                    parameterList: Syntax.ParameterList(),
                    body: Syntax.Block()
                )
            );
        }
    }
}
