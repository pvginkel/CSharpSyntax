using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class IndexerDeclarationFixture : TestBase
    {
        [Test]
        public void WithSingleParameter()
        {
            Test(
@"// Before 1
string this[int index]
{
    // Before 2
    get { }
    // After 2
}
// After 1
",
                new IndexerDeclarationSyntax
                {
                    Type = Syntax.ParseName("string"),
                    ParameterList = new BracketedParameterListSyntax
                    {
                        Parameters =
                        {
                            new ParameterSyntax
                            {
                                Identifier = "index",
                                Type = Syntax.ParseName("int")
                            }
                        }
                    },
                    AccessorList = new AccessorListSyntax
                    {
                        Accessors =
                        {
                            new AccessorDeclarationSyntax
                            {
                                Kind = AccessorDeclarationKind.Get,
                                Body = new BlockSyntax()
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void WithMultipleParameters()
        {
            Test(
@"// Before 1
string this[int index, int index]
{
    // Before 2
    get { }
    // After 2
}
// After 1
",
                new IndexerDeclarationSyntax
                {
                    Type = Syntax.ParseName("string"),
                    ParameterList = new BracketedParameterListSyntax
                    {
                        Parameters =
                        {
                            new ParameterSyntax
                            {
                                Identifier = "index",
                                Type = Syntax.ParseName("int")
                            },
                            new ParameterSyntax
                            {
                                Identifier = "index",
                                Type = Syntax.ParseName("int")
                            }
                        }
                    },
                    AccessorList = new AccessorListSyntax
                    {
                        Accessors =
                        {
                            new AccessorDeclarationSyntax
                            {
                                Kind = AccessorDeclarationKind.Get,
                                Body = new BlockSyntax()
                            }
                        }
                    }
                }
            );
        }
    }
}
