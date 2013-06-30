using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class OperatorDeclarationFixture : TestBase
    {
        [TestCase(Operator.Ampersand, "&")]
        [TestCase(Operator.Asterisk, "*")]
        [TestCase(Operator.Bar, "|")]
        [TestCase(Operator.Caret, "^")]
        [TestCase(Operator.EqualsEquals, "==")]
        [TestCase(Operator.Exclamation, "!")]
        [TestCase(Operator.ExclamationEquals, "!=")]
        [TestCase(Operator.False, "false")]
        [TestCase(Operator.GreaterThan, ">")]
        [TestCase(Operator.GreaterThanEquals, ">=")]
        [TestCase(Operator.GreaterThanGreaterThan, ">>")]
        [TestCase(Operator.LessThan, "<")]
        [TestCase(Operator.LessThanEquals, "<=")]
        [TestCase(Operator.LessThanLessThan, "<<")]
        [TestCase(Operator.Minus, "-")]
        [TestCase(Operator.MinusMinus, "--")]
        [TestCase(Operator.Percent, "%")]
        [TestCase(Operator.Plus, "+")]
        [TestCase(Operator.PlusPlus, "++")]
        [TestCase(Operator.Slash, "/")]
        [TestCase(Operator.Tilde, "~")]
        [TestCase(Operator.True, "true")]
        public void SimpleOperators(Operator @operator, string keyword)
        {
            Test(
@"public static operator bool " + keyword + @"()
{
}
",
                Syntax.OperatorDeclaration(
                    modifiers: Modifiers.Public | Modifiers.Static,
                    returnType: Syntax.ParseName("bool"),
                    @operator: @operator,
                    parameterList: Syntax.ParameterList(),
                    body: Syntax.Block()
                )
            );
        }
    }
}
