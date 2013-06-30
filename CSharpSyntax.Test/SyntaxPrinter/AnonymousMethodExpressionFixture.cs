using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class AnonymousMethodExpressionFixture : TestBase
    {
        [Test]
        public void SingleParameter()
        {
            Test(
@"delegate()
{
}",
                Syntax.AnonymousMethodExpression(
                    block: Syntax.Block(),
                    parameterList: Syntax.ParameterList()
                )
            );
        }

        [Test]
        public void MultipleParameters()
        {
            Test(
@"delegate(int p1, int p2)
{
}",
                Syntax.AnonymousMethodExpression(
                    block: Syntax.Block(),
                    parameterList: Syntax.ParameterList(
                        Syntax.Parameter(
                            identifier: "p1",
                            type: Syntax.ParseName("int")
                        ),
                        Syntax.Parameter(
                            identifier: "p2",
                            type: Syntax.ParseName("int")
                        )
                    )
                )
            );
        }

        [Test]
        public void WithType()
        {
            Test(
@"delegate(int p)
{
}",
                Syntax.AnonymousMethodExpression(
                    block: Syntax.Block(),
                    parameterList: Syntax.ParameterList(
                        Syntax.Parameter(
                            identifier: "p",
                            type: Syntax.ParseName("int")
                        )
                    )
                )
            );
        }
    }
}
