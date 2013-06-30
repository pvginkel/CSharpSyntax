using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
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
label:
    {
    }
label:
    break;
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
label:
label:
    {
    }
label:
label:
    break;
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
