using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class TryStatementFixture : TestBase
    {
        [Test]
        public void SimpleTryCatch()
        {
            Test(
@"// Before 1
try
{
}
catch
{
}
// After 1
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
@"// Before 1
try
{
}
finally
{
}
// After 1
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
@"// Before 1
try
{
}
catch (Exception)
{
}
// After 1
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
@"// Before 1
try
{
}
catch (Exception ex)
{
}
// After 1
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
@"// Before 1
try
{
}
catch (Exception)
{
}
catch (Exception)
{
}
// After 1
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
