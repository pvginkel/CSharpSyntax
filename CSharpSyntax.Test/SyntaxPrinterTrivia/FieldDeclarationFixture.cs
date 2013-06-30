using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class FieldDeclarationFixture : TestBase
    {
        [Test]
        public void SimpleField()
        {
            Test(
@"// Before 1
int i;
// After 1
",
                Syntax.FieldDeclaration(
                    Syntax.VariableDeclaration(
                        Syntax.ParseName("int"),
                        new[] { Syntax.VariableDeclarator("i") }
                    )
                )
            );
        }

        [Test]
        public void WithModifier()
        {
            Test(
@"// Before 1
private int i;
// After 1
",
                Syntax.FieldDeclaration(
                    modifiers: Modifiers.Private,
                    declaration: Syntax.VariableDeclaration(
                        Syntax.ParseName("int"),
                        new[] { Syntax.VariableDeclarator("i") }
                    )
                )
            );
        }

        [Test]
        public void Const()
        {
            Test(
@"// Before 1
private const int i;
// After 1
",
                Syntax.FieldDeclaration(
                    modifiers: Modifiers.Private | Modifiers.Const,
                    declaration: Syntax.VariableDeclaration(
                        Syntax.ParseName("int"),
                        new[] { Syntax.VariableDeclarator("i") }
                    )
                )
            );
        }
    }
}
