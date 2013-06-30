using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class BinaryExpressionFixture : TestBase
    {
        [TestCase("1 & 1", BinaryOperator.Ampersand)]
        [TestCase("1 && 1", BinaryOperator.AmpersandAmpersand)]
        [TestCase("1 &= 1", BinaryOperator.AmpersandEquals)]
        [TestCase("1 as 1", BinaryOperator.AsKeyword)]
        [TestCase("1 * 1", BinaryOperator.Asterisk)]
        [TestCase("1 *= 1", BinaryOperator.AsteriskEquals)]
        [TestCase("1 | 1", BinaryOperator.Bar)]
        [TestCase("1 || 1", BinaryOperator.BarBar)]
        [TestCase("1 |= 1", BinaryOperator.BarEquals)]
        [TestCase("1 ^ 1", BinaryOperator.Caret)]
        [TestCase("1 ^= 1", BinaryOperator.CaretEquals)]
        [TestCase("1 = 1", BinaryOperator.Equals)]
        [TestCase("1 == 1", BinaryOperator.EqualsEquals)]
        [TestCase("1 != 1", BinaryOperator.ExclamationEquals)]
        [TestCase("1 > 1", BinaryOperator.GreaterThan)]
        [TestCase("1 >= 1", BinaryOperator.GreaterThanEquals)]
        [TestCase("1 >> 1", BinaryOperator.GreaterThanGreaterThan)]
        [TestCase("1 >>= 1", BinaryOperator.GreaterThanGreaterThanEquals)]
        [TestCase("1 is 1", BinaryOperator.IsKeyword)]
        [TestCase("1 < 1", BinaryOperator.LessThan)]
        [TestCase("1 <= 1", BinaryOperator.LessThanEquals)]
        [TestCase("1 << 1", BinaryOperator.LessThanLessThan)]
        [TestCase("1 <<= 1", BinaryOperator.LessThanLessThanEquals)]
        [TestCase("1 - 1", BinaryOperator.Minus)]
        [TestCase("1 -= 1", BinaryOperator.MinusEquals)]
        [TestCase("1 % 1", BinaryOperator.Percent)]
        [TestCase("1 %= 1", BinaryOperator.PercentEquals)]
        [TestCase("1 + 1", BinaryOperator.Plus)]
        [TestCase("1 += 1", BinaryOperator.PlusEquals)]
        [TestCase("1 ?? 1", BinaryOperator.QuestionQuestion)]
        [TestCase("1 / 1", BinaryOperator.Slash)]
        [TestCase("1 /= 1", BinaryOperator.SlashEquals)]
        public void Test(string expected, BinaryOperator @operator)
        {
            Test(
                expected,
                new BinaryExpressionSyntax
                {
                    Left = new LiteralExpressionSyntax { Value = 1 },
                    Right = new LiteralExpressionSyntax { Value = 1 },
                    Operator = @operator
                }
            );
        }
    }
}
