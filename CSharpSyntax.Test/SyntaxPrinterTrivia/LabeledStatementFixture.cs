using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class LabeledStatementFixture : TestBase
    {
        [Test]
        public void Simple()
        {
            Test(
@"
{
    // Before 1
label:
    // After 1
    {
    }
    // Before 2
label:
    // After 2
    // Before 3
    break;
    // After 3
}
",
                Syntax.Block(
                    Syntax.LabeledStatement("label", Syntax.Block()),
                    Syntax.LabeledStatement("label", Syntax.BreakStatement())
                )
            );
        }

        [Test]
        public void Multiple()
        {
            Test(
@"
{
    // Before 1
label:
    // After 1
    // Before 2
label:
    // After 2
    {
    }
    // Before 3
label:
    // After 3
    // Before 4
label:
    // After 4
    // Before 5
    break;
    // After 5
}
",
                Syntax.Block(
                    Syntax.LabeledStatement(
                        "label",
                        Syntax.LabeledStatement("label", Syntax.Block())
                    ),
                    Syntax.LabeledStatement(
                        "label",
                        Syntax.LabeledStatement("label", Syntax.BreakStatement())
                    )
                )
            );
        }
    }
}
