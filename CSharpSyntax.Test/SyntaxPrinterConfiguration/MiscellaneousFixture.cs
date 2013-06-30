using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterConfiguration
{
    [TestFixture]
    public class MiscellaneousFixture : TestBase
    {
        [Test]
        public void EmptyBlocks()
        {
            Test(
@"void Method()
{
    {
    }
    {
    }
}
",
                Syntax.MethodDeclaration(
                    Syntax.ParseName("void"),
                    "Method",
                    Syntax.ParameterList(),
                    Syntax.Block(
                        Syntax.Block(),
                        Syntax.Block()
                    )
                )
            );
        }

        [Test]
        public void ComplexCaseStatement()
        {
            Test(
@"switch (1)
{
    case 1:
        {
        }
        {
        }
        break;
        {
        }
        break;
        break;

    case 1:
        {
            {
            }
            {
            }
            break;
            {
            }
            break;
            break;
        }
}
",
                Syntax.SwitchStatement(
                    Syntax.LiteralExpression(1),
                    new[]
                    {
                        Syntax.SwitchSection(
                            new[]
                            {
                                Syntax.SwitchLabel(CaseOrDefault.Case, Syntax.LiteralExpression(1))
                            },
                            new StatementSyntax[]
                            {
                                Syntax.Block(),
                                Syntax.Block(),
                                Syntax.BreakStatement(),
                                Syntax.Block(),
                                Syntax.BreakStatement(),
                                Syntax.BreakStatement()
                            }
                        ),
                        Syntax.SwitchSection(
                            new[]
                            {
                                Syntax.SwitchLabel(CaseOrDefault.Case, Syntax.LiteralExpression(1))
                            },
                            new[]
                            {
                                Syntax.Block(
                                    Syntax.Block(),
                                    Syntax.Block(),
                                    Syntax.BreakStatement(),
                                    Syntax.Block(),
                                    Syntax.BreakStatement(),
                                    Syntax.BreakStatement()
                                )
                            }
                        )
                    }
                ),
                p => p.BracesLayout.BlockUnderCaseLabel = Printer.Configuration.BraceFormatting.NextLineIndented2
            );
        }

        [Test]
        public void ComplexIfStatement()
        {
            Test(
@"if (1)
{
    {
    }
    {
    }
    break;
    {
    }
    break;
    break;
}
",
                Syntax.IfStatement(
                    Syntax.LiteralExpression(1),
                    Syntax.Block(
                        Syntax.Block(),
                        Syntax.Block(),
                        Syntax.BreakStatement(),
                        Syntax.Block(),
                        Syntax.BreakStatement(),
                        Syntax.BreakStatement()
                    )
                )
            );
        }

        [Test]
        public void SimpleCaseStatement()
        {
            Test(
@"switch (1)
{
    case 1:
        break;
}
",
                Syntax.SwitchStatement(
                    Syntax.LiteralExpression(1),
                    new[]
                    {
                        Syntax.SwitchSection(
                            new[]
                            {
                                Syntax.SwitchLabel(CaseOrDefault.Case, Syntax.LiteralExpression(1))
                            },
                            new StatementSyntax[]
                            {
                                Syntax.BreakStatement()
                            }
                        )
                    }
                ),
                p => p.BracesLayout.BlockUnderCaseLabel = Printer.Configuration.BraceFormatting.NextLineIndented2
            );
        }

        [Test]
        public void MultipleCaseStatements()
        {
            Test(
@"switch (1)
{
    case 1:
        break;

    case 1:
        break;

    case 1:
        break;

    case 1:
        {
        }
}
",
                Syntax.SwitchStatement(
                    Syntax.LiteralExpression(1),
                    new[]
                    {
                        Syntax.SwitchSection(
                            new[]
                            {
                                Syntax.SwitchLabel(CaseOrDefault.Case, Syntax.LiteralExpression(1))
                            },
                            new StatementSyntax[]
                            {
                                Syntax.BreakStatement()
                            }
                        ),
                        Syntax.SwitchSection(
                            new[]
                            {
                                Syntax.SwitchLabel(CaseOrDefault.Case, Syntax.LiteralExpression(1))
                            },
                            new StatementSyntax[]
                            {
                                Syntax.BreakStatement()
                            }
                        ),
                        Syntax.SwitchSection(
                            new[]
                            {
                                Syntax.SwitchLabel(CaseOrDefault.Case, Syntax.LiteralExpression(1))
                            },
                            new StatementSyntax[]
                            {
                                Syntax.BreakStatement()
                            }
                        ),
                        Syntax.SwitchSection(
                            new[]
                            {
                                Syntax.SwitchLabel(CaseOrDefault.Case, Syntax.LiteralExpression(1))
                            },
                            new StatementSyntax[]
                            {
                                Syntax.Block()
                            }
                        )
                    }
                ),
                p => p.BracesLayout.BlockUnderCaseLabel = Printer.Configuration.BraceFormatting.NextLineIndented2
            );
        }

        [Test]
        public void StaticConstructor()
        {
            Test(
@"static Constructor()
{
}
",
                Syntax.ConstructorDeclaration(
                    identifier: "Constructor",
                    parameterList: Syntax.ParameterList(),
                    body: Syntax.Block(),
                    modifiers: Modifiers.Static
                )
            );
        }

        [Test]
        public void Interface()
        {
            Test(
@"interface Interface
{
    string Property { get; }

    void Method();

    event EventHandler Event;
}
",
                Syntax.InterfaceDeclaration(
                    identifier: "Interface",
                    members: new MemberDeclarationSyntax[]
                    {
                        Syntax.PropertyDeclaration(
                            type: Syntax.ParseName("string"),
                            identifier: "Property",
                            accessorList: Syntax.AccessorList(
                                Syntax.AccessorDeclaration(
                                    AccessorDeclarationKind.Get,
                                    null
                                )
                            )
                        ),
                        Syntax.MethodDeclaration(
                            Syntax.ParseName("void"),
                            "Method",
                            Syntax.ParameterList(),
                            null
                        ),
                        Syntax.EventFieldDeclaration(
                            Syntax.VariableDeclaration(
                                Syntax.ParseName("EventHandler"),
                                new[] { Syntax.VariableDeclarator("Event") }
                            )
                        )
                    }
                )
            );
        }

        [Test]
        public void Checked()
        {
            Test(
@"checked
{
}
",
                Syntax.CheckedStatement(CheckedOrUnchecked.Checked, Syntax.Block())
            );
        }

        [Test]
        public void SimpleIf()
        {
            Test(
@"if (1)
    break;
",
                Syntax.IfStatement(
                    Syntax.LiteralExpression(1),
                    Syntax.BreakStatement()
                )
            );
        }

        [Test]
        public void ArrayCreation()
        {
            Test(
@"var i = new int[]
{
    1
};
",
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
                                            Syntax.LiteralExpression(1)
                                        )
                                    )
                                )
                            )
                        }
                    )
                )
            );
        }

        [Test]
        public void ArrayWithRankCreation()
        {
            var arrayType = (ArrayTypeSyntax)Syntax.ParseName("int[]");

            arrayType.RankSpecifiers[0].Sizes[0] = Syntax.LiteralExpression(7);

            Test(
@"var i = new int[7];
",
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
                                        arrayType
                                    )
                                )
                            )
                        }
                    )
                )
            );
        }

        [Test]
        public void ImplicitArrayCreation()
        {
            Test(
@"var i = new[]
{
    1
};
",
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
                                            Syntax.LiteralExpression(1)
                                        )
                                    )
                                )
                            )
                        }
                    )
                )
            );
        }
    }
}
