using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class ForStatementFixture : TestBase
    {
        [Test]
        public void EmptyFor()
        {
            Test(
@"for (; ; )
{
}
",
                Syntax.ForStatement(Syntax.Block())
            );
        }

        [Test]
        public void WithVariableDeclaration()
        {
            Test(
@"for (int i; ; )
{
}
",
                Syntax.ForStatement(
                    declaration: Syntax.VariableDeclaration(
                        Syntax.ParseName("int"),
                        new[] { Syntax.VariableDeclarator("i") }
                    ),
                    statement: Syntax.Block()
                )
            );
        }

        [Test]
        public void WithInitializer()
        {
            Test(
@"for (1; ; )
{
}
",
                Syntax.ForStatement(
                    initializers: new[] { Syntax.LiteralExpression(1) },
                    statement: Syntax.Block()
                )
            );
        }

        [Test]
        public void WithMultipleInitializers()
        {
            Test(
@"for (1, 2, 3; ; )
{
}
",
                Syntax.ForStatement(
                    initializers: new[]
                    {
                        Syntax.LiteralExpression(1),
                        Syntax.LiteralExpression(2),
                        Syntax.LiteralExpression(3)
                    },
                    statement: Syntax.Block()
                )
            );
        }

        [Test]
        public void WithCondition()
        {
            Test(
@"for (; 1; )
{
}
",
                Syntax.ForStatement(
                    condition: Syntax.LiteralExpression(1),
                    statement: Syntax.Block()
                )
            );
        }

        [Test]
        public void WithIncrementor()
        {
            Test(
@"for (; ; 1)
{
}
",
                Syntax.ForStatement(
                    incrementors: new[] { Syntax.LiteralExpression(1) },
                    statement: Syntax.Block()
                )
            );
        }

        [Test]
        public void WithMultipleIncrementors()
        {
            Test(
@"for (; ; 1, 2, 3)
{
}
",
                Syntax.ForStatement(
                    incrementors: new[]
                    {
                        Syntax.LiteralExpression(1),
                        Syntax.LiteralExpression(2),
                        Syntax.LiteralExpression(3)
                    },
                    statement: Syntax.Block()
                )
            );
        }

        [Test]
        public void WithAll()
        {
            Test(
@"for (1, 2, 3; 1; 1, 2, 3)
{
}
",
                Syntax.ForStatement(
                    initializers: new[]
                    {
                        Syntax.LiteralExpression(1),
                        Syntax.LiteralExpression(2),
                        Syntax.LiteralExpression(3)
                    },
                    condition: Syntax.LiteralExpression(1),
                    incrementors: new[]
                    {
                        Syntax.LiteralExpression(1),
                        Syntax.LiteralExpression(2),
                        Syntax.LiteralExpression(3)
                    },
                    statement: Syntax.Block()
                )
            );
        }
    }
}
