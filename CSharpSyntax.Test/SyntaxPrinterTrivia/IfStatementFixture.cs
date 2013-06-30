using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class IfStatementFixture : TestBase
    {
        [Test]
        public void Simple()
        {
            Test(
@"// Before 1
if (1)
    // Before 2
    break;
    // After 2
// After 1
",
                new IfStatementSyntax
                {
                    Condition = new LiteralExpressionSyntax { Value = 1 },
                    Statement = new BreakStatementSyntax()
                }
            );
        }

        [Test]
        public void WithBlock()
        {
            Test(
@"// Before 1
if (1)
{
}
// After 1
",
                new IfStatementSyntax
                {
                    Condition = new LiteralExpressionSyntax { Value = 1 },
                    Statement = new BlockSyntax()
                }
            );
        }

        [Test]
        public void WithElse()
        {
            Test(
@"// Before 1
if (1)
{
}
else
{
}
// After 1
",
                new IfStatementSyntax
                {
                    Condition = new LiteralExpressionSyntax { Value = 1 },
                    Statement = new BlockSyntax(),
                    Else = new ElseClauseSyntax
                    {
                        Statement = new BlockSyntax()
                    }
                }
            );
        }

        [Test]
        public void WithElseIf()
        {
            Test(
@"// Before 1
if (1)
{
}
else if (1)
{
}
// After 1
",
                new IfStatementSyntax
                {
                    Condition = new LiteralExpressionSyntax { Value = 1 },
                    Statement = new BlockSyntax(),
                    Else = new ElseClauseSyntax
                    {
                        Statement = new IfStatementSyntax
                        {
                            Condition = new LiteralExpressionSyntax { Value = 1 },
                            Statement = new BlockSyntax()
                        }
                    }
                }
            );
        }

        [Test]
        public void WithElseIfVerifyIndent()
        {
            Test(
@"// Before 1
if (1)
    // Before 2
    if (1)
    {
    }
    else if (1)
    {
    }
    // After 2
// After 1
",
                new IfStatementSyntax
                {
                    Condition = new LiteralExpressionSyntax { Value = 1 },
                    Statement = new IfStatementSyntax
                    {
                        Condition = new LiteralExpressionSyntax { Value = 1 },
                        Statement = new BlockSyntax(),
                        Else = new ElseClauseSyntax
                        {
                            Statement = new IfStatementSyntax
                            {
                                Condition = new LiteralExpressionSyntax { Value = 1 },
                                Statement = new BlockSyntax()
                            }
                        }
                    }
                }
            );
        }
    }
}
