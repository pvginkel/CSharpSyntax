using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class MiscellaneousStatementsFixture : TestBase
    {
        [Test]
        public void Break()
        {
            Test(
@"// Before 1
break;
// After 1
",
                new BreakStatementSyntax());
        }

        [Test]
        public void Continue()
        {
            Test(
@"// Before 1
continue;
// After 1
",
                new ContinueStatementSyntax());
        }

        [Test]
        public void Checked()
        {
            Test(
@"// Before 1
checked
{
}
// After 1
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
@"// Before 1
unchecked
{
}
// After 1
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
@"// Before 1
;
// After 1
",
                new EmptyStatementSyntax()
            );
        }
    }
}
