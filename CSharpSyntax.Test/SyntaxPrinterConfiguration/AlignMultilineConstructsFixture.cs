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
    public class AlignMultiLineConstructsFixture : TestBase
    {
        [TestCase(
            true,
@"void Method(string parameter,
            string parameter)
{
}
"
        )]
        [TestCase(
            false,
@"void Method(string parameter,
    string parameter)
{
}
"
        )]
        public void MethodParameters(bool align, string expected)
        {
            Test(
                expected,
                Syntax.MethodDeclaration(
                    Syntax.ParseName("void"),
                    "Method",
                    Syntax.ParameterList(
                        Syntax.Parameter(
                            type: Syntax.ParseName("string"),
                            identifier: "parameter"
                        ),
                        Syntax.Parameter(
                            type: Syntax.ParseName("string"),
                            identifier: "parameter"
                        )
                    ),
                    Syntax.Block()
                ),
                p =>
                {
                    p.Other.AlignMultiLineConstructs.MethodParameters = align;
                    p.LineBreaksAndWrapping.LineWrapping.RightMargin = 35;
                }
            );
        }


        [TestCase(
            true,
@"Method(""parameter"", ""parameter"",
       ""parameter"", ""parameter"");
"
        )]
        [TestCase(
            false,
@"Method(""parameter"", ""parameter"",
    ""parameter"", ""parameter"");
"
        )]
        public void WrapInvocationArguments(bool align, string expected)
        {
            Test(
                expected,
                Syntax.ExpressionStatement(
                    Syntax.InvocationExpression(
                        Syntax.ParseName("Method"),
                        Syntax.ArgumentList(
                            Syntax.Argument(Syntax.LiteralExpression("parameter")),
                            Syntax.Argument(Syntax.LiteralExpression("parameter")),
                            Syntax.Argument(Syntax.LiteralExpression("parameter")),
                            Syntax.Argument(Syntax.LiteralExpression("parameter"))
                        )
                    )
                ),
                p =>
                {
                    p.Other.AlignMultiLineConstructs.CallArguments = align;
                    p.LineBreaksAndWrapping.LineWrapping.RightMargin = 40;
                }
            );
        }

        [TestCase(
            true,
@"class Class : Interface,
              Interface
{
}
"
        )]
        [TestCase(
            false,
@"class Class : Interface,
    Interface
{
}
"
        )]
        public void ListOfBaseClassesAndInterfaces(bool align, string expected)
        {
            Test(
                expected,
                Syntax.ClassDeclaration(
                    identifier: "Class",
                    baseList: Syntax.BaseList(
                        Syntax.ParseName("Interface"),
                        Syntax.ParseName("Interface")
                    )
                ),
                p =>
                {
                    p.Other.AlignMultiLineConstructs.ListOfBaseClassesAndInterfaces = align;
                    p.LineBreaksAndWrapping.LineWrapping.RightMargin = 30;
                }
            );
        }

        [TestCase(
            false,
            false,
@"var i = 1 + 1
    + 1;
"
)]
        [TestCase(
            true,
            false,
@"var i = 1 + 1
        + 1;
"
)]
        [TestCase(
            false,
            true,
@"var i = 1 + 1
          + 1;
"
)]
        [TestCase(
            true,
            true,
@"var i = 1 + 1
          + 1;
"
)]
        public void Expression(bool alignExpression, bool alignBinary, string expected)
        {
            Test(
                expected,
                Syntax.LocalDeclarationStatement(
                    Syntax.VariableDeclaration(
                        Syntax.ParseName("var"),
                        new[]
                        {
                            Syntax.VariableDeclarator(
                                "i",
                                null,
                                Syntax.EqualsValueClause(
                                    Syntax.BinaryExpression(
                                        BinaryOperator.Plus,
                                        Syntax.BinaryExpression(
                                            BinaryOperator.Plus,
                                            Syntax.LiteralExpression(1),
                                            Syntax.LiteralExpression(1)
                                        ),
                                        Syntax.LiteralExpression(1)
                                    )
                                )
                            )
                        }
                    )
                ),
                p =>
                {
                    p.Other.AlignMultiLineConstructs.Expression = alignExpression;
                    p.Other.AlignMultiLineConstructs.ChainedBinaryExpressions = alignBinary;
                    p.LineBreaksAndWrapping.LineWrapping.RightMargin = 14;
                }
            );
        }

        [TestCase(
            true,
@"for (1;
     1;
     1)
{
}
"
)]
        [TestCase(
            false,
@"for (1;
    1;
    1)
{
}
"
)]
        public void ForStatementHeader(bool align, string expected)
        {
            Test(
                expected,
                Syntax.ForStatement(
                    null,
                    new[] { Syntax.LiteralExpression(1) },
                    Syntax.LiteralExpression(1),
                    new[] { Syntax.LiteralExpression(1) },
                    Syntax.Block()
                ),
                p =>
                {
                    p.Other.AlignMultiLineConstructs.ForStatementHeader = align;
                    p.LineBreaksAndWrapping.LineWrapping.WrapForStatementHeader = WrapStyle.ChopAlways;
                }
            );
        }

        [TestCase(
           true,
@"var i = 0,
    j = 0;
"
        )]
        [TestCase(
           false,
@"var i = 0,
  j = 0;
"
        )]
        public void MultipleDeclarations(bool align, string expected)
        {
            Test(
                expected,
                Syntax.LocalDeclarationStatement(
                    Syntax.VariableDeclaration(
                        Syntax.ParseName("var"),
                        new[]
                        {
                            Syntax.VariableDeclarator(
                                "i",
                                null,
                                Syntax.EqualsValueClause(Syntax.LiteralExpression(0))
                            ),
                            Syntax.VariableDeclarator(
                                "j",
                                null,
                                Syntax.EqualsValueClause(Syntax.LiteralExpression(0))
                            )
                        }
                    )
                ),
                p =>
                {
                    p.Other.AlignMultiLineConstructs.MultipleDeclarations = align;
                    p.LineBreaksAndWrapping.LineWrapping.WrapMultipleDeclarations = WrapStyle.ChopAlways;
                    p.Indentation = 2;
                }
            );
        }

        [TestCase(
           false,
@"class Class where T : class
    where T : class
{
}
"
        )]
        [TestCase(
           true,
@"class Class where T : class
            where T : class
{
}
"
        )]
        public void TypeParameterConstraints(bool align, string expected)
        {
            Test(
                expected,
                Syntax.ClassDeclaration(
                    identifier: "Class",
                    constraintClauses: new[]
                    {
                        Syntax.TypeParameterConstraintClause(
                            (IdentifierNameSyntax)Syntax.ParseName("T"),
                            new[]
                            {
                                Syntax.ClassOrStructConstraint()
                            }
                        ),
                        Syntax.TypeParameterConstraintClause(
                            (IdentifierNameSyntax)Syntax.ParseName("T"),
                            new[]
                            {
                                Syntax.ClassOrStructConstraint()
                            }
                        )
                    }
                ),
                p =>
                {
                    p.Other.AlignMultiLineConstructs.TypeParameterConstraints = align;
                    p.LineBreaksAndWrapping.LineWrapping.WrapMultipleTypeParameterConstraints = WrapStyle.ChopAlways;
                    p.LineBreaksAndWrapping.Other.PlaceTypeConstraintsOnSameLine = true;
                }
            );
        }

        [TestCase(
            true,
@"var i = from i in j
        from i in j
        join k in l on i equals k into m
        select i;
"
        )]
        [TestCase(
            false,
@"var i = from i in j
    from i in j
    join k in l on i equals k into m
    select i;
"
        )]
        public void PlaceLinqExpressionOnSingleLine(bool align, string expected)
        {
            Test(
                expected,
                Syntax.LocalDeclarationStatement(
                    Syntax.VariableDeclaration(
                        Syntax.ParseName("var"),
                        new[]
                        {
                            Syntax.VariableDeclarator(
                                "i",
                                null,
                                Syntax.EqualsValueClause(
                                    Syntax.QueryExpression(
                                        Syntax.FromClause("i", Syntax.ParseName("j")),
                                        Syntax.QueryBody(
                                            new QueryClauseSyntax[]
                                            {
                                                Syntax.FromClause("i", Syntax.ParseName("j")),
                                                Syntax.JoinClause(
                                                    "k",
                                                    Syntax.ParseName("l"),
                                                    Syntax.ParseName("i"),
                                                    Syntax.ParseName("k"),
                                                    Syntax.JoinIntoClause("m")
                                                )
                                            },
                                            Syntax.SelectClause(Syntax.ParseName("i"))
                                        )
                                    )
                                )
                            )
                        }
                    )
                ),
                p =>
                    {
                        p.LineBreaksAndWrapping.Other.PlaceLinqExpressionOnSingleLine = false;
                        p.Other.AlignMultiLineConstructs.LinqQuery = align;
                    }
            );
        }

        [Ignore("Not yet implemented; difficult with the model we use")]
        public void ChainedMethodCalls()
        {
        }

        [Ignore("Not yet implemented; difficult with the model we use")]
        public void ArrayObjectCollectionInitializer()
        {
        }

        [Ignore("Not yet implemented; difficult with the model we use")]
        public void AnonymousMethodBody()
        {
        }

        [Ignore("Not yet implemented")]
        public void TypeParametersList()
        {
        }

        [Ignore("Does not apply because we don't preserve existing newlines")]
        public void FirstCallArgumentsByParen()
        {
        }

        protected override void Test(string expected, IEnumerable<SyntaxNode> nodes, Action<Printer.Configuration.SyntaxPrinterConfiguration> configure)
        {
            base.Test(expected, nodes, p =>
            {
                if (configure != null)
                    configure(p);

                p.LineBreaksAndWrapping.LineWrapping.WrapLongLines = true;
            });
        }
    }
}
