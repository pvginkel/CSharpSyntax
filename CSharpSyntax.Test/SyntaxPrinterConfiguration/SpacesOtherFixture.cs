using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpSyntax.Printer.Configuration;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterConfiguration
{
    [TestFixture]
    public class SpacesOtherFixture : TestBase
    {
        [TestCase(
            true,
@"(int) 10"
        )]
        [TestCase(
            false,
@"(int)10"
        )]
        public void AfterTypeCastParentheses(bool space, string expected)
        {
            Test(
                expected,
                Syntax.CastExpression(
                    Syntax.ParseName("int"),
                    Syntax.LiteralExpression(10)
                ),
                p => p.Spaces.Other.AfterTypeCastParentheses = space
            );
        }

        [TestCase(true, true, @"Method(1 , 2)")]
        [TestCase(false, true, @"Method(1, 2)")]
        [TestCase(true, false, @"Method(1 ,2)")]
        [TestCase(false, false, @"Method(1,2)")]
        public void BeforeAfterComma(bool before, bool after, string expected)
        {
            Test(
                expected,
                Syntax.InvocationExpression(
                    Syntax.ParseName("Method"),
                    Syntax.ArgumentList(
                        Syntax.Argument(Syntax.LiteralExpression(1)),
                        Syntax.Argument(Syntax.LiteralExpression(2))
                    )
                ),
                p =>
                {
                    p.Spaces.Other.BeforeComma = before;
                    p.Spaces.Other.AfterComma = after;
                }
            );
        }

        [TestCase(
            true,
            true,
@"
{
    for (int i = 0 ; i < 10 ; i++)
    {
    }
}
"
        )]
        [TestCase(
            false,
            true,
@"
{
    for (int i = 0; i < 10; i++)
    {
    }
}
"
        )]
        [TestCase(
            true,
            false,
@"
{
    for (int i = 0 ;i < 10 ;i++)
    {
    }
}
"
        )]
        [TestCase(
            false,
            false,
@"
{
    for (int i = 0;i < 10;i++)
    {
    }
}
"
        )]
        public void BeforeAfterForSemiColon(bool before, bool after, string expected)
        {
            Test(
                expected,
                Syntax.Block(
                    Syntax.ForStatement(
                        Syntax.VariableDeclaration(
                            Syntax.ParseName("int"),
                            new[]
                            {
                                Syntax.VariableDeclarator(
                                    "i",
                                    initializer: Syntax.EqualsValueClause(
                                        Syntax.LiteralExpression(0)
                                    )
                                )
                            }
                        ),
                        condition: Syntax.BinaryExpression(
                            BinaryOperator.LessThan,
                            Syntax.ParseName("i"),
                            Syntax.LiteralExpression(10)
                        ),
                        incrementors: new[]
                        {
                            Syntax.PostfixUnaryExpression(
                                PostfixUnaryOperator.PlusPlus,
                                Syntax.ParseName("i")
                            )
                        },
                        statement: Syntax.Block()
                    )
                ),
                p =>
                {
                    p.Spaces.Other.BeforeForSemicolon = before;
                    p.Spaces.Other.AfterForSemicolon = after;
                }
            );
        }

        [TestCase(
            true,
            true,
@"[assembly : Attribute]"
        )]
        [TestCase(
            false,
            true,
@"[assembly: Attribute]"
        )]
        [TestCase(
            true,
            false,
@"[assembly :Attribute]"
        )]
        [TestCase(
            false,
            false,
@"[assembly:Attribute]"
        )]
        public void BeforeAfterColonInAttribute(bool before, bool after, string expected)
        {
            Test(
                expected,
                Syntax.AttributeList(
                    AttributeTarget.Assembly,
                    new[]
                    {
                        Syntax.Attribute(
                            (NameSyntax)Syntax.ParseName("Attribute")
                        )
                    }
                ),
                p =>
                {
                    p.Spaces.Other.BeforeColonInAttribute = before;
                    p.Spaces.Other.AfterColonInAttribute = after;
                }
            );
        }

        [TestCase(
            true,
            true,
@"class Class : BaseClass
{
}
"
        )]
        [TestCase(
            false,
            true,
@"class Class: BaseClass
{
}
"
        )]
        [TestCase(
            true,
            false,
@"class Class :BaseClass
{
}
"
        )]
        [TestCase(
            false,
            false,
@"class Class:BaseClass
{
}
"
        )]
        public void BeforeAfterBaseTypesListColon(bool before, bool after, string expected)
        {
            Test(
                expected,
                Syntax.ClassDeclaration(
                    identifier: "Class",
                    baseList: Syntax.BaseList(
                        Syntax.ParseName("BaseClass")
                    )
                ),
                p =>
                {
                    p.Spaces.Other.BeforeBaseTypesListColon = before;
                    p.Spaces.Other.AfterBaseTypesListColon = after;
                }
            );
        }

        [TestCase(true, "System . Console")]
        [TestCase(false, "System.Console")]
        public void AroundDot(bool space, string expected)
        {
            Test(
                expected,
                Syntax.ParseName("System.Console"),
                p => p.Spaces.Other.AroundDot = space
            );
        }

        [TestCase(true, "p => 1")]
        [TestCase(false, "p=>1")]
        public void AroundLambdaArrow(bool space, string expected)
        {
            Test(
                expected,
                Syntax.SimpleLambdaExpression(
                    Syntax.Parameter("p"),
                    Syntax.LiteralExpression(1)
                ),
                p => p.Spaces.Other.AroundLambdaArrow = space
            );
        }

        [TestCase(
            true,
@"p =>
{
}")]
        [TestCase(
            false,
@"p=>
{
}")]
        public void AroundLambdaArrowWithBlock(bool space, string expected)
        {
            Test(
                expected,
                Syntax.SimpleLambdaExpression(
                    Syntax.Parameter("p"),
                    Syntax.Block()
                ),
                p => p.Spaces.Other.AroundLambdaArrow = space
            );
        }

        [Ignore("Not yet implemented")]
        public void BeforeSingleLineAccessorBlock()
        {
        }

        [Ignore("Not yet implemented")]
        public void WithinSingleLineAccessor()
        {
        }

        [Ignore("Not yet implemented")]
        public void BetweenAccessorsInSingleLinePropertyEvent()
        {
        }

        [TestCase(
            true,
@"void Method() { }
"
        )]
        [TestCase(
            false,
@"void Method() {}
"
        )]
        public void SpacesBetweenEmptyBraces(bool space, string expected)
        {
            Test(
                expected,
                Syntax.MethodDeclaration(
                    Syntax.ParseName("void"),
                    "Method",
                    Syntax.ParameterList(),
                    Syntax.Block()
                ),
                p =>
                {
                    p.Spaces.Other.SpacesBetweenEmptyBraces = space;
                    p.BracesLayout.EmptyBraceFormatting = EmptyBraceFormatting.TogetherOnSameLine;
                }
            );
        }

        [Ignore("Not yet implemented")]
        public void WithinSingleLineMethod()
        {
        }

        [Ignore("Not yet implemented")]
        public void WithinSingleLineAnonymousMethod()
        {
        }

        [TestCase(
            true,
@"[ Attribute ]"
        )]
        [TestCase(
            false,
@"[Attribute]"
        )]
        public void WithinAttributeBrackets(bool space, string expected)
        {
            Test(
                expected,
                Syntax.AttributeList(
                    Syntax.Attribute(
                        (NameSyntax)Syntax.ParseName("Attribute")
                    )
                ),
                p => p.Spaces.Other.WithinAttributeBrackets = space
            );
        }

        [TestCase(
            true,
            true,
            true,
@"int [ ] [ , ] i = new int [ 0 ] [ 1, 2 ];
"
        )]
        [TestCase(
            false,
            true,
            true,
@"int[ ][ , ] i = new int[ 0 ][ 1, 2 ];
"
        )]
        [TestCase(
            true,
            false,
            true,
@"int [ ] [,] i = new int [0] [1, 2];
"
        )]
        [TestCase(
            true,
            true,
            false,
@"int [] [ , ] i = new int [ 0 ] [ 1, 2 ];
"
        )]
        public void ArrayRankBrackets(bool before, bool within, bool withinEmpty, string expected)
        {
            Test(
                expected,
                Syntax.LocalDeclarationStatement(
                    Syntax.VariableDeclaration(
                        Syntax.ParseName("int[][,]"),
                        new[]
                        {
                            Syntax.VariableDeclarator(
                                "i",
                                initializer: Syntax.EqualsValueClause(
                                    Syntax.ArrayCreationExpression(
                                        Syntax.ArrayType(
                                            Syntax.ParseName("int"),
                                            new[]
                                            {
                                                Syntax.ArrayRankSpecifier(
                                                    Syntax.LiteralExpression(0)
                                                ),
                                                Syntax.ArrayRankSpecifier(
                                                    Syntax.LiteralExpression(1),
                                                    Syntax.LiteralExpression(2)
                                                )
                                            }
                                        )
                                    )
                                )
                            )
                        }
                    )
                ),
                p =>
                {
                    p.Spaces.Other.BeforeArrayRankBrackets = before;
                    p.Spaces.Other.WithinArrayRankBrackets = within;
                    p.Spaces.Other.WithinArrayRankEmptyBrackets = withinEmpty;
                }
            );
        }

        [Ignore("Not yet implemented")]
        public void WithinSingleLineInitializerBraces()
        {
        }

        [TestCase(
            true,
@"i ;
"
        )]
        [TestCase(
            false,
@"i;
"
        )]
        public void BeforeSemicolon(bool space, string expected)
        {
            Test(
                expected,
                Syntax.ExpressionStatement(
                    Syntax.ParseName("i")
                ),
                p => p.Spaces.Other.BeforeSemicolon = space
            );
        }

        [TestCase(true, "case 1 :")]
        [TestCase(false, "case 1:")]
        public void BeforeColonInCaseStatement(bool space, string expected)
        {
            Test(
                expected,
                Syntax.SwitchLabel(CaseOrDefault.Case, Syntax.LiteralExpression(1)),
                p => p.Spaces.Other.BeforeColonInCaseStatement = space
            );
        }

        [TestCase(true, "int ?")]
        [TestCase(false, "int?")]
        public void BeforeNullableMark(bool space, string expected)
        {
            Test(
                expected,
                Syntax.ParseName("int?"),
                p => p.Spaces.Other.BeforeNullableMark = space
            );
        }

        [TestCase(
            true,
            true,
@"class Class<T>
    where T : int
{
}
")]
        [TestCase(
            false,
            true,
@"class Class<T>
    where T: int
{
}
")]
        [TestCase(
            true,
            false,
@"class Class<T>
    where T :int
{
}
")]
        public void BeforeAfterTypeParameterConstraintColon(bool before, bool after, string expected)
        {
            Test(
                expected,
                Syntax.ClassDeclaration(
                    identifier: "Class",
                    typeParameterList: Syntax.TypeParameterList(
                        Syntax.TypeParameter("T")
                    ),
                    constraintClauses: new[]
                    {
                        Syntax.TypeParameterConstraintClause(
                            (IdentifierNameSyntax)Syntax.ParseName("T"),
                            new[] { Syntax.TypeConstraint(Syntax.ParseName("int")) }
                        )
                    }
                ),
                p =>
                {
                    p.Spaces.Other.BeforeTypeParameterConstraintColon = before;
                    p.Spaces.Other.AfterTypeParameterConstraintColon = after;
                }
            );
        }

        [TestCase(
            true,
@"using Foo = System;
")]
        [TestCase(
            false,
@"using Foo=System;
")]
        public void AroundEqualsInNamespaceAliasDirective(bool space, string expected)
        {
            Test(
                expected,
                Syntax.UsingDirective(
                    Syntax.NameEquals("Foo"),
                    (NameSyntax)Syntax.ParseName("System")
                ),
                p => p.Spaces.Other.AroundEqualsInNamespaceAliasDirective = space
            );
        }

        [Ignore("Not yet implemented")]
        public void BeforeEndOfLineComment()
        {
        }
    }
}
