using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class UsingStatementFixture : TestBase
    {
        [Test]
        public void WithExpression()
        {
            Test(
@"// Before 1
using (1)
{
}
// After 1
",
                Syntax.UsingStatement(
                    expression: Syntax.LiteralExpression(1),
                    statement: Syntax.Block()
                )
            );
        }

        [Test]
        public void WithDeclaration()
        {
            Test(
@"// Before 1
using (int i = 1)
{
}
// After 1
",
                Syntax.UsingStatement(
                    declaration: Syntax.VariableDeclaration(
                        Syntax.ParseName("int"),
                        new[]
                        {
                            Syntax.VariableDeclarator(
                                identifier: "i",
                                initializer: Syntax.EqualsValueClause(Syntax.LiteralExpression(1))
                            )
                        }
                    ),
                    statement: Syntax.Block()
                )
            );
        }
    }
}
