using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class ConversionOperatorDeclarationFixture : TestBase
    {
        [Test]
        public void Explicit()
        {
            Test(
@"public static explicit operator int(int i)
{
}
",
                new ConversionOperatorDeclarationSyntax
                {
                    Kind = ImplicitOrExplicit.Explicit,
                    Body = new BlockSyntax(),
                    Modifiers = Modifiers.Public | Modifiers.Static,
                    ParameterList = new ParameterListSyntax
                    {
                        Parameters =
                        {
                            new ParameterSyntax
                            {
                                Identifier = "i",
                                Type = Syntax.ParseName("int")
                            }
                        }
                    },
                    Type = Syntax.ParseName("int")
                }
            );
        }

        [Test]
        public void Implicit()
        {
            Test(
@"public static implicit operator int(int i)
{
}
",
                new ConversionOperatorDeclarationSyntax
                {
                    Kind = ImplicitOrExplicit.Implicit,
                    Body = new BlockSyntax(),
                    Modifiers = Modifiers.Public | Modifiers.Static,
                    ParameterList = new ParameterListSyntax
                    {
                        Parameters =
                        {
                            new ParameterSyntax
                            {
                                Identifier = "i",
                                Type = Syntax.ParseName("int")
                            }
                        }
                    },
                    Type = Syntax.ParseName("int")
                }
            );
        }
    }
}
