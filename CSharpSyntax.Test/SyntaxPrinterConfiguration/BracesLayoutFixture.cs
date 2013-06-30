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
    public class BracesLayoutFixture : TestBase
    {
        [TestCase(
            BraceFormatting.EndOfLine,
@"void Method(){
    break;
}
"
        )]
        [TestCase(
            BraceFormatting.EndOfLineKr,
@"void Method() {
    break;
}
"
        )]
        [TestCase(
            BraceFormatting.NextLine,
@"void Method()
{
    break;
}
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented,
@"void Method()
    {
    break;
    }
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented2,
@"void Method()
    {
        break;
    }
"
        )]
        public void MethodDeclaration(BraceFormatting braceFormatting, string expected)
        {
            Test(
                expected,
                Syntax.MethodDeclaration(
                    Syntax.ParseName("void"),
                    "Method",
                    Syntax.ParameterList(),
                    Syntax.Block(Syntax.BreakStatement())
                ),
                p => p.BracesLayout.MethodDeclaration = braceFormatting
            );
        }

        [TestCase(
            BraceFormatting.EndOfLine,
@"namespace Namespace{
    interface Interface{
        void Method();
    }
}
"
        )]
        [TestCase(
            BraceFormatting.EndOfLineKr,
@"namespace Namespace {
    interface Interface {
        void Method();
    }
}
"
        )]
        [TestCase(
            BraceFormatting.NextLine,
@"namespace Namespace
{
    interface Interface
    {
        void Method();
    }
}
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented,
@"namespace Namespace
    {
    interface Interface
        {
        void Method();
        }
    }
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented2,
@"namespace Namespace
    {
        interface Interface
            {
                void Method();
            }
    }
"
        )]
        public void TypeAndNamespaceDeclaration(BraceFormatting braceFormatting, string expected)
        {
            Test(
                expected,
                Syntax.NamespaceDeclaration(
                    (NameSyntax)Syntax.ParseName("Namespace"),
                    members: new[]
                    {
                        Syntax.InterfaceDeclaration(
                            identifier: "Interface",
                            members: new[]
                            {
                                Syntax.MethodDeclaration(
                                    Syntax.ParseName("void"),
                                    "Method",
                                    Syntax.ParameterList()
                                )
                            }
                        )
                    }
                ),
                p => p.BracesLayout.TypeAndNamespaceDeclaration = braceFormatting
            );
        }

        [TestCase(
            BraceFormatting.EndOfLine,
@"var del = delegate(){
    return 0;
};
"
        )]
        [TestCase(
            BraceFormatting.EndOfLineKr,
@"var del = delegate() {
    return 0;
};
"
        )]
        [TestCase(
            BraceFormatting.NextLine,
@"var del = delegate()
{
    return 0;
};
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented,
@"var del = delegate()
    {
    return 0;
    };
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented2,
@"var del = delegate()
    {
        return 0;
    };
"
        )]
        public void AnonymousMethodDeclaration(BraceFormatting braceFormatting, string expected)
        {
            Test(
                expected,
                Syntax.LocalDeclarationStatement(
                    Syntax.VariableDeclaration(
                        Syntax.ParseName("var"),
                        new[]
                        {
                            Syntax.VariableDeclarator(
                                "del",
                                initializer: Syntax.EqualsValueClause(
                                    Syntax.AnonymousMethodExpression(
                                        Syntax.ParameterList(),
                                        Syntax.Block(
                                            Syntax.ReturnStatement(Syntax.LiteralExpression(0))
                                        )
                                    )
                                )
                            )
                        }
                    )
                ),
                p => p.BracesLayout.AnonymousMethodDeclaration = braceFormatting
            );
        }

        [TestCase(
            BraceFormatting.EndOfLine,
@"switch (1)
{
    case 0:
    case 0:{
        break;
    }
}
"
        )]
        [TestCase(
            BraceFormatting.EndOfLineKr,
@"switch (1)
{
    case 0:
    case 0: {
        break;
    }
}
"
        )]
        [TestCase(
            BraceFormatting.NextLine,
@"switch (1)
{
    case 0:
    case 0:
    {
        break;
    }
}
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented,
@"switch (1)
{
    case 0:
    case 0:
        {
        break;
        }
}
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented2,
@"switch (1)
{
    case 0:
    case 0:
        {
            break;
        }
}
"
        )]
        public void BlockUnderCaseLabel(BraceFormatting braceFormatting, string expected)
        {
            Test(
                expected,
                Syntax.SwitchStatement(
                    Syntax.LiteralExpression(1),
                    new[]
                    {
                        Syntax.SwitchSection(
                            new[]
                            {
                                Syntax.SwitchLabel(CaseOrDefault.Case, Syntax.LiteralExpression(0)),
                                Syntax.SwitchLabel(CaseOrDefault.Case, Syntax.LiteralExpression(0))
                            },
                            new[] { Syntax.Block(Syntax.BreakStatement()) }
                        )
                    }
                ),
                p => p.BracesLayout.BlockUnderCaseLabel = braceFormatting
            );
        }

        [TestCase(
            BraceFormatting.EndOfLine,
@"
{
    var array = new[]{
        1, 2, 3
    };
    var obj = new Class{
        Name = 1
    };
    var obj = new{
        Name = 1
    };
}
"
        )]
        [TestCase(
            BraceFormatting.EndOfLineKr,
@"
{
    var array = new[] {
        1, 2, 3
    };
    var obj = new Class {
        Name = 1
    };
    var obj = new {
        Name = 1
    };
}
"
        )]
        [TestCase(
            BraceFormatting.NextLine,
@"
{
    var array = new[]
    {
        1, 2, 3
    };
    var obj = new Class
    {
        Name = 1
    };
    var obj = new
    {
        Name = 1
    };
}
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented,
@"
{
    var array = new[]
        {
        1, 2, 3
        };
    var obj = new Class
        {
        Name = 1
        };
    var obj = new
        {
        Name = 1
        };
}
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented2,
@"
{
    var array = new[]
        {
            1, 2, 3
        };
    var obj = new Class
        {
            Name = 1
        };
    var obj = new
        {
            Name = 1
        };
}
"
        )]
        public void ArrayAndObjectInitializer(BraceFormatting braceFormatting, string expected)
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
                                    "array",
                                    initializer: Syntax.EqualsValueClause(
                                        Syntax.ImplicitArrayCreationExpression(
                                            Syntax.InitializerExpression(
                                                Syntax.LiteralExpression(1),
                                                Syntax.LiteralExpression(2),
                                                Syntax.LiteralExpression(3)
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
                                    "obj",
                                    initializer: Syntax.EqualsValueClause(
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
                                    "obj",
                                    initializer: Syntax.EqualsValueClause(
                                        Syntax.AnonymousObjectCreationExpression(
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
                p => p.BracesLayout.ArrayAndObjectInitializer = braceFormatting
            );
        }

        [TestCase(
            BraceFormatting.EndOfLine,
@"if (1){
    break;
}
"
        )]
        [TestCase(
            BraceFormatting.EndOfLineKr,
@"if (1) {
    break;
}
"
        )]
        [TestCase(
            BraceFormatting.NextLine,
@"if (1)
{
    break;
}
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented,
@"if (1)
    {
    break;
    }
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented2,
@"if (1)
    {
        break;
    }
"
        )]
        public void Other(BraceFormatting braceFormatting, string expected)
        {
            Test(
                expected,
                Syntax.IfStatement(
                    Syntax.LiteralExpression(1),
                    Syntax.Block(Syntax.BreakStatement())
                ),
                p => p.BracesLayout.Other = braceFormatting
            );
        }

        [TestCase(
            BraceFormatting.EndOfLine,
            BraceFormatting.NextLine,
            BraceFormatting.NextLine,
            BraceFormatting.NextLine,
@"class Class{
    void Method()
    {
        if (1)
        {
            var array = new[]
            {
                1, 2, 3
            };
        }
    }
}
"
        )]
        [TestCase(
            BraceFormatting.NextLine,
            BraceFormatting.EndOfLine,
            BraceFormatting.NextLine,
            BraceFormatting.NextLine,
@"class Class
{
    void Method(){
        if (1)
        {
            var array = new[]
            {
                1, 2, 3
            };
        }
    }
}
"
        )]
        [TestCase(
            BraceFormatting.NextLine,
            BraceFormatting.NextLine,
            BraceFormatting.EndOfLine,
            BraceFormatting.NextLine,
@"class Class
{
    void Method()
    {
        if (1)
        {
            var array = new[]{
                1, 2, 3
            };
        }
    }
}
"
        )]
        [TestCase(
            BraceFormatting.NextLine,
            BraceFormatting.NextLine,
            BraceFormatting.NextLine,
            BraceFormatting.EndOfLine,
@"class Class
{
    void Method()
    {
        if (1){
            var array = new[]
            {
                1, 2, 3
            };
        }
    }
}
"
        )]
        public void Combinations(BraceFormatting typeAndNamespaceDeclaration, BraceFormatting methodDeclaration, BraceFormatting arrayAndObjectInitializer, BraceFormatting other, string expected)
        {
            Test(
                expected,
                Syntax.ClassDeclaration(
                    identifier: "Class",
                    members: new[]
                    {
                        Syntax.MethodDeclaration(
                            Syntax.ParseName("void"),
                            "Method",
                            Syntax.ParameterList(),
                            Syntax.Block(
                                Syntax.IfStatement(
                                    Syntax.LiteralExpression(1),
                                    Syntax.Block(
                                        Syntax.LocalDeclarationStatement(
                                            Syntax.VariableDeclaration(
                                                Syntax.ParseName("var"),
                                                new[]
                                                {
                                                    Syntax.VariableDeclarator(
                                                        "array",
                                                        initializer: Syntax.EqualsValueClause(
                                                            Syntax.ImplicitArrayCreationExpression(
                                                                Syntax.InitializerExpression(
                                                                    Syntax.LiteralExpression(1),
                                                                    Syntax.LiteralExpression(2),
                                                                    Syntax.LiteralExpression(3)
                                                                )
                                                            )
                                                        )
                                                    )
                                                }
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    }
                ),
                p =>
                {
                    p.BracesLayout.TypeAndNamespaceDeclaration = typeAndNamespaceDeclaration;
                    p.BracesLayout.MethodDeclaration = methodDeclaration;
                    p.BracesLayout.ArrayAndObjectInitializer = arrayAndObjectInitializer;
                    p.BracesLayout.Other = other;
                }
            );
        }

        [TestCase(
            Printer.Configuration.EmptyBraceFormatting.OnDifferentLines,
@"void Method()
{
}
"
        )]
        [TestCase(
            Printer.Configuration.EmptyBraceFormatting.PlaceTogether,
@"void Method()
{}
"
        )]
        [TestCase(
            Printer.Configuration.EmptyBraceFormatting.TogetherOnSameLine,
@"void Method() {}
"
        )]
        public void EmptyBraceFormatting(EmptyBraceFormatting emptyBraceFormatting, string expected)
        {
            Test(
                expected,
                Syntax.MethodDeclaration(
                    Syntax.ParseName("void"),
                    "Method",
                    Syntax.ParameterList(),
                    Syntax.Block()
                ),
                p => p.BracesLayout.EmptyBraceFormatting = emptyBraceFormatting
            );
        }
    }
}
