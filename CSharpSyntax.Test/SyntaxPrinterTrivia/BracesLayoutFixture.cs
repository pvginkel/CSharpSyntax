using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpSyntax.Printer.Configuration;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class BracesLayoutFixture : TestBase
    {
        [TestCase(
            BraceFormatting.EndOfLine,
@"// Before 1
void Method(){
    // Before 2
    break;
    // After 2
}
// After 1
"
        )]
        [TestCase(
            BraceFormatting.EndOfLineKr,
@"// Before 1
void Method() {
    // Before 2
    break;
    // After 2
}
// After 1
"
        )]
        [TestCase(
            BraceFormatting.NextLine,
@"// Before 1
void Method()
{
    // Before 2
    break;
    // After 2
}
// After 1
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented,
@"// Before 1
void Method()
    {
    // Before 2
    break;
    // After 2
    }
// After 1
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented2,
@"// Before 1
void Method()
    {
        // Before 2
        break;
        // After 2
    }
// After 1
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
@"// Before 1
namespace Namespace{
    // Before 2
    interface Interface{
        // Before 3
        void Method();
        // After 3
    }
    // After 2
}
// After 1
"
        )]
        [TestCase(
            BraceFormatting.EndOfLineKr,
@"// Before 1
namespace Namespace {
    // Before 2
    interface Interface {
        // Before 3
        void Method();
        // After 3
    }
    // After 2
}
// After 1
"
        )]
        [TestCase(
            BraceFormatting.NextLine,
@"// Before 1
namespace Namespace
{
    // Before 2
    interface Interface
    {
        // Before 3
        void Method();
        // After 3
    }
    // After 2
}
// After 1
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented,
@"// Before 1
namespace Namespace
    {
    // Before 2
    interface Interface
        {
        // Before 3
        void Method();
        // After 3
        }
    // After 2
    }
// After 1
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented2,
@"// Before 1
namespace Namespace
    {
        // Before 2
        interface Interface
            {
                // Before 3
                void Method();
                // After 3
            }
        // After 2
    }
// After 1
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
@"// Before 1
var del = delegate(){
    // Before 2
    return 0;
    // After 2
};
// After 1
"
        )]
        [TestCase(
            BraceFormatting.EndOfLineKr,
@"// Before 1
var del = delegate() {
    // Before 2
    return 0;
    // After 2
};
// After 1
"
        )]
        [TestCase(
            BraceFormatting.NextLine,
@"// Before 1
var del = delegate()
{
    // Before 2
    return 0;
    // After 2
};
// After 1
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented,
@"// Before 1
var del = delegate()
    {
    // Before 2
    return 0;
    // After 2
    };
// After 1
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented2,
@"// Before 1
var del = delegate()
    {
        // Before 2
        return 0;
        // After 2
    };
// After 1
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
@"// Before 1
switch (1)
{
    // Before 2
    case 0:
    case 0:{
        // Before 3
        break;
        // After 3
    }// After 2

}
// After 1
"
        )]
        [TestCase(
            BraceFormatting.EndOfLineKr,
@"// Before 1
switch (1)
{
    // Before 2
    case 0:
    case 0: {
        // Before 3
        break;
        // After 3
    }// After 2

}
// After 1
"
        )]
        [TestCase(
            BraceFormatting.NextLine,
@"// Before 1
switch (1)
{
    // Before 2
    case 0:
    case 0:
    {
        // Before 3
        break;
        // After 3
    }// After 2

}
// After 1
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented,
@"// Before 1
switch (1)
{
    // Before 2
    case 0:
    case 0:
        {
        // Before 3
        break;
        // After 3
    }// After 2

}
// After 1
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented2,
@"// Before 1
switch (1)
{
    // Before 2
    case 0:
    case 0:
        {
            // Before 3
            break;
            // After 3
    }// After 2

}
// After 1
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
    // Before 1
    var array = new[]{
        1, 2, 3
    };
    // After 1
    // Before 2
    var obj = new Class{
        Name = 1
    };
    // After 2
    // Before 3
    var obj = new{
        Name = 1
    };
    // After 3
}
"
        )]
        [TestCase(
            BraceFormatting.EndOfLineKr,
@"
{
    // Before 1
    var array = new[] {
        1, 2, 3
    };
    // After 1
    // Before 2
    var obj = new Class {
        Name = 1
    };
    // After 2
    // Before 3
    var obj = new {
        Name = 1
    };
    // After 3
}
"
        )]
        [TestCase(
            BraceFormatting.NextLine,
@"
{
    // Before 1
    var array = new[]
    {
        1, 2, 3
    };
    // After 1
    // Before 2
    var obj = new Class
    {
        Name = 1
    };
    // After 2
    // Before 3
    var obj = new
    {
        Name = 1
    };
    // After 3
}
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented,
@"
{
    // Before 1
    var array = new[]
        {
        1, 2, 3
        };
    // After 1
    // Before 2
    var obj = new Class
        {
        Name = 1
        };
    // After 2
    // Before 3
    var obj = new
        {
        Name = 1
        };
    // After 3
}
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented2,
@"
{
    // Before 1
    var array = new[]
        {
            1, 2, 3
        };
    // After 1
    // Before 2
    var obj = new Class
        {
            Name = 1
        };
    // After 2
    // Before 3
    var obj = new
        {
            Name = 1
        };
    // After 3
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
@"// Before 1
if (1){
    // Before 2
    break;
    // After 2
}
// After 1
"
        )]
        [TestCase(
            BraceFormatting.EndOfLineKr,
@"// Before 1
if (1) {
    // Before 2
    break;
    // After 2
}
// After 1
"
        )]
        [TestCase(
            BraceFormatting.NextLine,
@"// Before 1
if (1)
{
    // Before 2
    break;
    // After 2
}
// After 1
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented,
@"// Before 1
if (1)
    {
    // Before 2
    break;
    // After 2
    }
// After 1
"
        )]
        [TestCase(
            BraceFormatting.NextLineIndented2,
@"// Before 1
if (1)
    {
        // Before 2
        break;
        // After 2
    }
// After 1
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
@"// Before 1
class Class{
    // Before 2
    void Method()
    {
        // Before 3
        if (1)
        {
            // Before 4
            var array = new[]
            {
                1, 2, 3
            };
            // After 4
        }
        // After 3
    }
    // After 2
}
// After 1
"
        )]
        [TestCase(
            BraceFormatting.NextLine,
            BraceFormatting.EndOfLine,
            BraceFormatting.NextLine,
            BraceFormatting.NextLine,
@"// Before 1
class Class
{
    // Before 2
    void Method(){
        // Before 3
        if (1)
        {
            // Before 4
            var array = new[]
            {
                1, 2, 3
            };
            // After 4
        }
        // After 3
    }
    // After 2
}
// After 1
"
        )]
        [TestCase(
            BraceFormatting.NextLine,
            BraceFormatting.NextLine,
            BraceFormatting.EndOfLine,
            BraceFormatting.NextLine,
@"// Before 1
class Class
{
    // Before 2
    void Method()
    {
        // Before 3
        if (1)
        {
            // Before 4
            var array = new[]{
                1, 2, 3
            };
            // After 4
        }
        // After 3
    }
    // After 2
}
// After 1
"
        )]
        [TestCase(
            BraceFormatting.NextLine,
            BraceFormatting.NextLine,
            BraceFormatting.NextLine,
            BraceFormatting.EndOfLine,
@"// Before 1
class Class
{
    // Before 2
    void Method()
    {
        // Before 3
        if (1){
            // Before 4
            var array = new[]
            {
                1, 2, 3
            };
            // After 4
        }
        // After 3
    }
    // After 2
}
// After 1
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
@"// Before 1
void Method()
{
}
// After 1
"
        )]
        [TestCase(
            Printer.Configuration.EmptyBraceFormatting.PlaceTogether,
@"// Before 1
void Method()
{}
// After 1
"
        )]
        [TestCase(
            Printer.Configuration.EmptyBraceFormatting.TogetherOnSameLine,
@"// Before 1
void Method() {}
// After 1
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
