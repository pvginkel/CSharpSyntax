using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class IndexerDeclarationFixture : TestBase
    {
        [Test]
        public void WithSingleParameter()
        {
            Test(
@"string this[int index]
{
    get { }
}
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
@"string this[int index, int index]
{
    get { }
}
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
