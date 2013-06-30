using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class LocalDeclarationFixture : TestBase
    {
        [Test]
        public void Declaration()
        {
            Test(
@"// Before 1
int i;
// After 1
",
                Syntax.LocalDeclarationStatement(
                    Syntax.VariableDeclaration(
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
const int i;
// After 1
",
                Syntax.LocalDeclarationStatement(
                    Modifiers.Const,
                    Syntax.VariableDeclaration(
                        Syntax.ParseName("int"),
                        new[] { Syntax.VariableDeclarator("i") }
                    )
                )
            );
        }

        [Test]
        public void ConstDeclaration()
        {
            Test(
@"// Before 1
const int i;
// After 1
",
                Syntax.LocalDeclarationStatement(
                    Modifiers.Const,
                    Syntax.VariableDeclaration(
                        Syntax.ParseName("int"),
                        new[] { Syntax.VariableDeclarator("i") }
                    )
                )
            );
        }

        [Test]
        public void VariableDeclarationWithAssignment()
        {
            Test(
@"// Before 1
int i = 5;
// After 1
",
                Syntax.LocalDeclarationStatement(
                    Syntax.VariableDeclaration(
                        Syntax.ParseName("int"),
                        new[]
                        {
                            Syntax.VariableDeclarator(
                                "i",
                                initializer: Syntax.EqualsValueClause(Syntax.LiteralExpression(5))
                            ),
                        }
                    )
                )
            );
        }

        [Test]
        public void MultipleDeclarations()
        {
            Test(
@"// Before 1
int i = 5, j, k = 7;
// After 1
",
                Syntax.LocalDeclarationStatement(
                    Syntax.VariableDeclaration(
                        Syntax.ParseName("int"),
                        new[]
                        {
                            Syntax.VariableDeclarator(
                                "i",
                                initializer: Syntax.EqualsValueClause(Syntax.LiteralExpression(5))
                            ),
                            Syntax.VariableDeclarator("j"),
                            Syntax.VariableDeclarator(
                                "k",
                                initializer: Syntax.EqualsValueClause(Syntax.LiteralExpression(7))
                            )
                        }
                    )
                )
            );
        }
    }
}
