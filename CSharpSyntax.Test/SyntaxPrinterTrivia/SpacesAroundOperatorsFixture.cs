using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class SpacesAroundOperatorsFixture : TestBase
    {
        [TestCase(BinaryOperator.AmpersandEquals, true, @"a &= 1")]
        [TestCase(BinaryOperator.AmpersandEquals, false, @"a&=1")]
        [TestCase(BinaryOperator.AsteriskEquals, true, @"a *= 1")]
        [TestCase(BinaryOperator.AsteriskEquals, false, @"a*=1")]
        [TestCase(BinaryOperator.BarEquals, true, @"a |= 1")]
        [TestCase(BinaryOperator.BarEquals, false, @"a|=1")]
        [TestCase(BinaryOperator.CaretEquals, true, @"a ^= 1")]
        [TestCase(BinaryOperator.CaretEquals, false, @"a^=1")]
        [TestCase(BinaryOperator.Equals, true, @"a = 1")]
        [TestCase(BinaryOperator.Equals, false, @"a=1")]
        [TestCase(BinaryOperator.GreaterThanGreaterThanEquals, true, @"a >>= 1")]
        [TestCase(BinaryOperator.GreaterThanGreaterThanEquals, false, @"a>>=1")]
        [TestCase(BinaryOperator.LessThanLessThanEquals, true, @"a <<= 1")]
        [TestCase(BinaryOperator.LessThanLessThanEquals, false, @"a<<=1")]
        [TestCase(BinaryOperator.MinusEquals, true, @"a -= 1")]
        [TestCase(BinaryOperator.MinusEquals, false, @"a-=1")]
        [TestCase(BinaryOperator.PercentEquals, true, @"a %= 1")]
        [TestCase(BinaryOperator.PercentEquals, false, @"a%=1")]
        [TestCase(BinaryOperator.PlusEquals, true, @"a += 1")]
        [TestCase(BinaryOperator.PlusEquals, false, @"a+=1")]
        [TestCase(BinaryOperator.SlashEquals, true, @"a /= 1")]
        [TestCase(BinaryOperator.SlashEquals, false, @"a/=1")]
        public void AssignmentOperators(BinaryOperator @operator, bool space, string expected)
        {
            Test(
                expected,
                Syntax.BinaryExpression(
                    @operator,
                    Syntax.ParseName("a"),
                    Syntax.LiteralExpression(1)
                ),
                p => p.Spaces.AroundOperators.AssignmentOperators = space
            );
        }

        [TestCase(BinaryOperator.AmpersandAmpersand, true, @"a && 1")]
        [TestCase(BinaryOperator.AmpersandAmpersand, false, @"a&&1")]
        [TestCase(BinaryOperator.BarBar, true, @"a || 1")]
        [TestCase(BinaryOperator.BarBar, false, @"a||1")]
        public void BeforeTypeParameterListAngle(BinaryOperator @operator, bool space, string expected)
        {
            Test(
                expected,
                Syntax.BinaryExpression(
                    @operator,
                    Syntax.ParseName("a"),
                    Syntax.LiteralExpression(1)
                ),
                p => p.Spaces.AroundOperators.LogicalOperators = space
            );
        }

        [TestCase(BinaryOperator.EqualsEquals, true, @"a == 1")]
        [TestCase(BinaryOperator.EqualsEquals, false, @"a==1")]
        [TestCase(BinaryOperator.ExclamationEquals, true, @"a != 1")]
        [TestCase(BinaryOperator.ExclamationEquals, false, @"a!=1")]
        public void EqualityOperators(BinaryOperator @operator, bool space, string expected)
        {
            Test(
                expected,
                Syntax.BinaryExpression(
                    @operator,
                    Syntax.ParseName("a"),
                    Syntax.LiteralExpression(1)
                ),
                p => p.Spaces.AroundOperators.EqualityOperators = space
            );
        }

        [TestCase(BinaryOperator.GreaterThan, true, @"a > 1")]
        [TestCase(BinaryOperator.GreaterThan, false, @"a>1")]
        [TestCase(BinaryOperator.GreaterThanEquals, true, @"a >= 1")]
        [TestCase(BinaryOperator.GreaterThanEquals, false, @"a>=1")]
        [TestCase(BinaryOperator.LessThan, true, @"a < 1")]
        [TestCase(BinaryOperator.LessThan, false, @"a<1")]
        [TestCase(BinaryOperator.LessThanEquals, true, @"a <= 1")]
        [TestCase(BinaryOperator.LessThanEquals, false, @"a<=1")]
        public void RelationalOperators(BinaryOperator @operator, bool space, string expected)
        {
            Test(
                expected,
                Syntax.BinaryExpression(
                    @operator,
                    Syntax.ParseName("a"),
                    Syntax.LiteralExpression(1)
                ),
                p => p.Spaces.AroundOperators.RelationalOperators = space
            );
        }

        [TestCase(BinaryOperator.Ampersand, true, @"a & 1")]
        [TestCase(BinaryOperator.Ampersand, false, @"a&1")]
        [TestCase(BinaryOperator.Bar, true, @"a | 1")]
        [TestCase(BinaryOperator.Bar, false, @"a|1")]
        [TestCase(BinaryOperator.Caret, true, @"a ^ 1")]
        [TestCase(BinaryOperator.Caret, false, @"a^1")]
        public void BitwiseOperators(BinaryOperator @operator, bool space, string expected)
        {
            Test(
                expected,
                Syntax.BinaryExpression(
                    @operator,
                    Syntax.ParseName("a"),
                    Syntax.LiteralExpression(1)
                ),
                p => p.Spaces.AroundOperators.BitwiseOperators = space
            );
        }

        [TestCase(BinaryOperator.Plus, true, @"a + 1")]
        [TestCase(BinaryOperator.Plus, false, @"a+1")]
        [TestCase(BinaryOperator.Minus, true, @"a - 1")]
        [TestCase(BinaryOperator.Minus, false, @"a-1")]
        public void AdditiveOperators(BinaryOperator @operator, bool space, string expected)
        {
            Test(
                expected,
                Syntax.BinaryExpression(
                    @operator,
                    Syntax.ParseName("a"),
                    Syntax.LiteralExpression(1)
                ),
                p => p.Spaces.AroundOperators.AdditiveOperators = space
            );
        }

        [TestCase(BinaryOperator.Asterisk, true, @"a * 1")]
        [TestCase(BinaryOperator.Asterisk, false, @"a*1")]
        [TestCase(BinaryOperator.Slash, true, @"a / 1")]
        [TestCase(BinaryOperator.Slash, false, @"a/1")]
        [TestCase(BinaryOperator.Percent, true, @"a % 1")]
        [TestCase(BinaryOperator.Percent, false, @"a%1")]
        public void MultiplicativeOperators(BinaryOperator @operator, bool space, string expected)
        {
            Test(
                expected,
                Syntax.BinaryExpression(
                    @operator,
                    Syntax.ParseName("a"),
                    Syntax.LiteralExpression(1)
                ),
                p => p.Spaces.AroundOperators.MultiplicativeOperators = space
            );
        }

        [TestCase(BinaryOperator.GreaterThanGreaterThan, true, @"a >> 1")]
        [TestCase(BinaryOperator.GreaterThanGreaterThan, false, @"a>>1")]
        [TestCase(BinaryOperator.LessThanLessThan, true, @"a << 1")]
        [TestCase(BinaryOperator.LessThanLessThan, false, @"a<<1")]
        public void ShiftOperators(BinaryOperator @operator, bool space, string expected)
        {
            Test(
                expected,
                Syntax.BinaryExpression(
                    @operator,
                    Syntax.ParseName("a"),
                    Syntax.LiteralExpression(1)
                ),
                p => p.Spaces.AroundOperators.ShiftOperators = space
            );
        }

        [TestCase(BinaryOperator.QuestionQuestion, true, @"a ?? 1")]
        [TestCase(BinaryOperator.QuestionQuestion, false, @"a??1")]
        public void NullCoalescingOperator(BinaryOperator @operator, bool space, string expected)
        {
            Test(
                expected,
                Syntax.BinaryExpression(
                    @operator,
                    Syntax.ParseName("a"),
                    Syntax.LiteralExpression(1)
                ),
                p => p.Spaces.AroundOperators.NullCoalescingOperator = space
            );
        }
    }
}
