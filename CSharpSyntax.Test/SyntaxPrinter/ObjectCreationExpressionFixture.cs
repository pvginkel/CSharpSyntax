using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class ObjectCreationExpressionFixture : TestBase
    {
        [Test]
        public void Simple()
        {
            Test(
@"new Class()",
                Syntax.ObjectCreationExpression(
                    Syntax.ParseName("Class"),
                    Syntax.ArgumentList()
                )
            );
        }

        [Test]
        public void WithArguments()
        {
            Test(
@"new Class(1)",
                Syntax.ObjectCreationExpression(
                    Syntax.ParseName("Class"),
                    Syntax.ArgumentList(
                        Syntax.Argument(Syntax.LiteralExpression(1))
                    )
                )
            );
        }

        [Test]
        public void SimpleWithInitializer()
        {
            Test(
@"new Class
{
}",
                Syntax.ObjectCreationExpression(
                    Syntax.ParseName("Class"),
                    initializer: Syntax.InitializerExpression()
                )
            );
        }

        [Test]
        public void WithEmptyArgumentsWithInitializer()
        {
            Test(
@"new Class()
{
}",
                Syntax.ObjectCreationExpression(
                    Syntax.ParseName("Class"),
                    Syntax.ArgumentList(),
                    Syntax.InitializerExpression()
                )
            );
        }

        [Test]
        public void WithArgumentsAndInitializer()
        {
            Test(
@"new Class(1)
{
}",
                Syntax.ObjectCreationExpression(
                    Syntax.ParseName("Class"),
                    Syntax.ArgumentList(
                        Syntax.Argument(Syntax.LiteralExpression(1))
                    ),
                    Syntax.InitializerExpression()
                )
            );
        }

        [Test]
        public void WithInitializer()
        {
            Test(
@"new Class
{
    Name = 1
}",
                Syntax.ObjectCreationExpression(
                    Syntax.ParseName("Class"),
                    initializer: Syntax.InitializerExpression(
                        Syntax.BinaryExpression(
                            BinaryOperator.Equals,
                            Syntax.ParseName("Name"),
                            Syntax.LiteralExpression(1)
                        )
                    )
                )
            );
        }
    }
}
