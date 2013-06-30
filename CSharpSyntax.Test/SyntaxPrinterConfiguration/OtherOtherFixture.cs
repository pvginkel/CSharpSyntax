using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterConfiguration
{
    [TestFixture]
    public class OtherOtherFixture : TestBase
    {
        [TestCase(
            true,
@"
{
    if (1)
    {
    }
    else if (2)
    {
    }
}
"
        )]
        [TestCase(
            false,
@"
{
    if (1)
    {
    }
    else
        if (2)
        {
        }
}
"
        )]
        public void SpecialElseIfTreatment(bool apply, string expected)
        {
            Test(
                expected,
                Syntax.Block(
                    Syntax.IfStatement(
                        Syntax.LiteralExpression(1),
                        Syntax.Block(),
                        Syntax.ElseClause(
                            Syntax.IfStatement(
                                Syntax.LiteralExpression(2),
                                Syntax.Block()
                            )
                        )
                    )
                ),
                p => p.Other.Other.SpecialElseIfTreatment = apply
            );
        }

        [TestCase(
            true,
@"
{
    switch (1)
    {
        case 1:
            break;
    }
}
"
        )]
        [TestCase(
            false,
@"
{
    switch (1)
    {
    case 1:
        break;
    }
}
"
        )]
        public void IndentCaseFromSwitch(bool apply, string expected)
        {
            Test(
                expected,
                Syntax.Block(
                    Syntax.SwitchStatement(
                        Syntax.LiteralExpression(1),
                        new[]
                        {
                            Syntax.SwitchSection(
                                new[]
                                {
                                    Syntax.SwitchLabel(
                                        CaseOrDefault.Case,
                                        Syntax.LiteralExpression(1)
                                    )
                                },
                                new[]
                                {
                                    Syntax.BreakStatement()
                                }
                            )
                        }
                    )
                ),
                p => p.Other.Other.IndentCaseFromSwitch = apply
            );
        }

        [TestCase(
            true,
@"
{
    using (1)
    using (2)
    {
    }
}
"
        )]
        [TestCase(
            false,
@"
{
    using (1)
        using (2)
        {
        }
}
"
        )]
        public void IndentNestedUsingStatements(bool apply, string expected)
        {
            Test(
                expected,
                Syntax.Block(
                    Syntax.UsingStatement(
                        null,
                        Syntax.LiteralExpression(1),
                        Syntax.UsingStatement(
                            null,
                            Syntax.LiteralExpression(2),
                            Syntax.Block()
                        )
                    )
                ),
                p => p.Other.Other.IndentNestedUsingStatements = apply
            );
        }
    }
}
