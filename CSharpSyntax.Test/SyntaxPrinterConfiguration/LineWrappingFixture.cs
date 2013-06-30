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
    public class LineWrappingFixture : TestBase
    {
        [TestCase(
            WrapStyle.SimpleWrap,
@"void Method(string parameter, string parameter, string
    parameter, string parameter)
{
}
"
        )]
        [TestCase(
            WrapStyle.ChopAlways,
@"void Method(string parameter,
    string parameter,
    string parameter,
    string parameter)
{
}
"
        )]
        [TestCase(
            WrapStyle.ChopIfLong,
@"void Method(string parameter, string parameter,
    string parameter, string parameter)
{
}
"
        )]
        public void WrapFormalParameters(WrapStyle wrapStyle, string expected)
        {
            Test(
                expected,
                Syntax.MethodDeclaration(
                    Syntax.ParseName("void"),
                    "Method",
                    Syntax.ParameterList(
                        Syntax.Parameter(type: Syntax.ParseName("string"), identifier: "parameter"),
                        Syntax.Parameter(type: Syntax.ParseName("string"), identifier: "parameter"),
                        Syntax.Parameter(type: Syntax.ParseName("string"), identifier: "parameter"),
                        Syntax.Parameter(type: Syntax.ParseName("string"), identifier: "parameter")
                    ),
                    Syntax.Block()
                ),
                p =>
                {
                    p.LineBreaksAndWrapping.LineWrapping.WrapFormalParameters = wrapStyle;
                    p.LineBreaksAndWrapping.LineWrapping.RightMargin = 60;
                }
            );
        }

        [TestCase(
            WrapStyle.SimpleWrap,
@"Method(""parameter"", ""parameter"" +
    ""parameter"", ""parameter"",
    ""parameter"");
"
        )]
        [TestCase(
            WrapStyle.ChopAlways,
@"Method(""parameter"",
    ""parameter"" + ""parameter"",
    ""parameter"",
    ""parameter"");
"
        )]
        [TestCase(
            WrapStyle.ChopIfLong,
@"Method(""parameter"",
    ""parameter"" + ""parameter"",
    ""parameter"", ""parameter"");
"
        )]
        public void WrapInvocationArguments(WrapStyle wrapStyle, string expected)
        {
            Test(
                expected,
                Syntax.ExpressionStatement(
                    Syntax.InvocationExpression(
                        Syntax.ParseName("Method"),
                        Syntax.ArgumentList(
                            Syntax.Argument(Syntax.LiteralExpression("parameter")),
                            Syntax.Argument(
                                Syntax.BinaryExpression(
                                    BinaryOperator.Plus,
                                    Syntax.LiteralExpression("parameter"),
                                    Syntax.LiteralExpression("parameter")
                                )
                            ),
                            Syntax.Argument(Syntax.LiteralExpression("parameter")),
                            Syntax.Argument(Syntax.LiteralExpression("parameter"))
                        )
                    )
                ),
                p =>
                {
                    p.LineBreaksAndWrapping.LineWrapping.WrapInvocationArguments = wrapStyle;
                    p.LineBreaksAndWrapping.LineWrapping.RightMargin = 40;
                }
            );
        }

        [TestCase(
            WrapStyle.ChopAlways,
            true,
@"Method().
    Method().
    Method().
    Method().
    Method().
    Method().
    Method().
    Method();
"
        )]
        [TestCase(
            WrapStyle.ChopIfLong,
            true,
@"Method().Method().Method().Method().
    Method().Method().Method().Method();
"
        )]
        [TestCase(
            WrapStyle.SimpleWrap,
            true,
@"Method().Method().Method().Method().
    Method().Method().Method().Method();
"
        )]
        [TestCase(
            WrapStyle.ChopAlways,
            false,
@"Method()
    .Method()
    .Method()
    .Method()
    .Method()
    .Method()
    .Method()
    .Method();
"
        )]
        [TestCase(
            WrapStyle.ChopIfLong,
            false,
@"Method().Method().Method().Method()
    .Method().Method().Method()
    .Method();
"
        )]
        [TestCase(
            WrapStyle.SimpleWrap,
            false,
@"Method().Method().Method().Method().
    Method().Method().Method().Method();
"
        )]
        public void WrapChainedMethodCalls(WrapStyle wrapStyle, bool afterDot, string expected)
        {
            Test(
                expected,
                Syntax.ExpressionStatement(
                    Syntax.InvocationExpression(
                        Syntax.MemberAccessExpression(
                            Syntax.InvocationExpression(
                                Syntax.MemberAccessExpression(
                                    Syntax.InvocationExpression(
                                        Syntax.MemberAccessExpression(
                                            Syntax.InvocationExpression(
                                                Syntax.MemberAccessExpression(
                                                    Syntax.InvocationExpression(
                                                        Syntax.MemberAccessExpression(
                                                            Syntax.InvocationExpression(
                                                                Syntax.MemberAccessExpression(
                                                                    Syntax.InvocationExpression(
                                                                        Syntax.MemberAccessExpression(
                                                                            Syntax.InvocationExpression(
                                                                                Syntax.ParseName("Method"),
                                                                                Syntax.ArgumentList()
                                                                            ),
                                                                            (SimpleNameSyntax)Syntax.ParseName("Method")
                                                                        ),
                                                                        Syntax.ArgumentList()
                                                                    ),
                                                                    (SimpleNameSyntax)Syntax.ParseName("Method")
                                                                ),
                                                                Syntax.ArgumentList()
                                                            ),
                                                            (SimpleNameSyntax)Syntax.ParseName("Method")
                                                        ),
                                                        Syntax.ArgumentList()
                                                    ),
                                                    (SimpleNameSyntax)Syntax.ParseName("Method")
                                                ),
                                                Syntax.ArgumentList()
                                            ),
                                            (SimpleNameSyntax)Syntax.ParseName("Method")
                                        ),
                                        Syntax.ArgumentList()
                                    ),
                                    (SimpleNameSyntax)Syntax.ParseName("Method")
                                ),
                                Syntax.ArgumentList()
                            ),
                            (SimpleNameSyntax)Syntax.ParseName("Method")
                        ),
                        Syntax.ArgumentList()
                    )
                ),
                p =>
                {
                    p.LineBreaksAndWrapping.LineWrapping.WrapChainedMethodCalls = wrapStyle;
                    p.LineBreaksAndWrapping.LineWrapping.PreferWrapAfterDotInMethodCalls = afterDot;
                    p.LineBreaksAndWrapping.LineWrapping.RightMargin = 40;
                }
            );
        }

        [TestCase(
           0,
@"void Method(string parameter, string parameter,
string parameter, string parameter)
{
}
"
        )]
        [TestCase(
           1,
@"void Method(string parameter, string parameter,
    string parameter, string parameter)
{
}
"
        )]
        [TestCase(
           2,
@"void Method(string parameter, string parameter,
        string parameter, string parameter)
{
}
"
        )]
        public void ContinuousLineIndentMultiplier(int indent, string expected)
        {
            Test(
                expected,
                Syntax.MethodDeclaration(
                    Syntax.ParseName("void"),
                    "Method",
                    Syntax.ParameterList(
                        Syntax.Parameter(type: Syntax.ParseName("string"), identifier: "parameter"),
                        Syntax.Parameter(type: Syntax.ParseName("string"), identifier: "parameter"),
                        Syntax.Parameter(type: Syntax.ParseName("string"), identifier: "parameter"),
                        Syntax.Parameter(type: Syntax.ParseName("string"), identifier: "parameter")
                    ),
                    Syntax.Block()
                ),
                p =>
                {
                    p.LineBreaksAndWrapping.LineWrapping.WrapFormalParameters = WrapStyle.ChopIfLong;
                    p.Other.Indentation.ContinuousLineIndentMultiplier = indent;
                    // Not yet supported.
                    //p.LineBreaksAndWrapping.LineWrapping.PreferWrapBeforeParenInDeclaration = false;
                    //p.LineBreaksAndWrapping.LineWrapping.PreferWrapAfterParenInDeclaration = false;
                    p.LineBreaksAndWrapping.LineWrapping.RightMargin = 60;
                }
            );
        }

        [TestCase(
           true,
           false,
@"if (1 ||
    1)
{
}
"
        )]
        [TestCase(
           false,
           false,
@"if (1 || 1)
{
}
"
        )]
        [TestCase(
           true,
           true,
@"if (1
    || 1)
{
}
"
        )]
        public void ForceChopCompoundConditionInIfStatement(bool force, bool before, string expected)
        {
            Test(
                expected,
                Syntax.IfStatement(
                    Syntax.BinaryExpression(
                        BinaryOperator.BarBar,
                        Syntax.LiteralExpression(1),
                        Syntax.LiteralExpression(1)
                    ),
                    Syntax.Block()
                ),
                p =>
                {
                    p.LineBreaksAndWrapping.LineWrapping.ForceChopCompoundConditionInIfStatement = force;
                    p.LineBreaksAndWrapping.LineWrapping.PreferWrapBeforeOperatorInBinaryExpression = before;
                }
            );
        }

        [TestCase(
           true,
@"do
{
}
while (1 ||
    1);
"
        )]
        [TestCase(
           false,
@"do
{
}
while (1 || 1);
"
        )]
        public void ForceChopCompoundConditionInDoStatement(bool force, string expected)
        {
            Test(
                expected,
                Syntax.DoStatement(
                    Syntax.Block(),
                    Syntax.BinaryExpression(
                        BinaryOperator.BarBar,
                        Syntax.LiteralExpression(1),
                        Syntax.LiteralExpression(1)
                    )
                ),
                p => p.LineBreaksAndWrapping.LineWrapping.ForceChopCompoundConditionInDoStatement = force
            );
        }

        [TestCase(
           true,
@"while (1 ||
    1)
{
}
"
        )]
        [TestCase(
           false,
@"while (1 || 1)
{
}
"
        )]
        public void ForceChopCompoundConditionInWhileStatement(bool force, string expected)
        {
            Test(
                expected,
                Syntax.WhileStatement(
                    Syntax.BinaryExpression(
                        BinaryOperator.BarBar,
                        Syntax.LiteralExpression(1),
                        Syntax.LiteralExpression(1)
                    ),
                    Syntax.Block()
                ),
                p => p.LineBreaksAndWrapping.LineWrapping.ForceChopCompoundConditionInWhileStatement = force
            );
        }

        [TestCase(
           WrapStyle.SimpleWrap,
@"1 ? 1 : 1"
        )]
        [TestCase(
           WrapStyle.ChopAlways,
@"1
    ? 1
    : 1"
        )]
        public void WrapTernaryExpression(WrapStyle wrapStyle, string expected)
        {
            Test(
                expected,
                Syntax.ConditionalExpression(
                    Syntax.LiteralExpression(1),
                    Syntax.LiteralExpression(1),
                    Syntax.LiteralExpression(1)
                ),
                p => p.LineBreaksAndWrapping.LineWrapping.WrapTernaryExpression = wrapStyle
            );
        }

        [TestCase(
           WrapStyle.SimpleWrap,
@"for (1; 1; 1)
{
}
"
        )]
        [TestCase(
           WrapStyle.ChopAlways,
@"for (1;
    1;
    1)
{
}
"
        )]
        public void WrapForStatementHeader(WrapStyle wrapStyle, string expected)
        {
            Test(
                expected,
                Syntax.ForStatement(
                    initializers: new[] { Syntax.LiteralExpression(1) },
                    condition: Syntax.LiteralExpression(1),
                    incrementors: new[] { Syntax.LiteralExpression(1) },
                    statement: Syntax.Block()
                ),
                p => p.LineBreaksAndWrapping.LineWrapping.WrapForStatementHeader = wrapStyle
            );
        }

        [TestCase(
           WrapStyle.SimpleWrap,
@"var i = 0, j = 0;
"
        )]
        [TestCase(
           WrapStyle.ChopAlways,
@"var i = 0,
    j = 0;
"
        )]
        public void WrapMultipleDeclarations(WrapStyle wrapStyle, string expected)
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
                p => p.LineBreaksAndWrapping.LineWrapping.WrapMultipleDeclarations = wrapStyle
            );
        }

        [TestCase(
           WrapStyle.SimpleWrap,
@"class Class : Interface, Interface
{
}
"
        )]
        [TestCase(
           WrapStyle.ChopAlways,
@"class Class : Interface,
    Interface
{
}
"
        )]
        public void WrapExtendsImplementsList(WrapStyle wrapStyle, string expected)
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
                p => p.LineBreaksAndWrapping.LineWrapping.WrapExtendsImplementsList = wrapStyle
            );
        }

        [TestCase(
           WrapStyle.SimpleWrap,
           true,
@"class Class where T : class where T : class
{
    void Method() where T : class where T : class
    {
    }

    delegate void Delegate() where T : class where T : class;
}
"
        )]
        [TestCase(
           WrapStyle.ChopAlways,
           true,
@"class Class where T : class
    where T : class
{
    void Method() where T : class
        where T : class
    {
    }

    delegate void Delegate() where T : class
        where T : class;
}
"
        )]
        [TestCase(
           WrapStyle.SimpleWrap,
           false,
@"class Class
    where T : class where T : class
{
    void Method()
        where T : class where T : class
    {
    }

    delegate void Delegate()
        where T : class where T : class;
}
"
        )]
        [TestCase(
           WrapStyle.ChopAlways,
           false,
@"class Class
    where T : class
    where T : class
{
    void Method()
        where T : class
        where T : class
    {
    }

    delegate void Delegate()
        where T : class
        where T : class;
}
"
        )]
        public void WrapMultipleTypeParameterConstraints(WrapStyle wrapStyle, bool sameLine, string expected)
        {
            Test(
                expected,
                Syntax.ClassDeclaration(
                    identifier: "Class",
                    constraintClauses: new[]
                    {
                        Syntax.TypeParameterConstraintClause(
                            (IdentifierNameSyntax)Syntax.ParseName("T"),
                            new[] { Syntax.ClassOrStructConstraint() }
                        ),
                        Syntax.TypeParameterConstraintClause(
                            (IdentifierNameSyntax)Syntax.ParseName("T"),
                            new[] { Syntax.ClassOrStructConstraint() }
                        )
                    },
                    members: new MemberDeclarationSyntax[]
                    {
                        Syntax.MethodDeclaration(
                            returnType: Syntax.ParseName("void"),
                            identifier: "Method",
                            parameterList: Syntax.ParameterList(),
                            body: Syntax.Block(),
                            constraintClauses: new[]
                            {
                                Syntax.TypeParameterConstraintClause(
                                    (IdentifierNameSyntax)Syntax.ParseName("T"),
                                    new[] { Syntax.ClassOrStructConstraint() }
                                ),
                                Syntax.TypeParameterConstraintClause(
                                    (IdentifierNameSyntax)Syntax.ParseName("T"),
                                    new[] { Syntax.ClassOrStructConstraint() }
                                )
                            }
                        ),
                        Syntax.DelegateDeclaration(
                            returnType: Syntax.ParseName("void"),
                            identifier: "Delegate",
                            parameterList: Syntax.ParameterList(),
                            constraintClauses: new[]
                            {
                                Syntax.TypeParameterConstraintClause(
                                    (IdentifierNameSyntax)Syntax.ParseName("T"),
                                    new[] { Syntax.ClassOrStructConstraint() }
                                ),
                                Syntax.TypeParameterConstraintClause(
                                    (IdentifierNameSyntax)Syntax.ParseName("T"),
                                    new[] { Syntax.ClassOrStructConstraint() }
                                )
                            }
                        )
                    }
                ),
                p =>
                {
                    p.LineBreaksAndWrapping.LineWrapping.WrapMultipleTypeParameterConstraints = wrapStyle;
                    p.LineBreaksAndWrapping.Other.PlaceTypeConstraintsOnSameLine = sameLine;
                }
            );
        }

        [TestCase(
            WrapStyle.SimpleWrap,
@"
{
    var i = new int[]
    {
        1, 2
    };
    var i = new[]
    {
        1, 2
    };
    var i = new Class
    {
        Name = 1, Name = 1
    };
    var i = new
    {
        Name = 1, Name = 1
    };
}
"
        )]
        [TestCase(
            WrapStyle.ChopAlways,
@"
{
    var i = new int[]
    {
        1,
        2
    };
    var i = new[]
    {
        1,
        2
    };
    var i = new Class
    {
        Name = 1,
        Name = 1
    };
    var i = new
    {
        Name = 1,
        Name = 1
    };
}
"
        )]
        public void WrapObjectAndCollectionInitializers(WrapStyle wrapStyle, string expected)
        {
            Test(
                expected,
                Syntax.Block(
                    Syntax.LocalDeclarationStatement(
                        Syntax.VariableDeclaration(
                            Syntax.ParseName("var"),
                            new[]
                            {
                                Syntax.VariableDeclarator(
                                    "i",
                                    null,
                                    Syntax.EqualsValueClause(
                                        Syntax.ArrayCreationExpression(
                                            (ArrayTypeSyntax)Syntax.ParseName("int[]"),
                                            Syntax.InitializerExpression(
                                                Syntax.LiteralExpression(1),
                                                Syntax.LiteralExpression(2)
                                            )
                                        )
                                    )
                                )
                            }
                        )
                    ),
                    Syntax.LocalDeclarationStatement(
                        Syntax.VariableDeclaration(
                            Syntax.ParseName("var"),
                            new[]
                            {
                                Syntax.VariableDeclarator(
                                    "i",
                                    null,
                                    Syntax.EqualsValueClause(
                                        Syntax.ImplicitArrayCreationExpression(
                                            0,
                                            Syntax.InitializerExpression(
                                                Syntax.LiteralExpression(1),
                                                Syntax.LiteralExpression(2)
                                            )
                                        )
                                    )
                                )
                            }
                        )
                    ),
                    Syntax.LocalDeclarationStatement(
                        Syntax.VariableDeclaration(
                            Syntax.ParseName("var"),
                            new[]
                            {
                                Syntax.VariableDeclarator(
                                    "i",
                                    initializer: Syntax.EqualsValueClause(
                                        Syntax.ObjectCreationExpression(
                                            Syntax.ParseName("Class"),
                                            initializer: Syntax.InitializerExpression(
                                                Syntax.BinaryExpression(
                                                    BinaryOperator.Equals,
                                                    Syntax.ParseName("Name"),
                                                    Syntax.LiteralExpression(1)
                                                ),
                                                Syntax.BinaryExpression(
                                                    BinaryOperator.Equals,
                                                    Syntax.ParseName("Name"),
                                                    Syntax.LiteralExpression(1)
                                                )
                                            )
                                        )
                                    )
                                )
                            }
                        )
                    ),
                    Syntax.LocalDeclarationStatement(
                        Syntax.VariableDeclaration(
                            Syntax.ParseName("var"),
                            new[]
                            {
                                Syntax.VariableDeclarator(
                                    "i",
                                    initializer: Syntax.EqualsValueClause(
                                        Syntax.AnonymousObjectCreationExpression(
                                            Syntax.AnonymousObjectMemberDeclarator(
                                                Syntax.NameEquals("Name"),
                                                Syntax.LiteralExpression(1)
                                            ),
                                            Syntax.AnonymousObjectMemberDeclarator(
                                                Syntax.NameEquals("Name"),
                                                Syntax.LiteralExpression(1)
                                            )
                                        )
                                    )
                                )
                            }
                        )
                    )
                ),
                p => p.LineBreaksAndWrapping.LineWrapping.WrapObjectAndCollectionInitializers = wrapStyle
            );
        }

        [Ignore("Not yet implemented")]
        public void PreferWrapBeforeParenInDeclaration()
        {
        }

        [Ignore("Not yet implemented")]
        public void PreferWrapAfterParenInDeclaration()
        {
        }

        [Ignore("Not yet implemented")]
        public void PreferWrapBeforeParenInInvocation()
        {
        }

        [Ignore("Not yet implemented")]
        public void PreferWrapAfterParenInInvocation()
        {
        }

        [Ignore("Not yet implemented")]
        public void PreferWrapBeforeColon()
        {
        }

        [Ignore("Not yet implemented")]
        public void PreferWrapBeforeFirstConstraint()
        {
        }

        [Ignore("Not yet implemented")]
        public void PreferWrapBeforeTypeParametersOpeningAngle()
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
