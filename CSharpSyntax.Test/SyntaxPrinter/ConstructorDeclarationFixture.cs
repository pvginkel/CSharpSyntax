using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class ConstructorDeclarationFixture : TestBase
    {
        [Test]
        public void SimpleConstructor()
        {
            Test(
@"public Class()
{
}
",
                new ConstructorDeclarationSyntax
                {
                    Modifiers = Modifiers.Public,
                    Identifier = "Class",
                    Body = new BlockSyntax(),
                    ParameterList = new ParameterListSyntax()
                }
            );
        }

        [Test]
        public void SimpleDestructor()
        {
            Test(
@"~Class()
{
}
",
                new DestructorDeclarationSyntax
                {
                    Identifier = "Class",
                    Body = new BlockSyntax(),
                    ParameterList = new ParameterListSyntax()
                }
            );
        }

        [Test]
        public void WithParameter()
        {
            Test(
@"public Class(int i)
{
}
",
                new ConstructorDeclarationSyntax
                {
                    Modifiers = Modifiers.Public,
                    Identifier = "Class",
                    Body = new BlockSyntax(),
                    ParameterList = new ParameterListSyntax()
                    {
                        Parameters =
                        {
                            new ParameterSyntax
                            {
                                Identifier = "i",
                                Type = Syntax.ParseName("int")
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void WithThisInitializer()
        {
            Test(
@"public Class()
    : this(1)
{
}
",
                new ConstructorDeclarationSyntax
                {
                    Modifiers = Modifiers.Public,
                    Identifier = "Class",
                    Body = new BlockSyntax(),
                    ParameterList = new ParameterListSyntax(),
                    Initializer = new ConstructorInitializerSyntax
                    {
                        Kind = ThisOrBase.This,
                        ArgumentList = new ArgumentListSyntax
                        {
                            Arguments =
                            {
                                new ArgumentSyntax
                                {
                                    Expression = new LiteralExpressionSyntax { Value = 1 }
                                }
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void WithBaseInitializer()
        {
            Test(
@"public Class()
    : base(1)
{
}
",
                new ConstructorDeclarationSyntax
                {
                    Modifiers = Modifiers.Public,
                    Identifier = "Class",
                    Body = new BlockSyntax(),
                    ParameterList = new ParameterListSyntax(),
                    Initializer = new ConstructorInitializerSyntax
                    {
                        Kind = ThisOrBase.Base,
                        ArgumentList = new ArgumentListSyntax
                        {
                            Arguments =
                            {
                                new ArgumentSyntax
                                {
                                    Expression = new LiteralExpressionSyntax { Value = 1 }
                                }
                            }
                        }
                    }
                }
            );
        }
    }
}
