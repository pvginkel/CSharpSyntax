using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class ElementAccessFixture : TestBase
    {
        [Test]
        public void ElementAccess()
        {
            Test(
                "i[10]",
                Syntax.ElementAccessExpression(
                    Syntax.ParseName("i"),
                    Syntax.BracketedArgumentList(
                        Syntax.Argument(Syntax.LiteralExpression(10))
                    )
                )
            );
        }
    }
}
