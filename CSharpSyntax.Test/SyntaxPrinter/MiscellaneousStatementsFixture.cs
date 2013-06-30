using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class MiscellaneousStatementsFixture : TestBase
    {
        [Test]
        public void Break()
        {
            Test(
@"break;
",
                new BreakStatementSyntax());
        }

        [Test]
        public void Continue()
        {
            Test(
@"continue;
",
                new ContinueStatementSyntax());
        }

        [Test]
        public void Checked()
        {
            Test(
@"checked
{
}
",
                new CheckedStatementSyntax
                {
                    Kind = CheckedOrUnchecked.Checked,
                    Block = new BlockSyntax()
                }
            );
        }

        [Test]
        public void Unchecked()
        {
            Test(
@"unchecked
{
}
",
                new CheckedStatementSyntax
                {
                    Kind = CheckedOrUnchecked.Unchecked,
                    Block = new BlockSyntax()
                }
            );
        }

        [Test]
        public void EmptyStatement()
        {
            Test(
@";
",
                new EmptyStatementSyntax()
            );
        }
    }
}
