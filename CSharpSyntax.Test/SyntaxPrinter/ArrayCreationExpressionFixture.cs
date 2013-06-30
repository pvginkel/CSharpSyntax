using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class ArrayCreationExpressionFixture : TestBase
    {
        [Test]
        public void WithoutInitializer()
        {
            Test(
@"new int[]",
                new ArrayCreationExpressionSyntax
                {
                    Type = (ArrayTypeSyntax)Syntax.ParseName("int[]")
                }
            );
        }

        [Test]
        public void WithoutInitializerWithSize()
        {
            var arrayType = (ArrayTypeSyntax)Syntax.ParseName("int[]");

            arrayType.RankSpecifiers[0].Sizes[0] = Syntax.LiteralExpression(7);

            Test(
@"new int[7]",
                new ArrayCreationExpressionSyntax
                {
                    Type = arrayType
                }
            );
        }

        [Test]
        public void WithInitializer()
        {
            Test(
@"new int[]
{
    1
}",
                new ArrayCreationExpressionSyntax
                {
                    Type = (ArrayTypeSyntax)Syntax.ParseName("int[]"),
                    Initializer = new InitializerExpressionSyntax
                    {
                        Expressions =
                        {
                            new LiteralExpressionSyntax { Value = 1 }
                        }
                    }
                }
            );
        }

        [Test]
        public void WithMultipleInitializers()
        {
            Test(
@"new int[]
{
    1, 2, 3
}",
                new ArrayCreationExpressionSyntax
                {
                    Type = (ArrayTypeSyntax)Syntax.ParseName("int[]"),
                    Initializer = new InitializerExpressionSyntax
                    {
                        Expressions =
                        {
                            new LiteralExpressionSyntax { Value = 1 },
                            new LiteralExpressionSyntax { Value = 2 },
                            new LiteralExpressionSyntax { Value = 3 }
                        }
                    }
                }
            );
        }
    }
}
