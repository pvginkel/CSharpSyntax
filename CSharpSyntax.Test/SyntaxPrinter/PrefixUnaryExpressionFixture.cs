using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class PrefixUnaryExpressionFixture : TestBase
    {
        [TestCase("&1", PrefixUnaryOperator.Ampersand)]
        [TestCase("*1", PrefixUnaryOperator.Asterisk)]
        [TestCase("!1", PrefixUnaryOperator.Exclamation)]
        [TestCase("--1", PrefixUnaryOperator.MinusMinus)]
        [TestCase("-1", PrefixUnaryOperator.Minus)]
        [TestCase("+1", PrefixUnaryOperator.Plus)]
        [TestCase("++1", PrefixUnaryOperator.PlusPlus)]
        [TestCase("~1", PrefixUnaryOperator.Tilde)]
        public void Test(string expected, PrefixUnaryOperator @operator)
        {
            Test(
                expected,
                new PrefixUnaryExpressionSyntax
                {
                    Operator = @operator,
                    Operand = new LiteralExpressionSyntax { Value = 1 }
                }
            );
        }
    }
}
