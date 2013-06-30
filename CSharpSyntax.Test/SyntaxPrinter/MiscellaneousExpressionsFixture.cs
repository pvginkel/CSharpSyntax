using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class MiscellaneousExpressionsFixture : TestBase
    {
        [Test]
        public void Base()
        {
            Test("base", Syntax.BaseExpression());
        }

        [Test]
        public void This()
        {
            Test("this", Syntax.ThisExpression());
        }

        [Test]
        public void Checked()
        {
            Test(
                "checked (7)",
                new CheckedExpressionSyntax
                {
                    Kind = CheckedOrUnchecked.Checked,
                    Expression = Syntax.LiteralExpression(7)
                }
            );
        }

        [Test]
        public void Unchecked()
        {
            Test(
                "unchecked (7)",
                new CheckedExpressionSyntax
                {
                    Kind = CheckedOrUnchecked.Unchecked,
                    Expression = Syntax.LiteralExpression(7)
                }
            );
        }

        [Test]
        public void Conditional()
        {
            Test(
@"0 ? 1 : 2",
                new ConditionalExpressionSyntax
                {
                    Condition = new LiteralExpressionSyntax { Value = 0 },
                    WhenTrue = new LiteralExpressionSyntax { Value = 1 },
                    WhenFalse = new LiteralExpressionSyntax { Value = 2 }
                }
            );
        }

        [Test]
        public void Default()
        {
            Test("default(int)", new DefaultExpressionSyntax { Type = Syntax.ParseName("int") });
        }

        [Test]
        public void Parenthesized()
        {
            Test(
                "(1)",
                Syntax.ParenthesizedExpression(Syntax.LiteralExpression(1))
            );
        }

        [Test]
        public void SizeOf()
        {
            Test(
                "sizeof(int)",
                Syntax.SizeOfExpression(Syntax.ParseName("int"))
            );
        }

        [Test]
        public void TypeOf()
        {
            Test(
                "typeof(int)",
                Syntax.TypeOfExpression(Syntax.ParseName("int"))
            );
        }
    }
}
