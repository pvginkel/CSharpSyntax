using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class OtherOtherFixture : TestBase
    {
        [TestCase(
            true,
@"
{
    // Before 1
    if (1)
    {
    }
    else if (2)
    {
    }
    // After 1
}
"
        )]
        [TestCase(
            false,
@"
{
    // Before 1
    if (1)
    {
    }
    else
        if (2)
        {
        }
    // After 1
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
    // Before 1
    using (1)
    // Before 2
    using (2)
    {
    }
    // After 2
    // After 1
}
"
        )]
        [TestCase(
            false,
@"
{
    // Before 1
    using (1)
        // Before 2
        using (2)
        {
        }
        // After 2
    // After 1
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
