using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class TryStatementFixture : TestBase
    {
        [Test]
        public void SimpleTryCatch()
        {
            Test(
@"try
{
}
catch
{
}
",
                new TryStatementSyntax
                {
                    Block = new BlockSyntax(),
                    Catches =
                    {
                        new CatchClauseSyntax
                        {
                            Block = new BlockSyntax()
                        }
                    }
                }
            );
        }

        [Test]
        public void WithFinally()
        {
            Test(
@"try
{
}
finally
{
}
",
                new TryStatementSyntax
                {
                    Block = new BlockSyntax(),
                    Finally = new FinallyClauseSyntax
                    {
                        Block = new BlockSyntax()
                    }
                }
            );
        }

        [Test]
        public void WithCatchType()
        {
            Test(
@"try
{
}
catch (Exception)
{
}
",
                new TryStatementSyntax
                {
                    Block = new BlockSyntax(),
                    Catches =
                    {
                        new CatchClauseSyntax
                        {
                            Declaration = new CatchDeclarationSyntax
                            {
                                Type = Syntax.ParseName("Exception")
                            },
                            Block = new BlockSyntax()
                        }
                    }
                }
            );
        }

        [Test]
        public void WithCatchTypeAndIdentifier()
        {
            Test(
@"try
{
}
catch (Exception ex)
{
}
",
                new TryStatementSyntax
                {
                    Block = new BlockSyntax(),
                    Catches =
                    {
                        new CatchClauseSyntax
                        {
                            Declaration = new CatchDeclarationSyntax
                            {
                                Type = Syntax.ParseName("Exception"),
                                Identifier = "ex"
                            },
                            Block = new BlockSyntax()
                        }
                    }
                }
            );
        }

        [Test]
        public void WithMultipleCatches()
        {
            Test(
@"try
{
}
catch (Exception)
{
}
catch (Exception)
{
}
",
                new TryStatementSyntax
                {
                    Block = new BlockSyntax(),
                    Catches =
                    {
                        new CatchClauseSyntax
                        {
                            Declaration = new CatchDeclarationSyntax
                            {
                                Type = Syntax.ParseName("Exception")
                            },
                            Block = new BlockSyntax()
                        },
                        new CatchClauseSyntax
                        {
                            Declaration = new CatchDeclarationSyntax
                            {
                                Type = Syntax.ParseName("Exception")
                            },
                            Block = new BlockSyntax()
                        }
                    }
                }
            );
        }
    }
}
