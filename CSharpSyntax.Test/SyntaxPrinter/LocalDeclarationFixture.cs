using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class LocalDeclarationFixture : TestBase
    {
        [Test]
        public void Declaration()
        {
            Test(
                @"int i;
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
                @"const int i;
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
                @"const int i;
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
                @"int i = 5;
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
                @"int i = 5, j, k = 7;
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
