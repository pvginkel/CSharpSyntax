using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class SwitchStatementFixture : TestBase
    {
        [Test]
        public void WithoutSections()
        {
            Test(
@"switch (1)
{
}
",
                Syntax.SwitchStatement(Syntax.LiteralExpression(1))
            );
        }

        [Test]
        public void WithCase()
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
                            new[] { Syntax.SwitchLabel(CaseOrDefault.Case, Syntax.LiteralExpression(1)) },
                            new[] { Syntax.BreakStatement() }
                        )
                    }
                )
            );
        }

        [Test]
        public void WithMultipleCases()
        {
            Test(
@"switch (1)
{
    case 1:
        break;

    case 2:
        break;
}
",
                Syntax.SwitchStatement(
                    Syntax.LiteralExpression(1),
                    new[]
                    {
                        Syntax.SwitchSection(
                            new[] { Syntax.SwitchLabel(CaseOrDefault.Case, Syntax.LiteralExpression(1)) },
                            new[] { Syntax.BreakStatement() }
                        ),
                        Syntax.SwitchSection(
                            new[] { Syntax.SwitchLabel(CaseOrDefault.Case, Syntax.LiteralExpression(2)) },
                            new[] { Syntax.BreakStatement() }
                        )
                    }
                )
            );
        }

        [Test]
        public void WithDefault()
        {
            Test(
@"switch (1)
{
    case 1:
        break;

    default:
        break;
}
",
                Syntax.SwitchStatement(
                    Syntax.LiteralExpression(1),
                    new[]
                    {
                        Syntax.SwitchSection(
                            new[] { Syntax.SwitchLabel(CaseOrDefault.Case, Syntax.LiteralExpression(1)) },
                            new[] { Syntax.BreakStatement() }
                        ),
                        Syntax.SwitchSection(
                            new[] { Syntax.SwitchLabel(CaseOrDefault.Default) },
                            new[] { Syntax.BreakStatement() }
                        )
                    }
                )
            );
        }

        [Test]
        public void WithMultipleLabels()
        {
            Test(
@"switch (1)
{
    case 1:
    case 2:
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
                                Syntax.SwitchLabel(CaseOrDefault.Case, Syntax.LiteralExpression(1)),
                                Syntax.SwitchLabel(CaseOrDefault.Case, Syntax.LiteralExpression(2))
                            },
                            new[] { Syntax.BreakStatement() }
                        )
                    }
                )
            );
        }

        [Test]
        public void WithMultipleLabelsAndDefault()
        {
            Test(
@"switch (1)
{
    case 1:
    default:
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
                                Syntax.SwitchLabel(CaseOrDefault.Case, Syntax.LiteralExpression(1)),
                                Syntax.SwitchLabel(CaseOrDefault.Default)
                            },
                            new[] { Syntax.BreakStatement() }
                        )
                    }
                )
            );
        }
    }
}
