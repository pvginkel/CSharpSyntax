using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class AnonymousObjectCreationExpressionFixture : TestBase
    {
        [Test]
        public void SingleWithName()
        {
            Test(
@"new
{
    Name = 7
}",
                Syntax.AnonymousObjectCreationExpression(
                    Syntax.AnonymousObjectMemberDeclarator(
                        Syntax.NameEquals("Name"),
                        Syntax.LiteralExpression(7)
                    )
                )
            );
        }

        [Test]
        public void SingleWithoutName()
        {
            Test(
@"new
{
    7
}",
                new AnonymousObjectCreationExpressionSyntax
                {
                    Initializers =
                    {
                        new AnonymousObjectMemberDeclaratorSyntax
                        {
                            Expression = Syntax.LiteralExpression(7)
                        }
                    }
                }
            );
        }

        [Test]
        public void MultipleWithName()
        {
            Test(
@"new
{
    Name = 7, Name = 7
}",
                new AnonymousObjectCreationExpressionSyntax
                {
                    Initializers =
                    {
                        new AnonymousObjectMemberDeclaratorSyntax
                        {
                            NameEquals = new NameEqualsSyntax
                            {
                                Name = new IdentifierNameSyntax { Identifier = "Name" }
                            },
                            Expression = Syntax.LiteralExpression(7)
                        },
                        new AnonymousObjectMemberDeclaratorSyntax
                        {
                            NameEquals = new NameEqualsSyntax
                            {
                                Name = new IdentifierNameSyntax { Identifier = "Name" }
                            },
                            Expression = Syntax.LiteralExpression(7)
                        }
                    }
                }
            );
        }

        [Test]
        public void MultipleWithoutName()
        {
            Test(
@"new
{
    7, 7
}",
                new AnonymousObjectCreationExpressionSyntax
                {
                    Initializers =
                    {
                        new AnonymousObjectMemberDeclaratorSyntax
                        {
                            Expression = Syntax.LiteralExpression(7)
                        },
                        new AnonymousObjectMemberDeclaratorSyntax
                        {
                            Expression = Syntax.LiteralExpression(7)
                        }
                    }
                }
            );
        }
    }
}
