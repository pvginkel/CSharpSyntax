using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class PostfixUnaryExpressionFixture : TestBase
    {
        [TestCase("1--", PostfixUnaryOperator.MinusMinus)]
        [TestCase("1++", PostfixUnaryOperator.PlusPlus)]
        public void Test(string expected, PostfixUnaryOperator @operator)
        {
            Test(
                expected,
                new PostfixUnaryExpressionSyntax
                {
                    Operator = @operator,
                    Operand = new LiteralExpressionSyntax { Value = 1 }
                }
            );
        }
    }
}
