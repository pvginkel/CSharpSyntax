using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class IfStatementFixture : TestBase
    {
        [Test]
        public void Simple()
        {
            Test(
@"if (1)
    break;
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
@"if (1)
{
}
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
@"if (1)
{
}
else
{
}
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
@"if (1)
{
}
else if (1)
{
}
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
@"if (1)
    if (1)
    {
    }
    else if (1)
    {
    }
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
