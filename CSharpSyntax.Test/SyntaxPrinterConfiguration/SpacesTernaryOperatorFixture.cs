using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterConfiguration
{
    [TestFixture]
    public class SpacesTernaryOperatorFixture : TestBase
    {
        [TestCase(true, true, true, true, @"1 ? 1 : 1")]
        [TestCase(false, true, true, true, @"1? 1 : 1")]
        [TestCase(true, false, true, true, @"1 ?1 : 1")]
        [TestCase(true, true, false, true, @"1 ? 1: 1")]
        [TestCase(true, true, true, false, @"1 ? 1 :1")]
        public void TypeParameterAngles(bool beforeQuestionMark, bool afterQuestionMark, bool beforeColon, bool afterColon, string expected)
        {
            Test(
                expected,
                Syntax.ConditionalExpression(
                    Syntax.LiteralExpression(1),
                    Syntax.LiteralExpression(1),
                    Syntax.LiteralExpression(1)
                ),
                p =>
                {
                    p.Spaces.TernaryOperator.BeforeQuestionMark = beforeQuestionMark;
                    p.Spaces.TernaryOperator.AfterQuestionMark = afterQuestionMark;
                    p.Spaces.TernaryOperator.BeforeColon = beforeColon;
                    p.Spaces.TernaryOperator.AfterColon = afterColon;
                }
            );
        }
    }
}
