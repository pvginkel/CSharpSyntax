using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class FieldDeclarationFixture : TestBase
    {
        [Test]
        public void SimpleField()
        {
            Test(
@"int i;
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
@"private int i;
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
@"private const int i;
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
