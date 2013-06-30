using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class ConversionOperatorDeclarationFixture : TestBase
    {
        [Test]
        public void Explicit()
        {
            Test(
@"// Before 1
public static explicit operator int(int i)
{
}
// After 1
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
@"// Before 1
public static implicit operator int(int i)
{
}
// After 1
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
