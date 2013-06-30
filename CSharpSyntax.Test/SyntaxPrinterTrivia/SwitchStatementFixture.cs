using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class SwitchStatementFixture : TestBase
    {
        [Test]
        public void WithoutSections()
        {
            Test(
@"// Before 1
switch (1)
{
}
// After 1
",
                Syntax.SwitchStatement(Syntax.LiteralExpression(1))
            );
        }

        [Test]
        public void WithCase()
        {
            Test(
@"// Before 1
switch (1)
{
    // Before 2
    case 1:
        // Before 3
        break;// After 3
    // After 2

}
// After 1
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
@"// Before 1
switch (1)
{
    // Before 2
    case 1:
        // Before 3
        break;// After 3
    // After 2


    // Before 4
    case 2:
        // Before 5
        break;// After 5
    // After 4

}
// After 1
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
@"// Before 1
switch (1)
{
    // Before 2
    case 1:
        // Before 3
        break;// After 3
    // After 2


    // Before 4
    default:
        // Before 5
        break;// After 5
    // After 4

}
// After 1
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
@"// Before 1
switch (1)
{
    // Before 2
    case 1:
    case 2:
        // Before 3
        break;// After 3
    // After 2

}
// After 1
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
@"// Before 1
switch (1)
{
    // Before 2
    case 1:
    default:
        // Before 3
        break;// After 3
    // After 2

}
// After 1
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
