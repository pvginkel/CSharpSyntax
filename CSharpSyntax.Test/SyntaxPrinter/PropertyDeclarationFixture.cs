using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class PropertyDeclarationFixture : TestBase
    {
        [Test]
        public void GetAndSet()
        {
            Test(
@"public int Property
{
    get { }
    set { }
}
",
                new PropertyDeclarationSyntax
                {
                    Identifier = "Property",
                    Type = new PredefinedTypeSyntax { Type = PredefinedType.Int },
                    Modifiers = Modifiers.Public,
                    AccessorList = new AccessorListSyntax
                    {
                        Accessors =
                        {
                            new AccessorDeclarationSyntax
                            {
                                Kind = AccessorDeclarationKind.Get,
                                Body = new BlockSyntax()
                            },
                            new AccessorDeclarationSyntax
                            {
                                Kind = AccessorDeclarationKind.Set,
                                Body = new BlockSyntax()
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void OnlyGet()
        {
            Test(
@"public int Property
{
    get { }
}
",
                new PropertyDeclarationSyntax
                {
                    Identifier = "Property",
                    Type = new PredefinedTypeSyntax { Type = PredefinedType.Int },
                    Modifiers = Modifiers.Public,
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
        public void OnlySet()
        {
            Test(
@"public int Property
{
    set { }
}
",
                new PropertyDeclarationSyntax
                {
                    Identifier = "Property",
                    Type = new PredefinedTypeSyntax { Type = PredefinedType.Int },
                    Modifiers = Modifiers.Public,
                    AccessorList = new AccessorListSyntax
                    {
                        Accessors =
                        {
                            new AccessorDeclarationSyntax
                            {
                                Kind = AccessorDeclarationKind.Set,
                                Body = new BlockSyntax()
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void NoModifier()
        {
            Test(
@"int Property
{
    set { }
}
",
                new PropertyDeclarationSyntax
                {
                    Identifier = "Property",
                    Type = new PredefinedTypeSyntax { Type = PredefinedType.Int },
                    AccessorList = new AccessorListSyntax
                    {
                        Accessors =
                        {
                            new AccessorDeclarationSyntax
                            {
                                Kind = AccessorDeclarationKind.Set,
                                Body = new BlockSyntax()
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void ModifierOnAccessor()
        {
            Test(
@"int Property
{
    private set { }
}
",
                new PropertyDeclarationSyntax
                {
                    Identifier = "Property",
                    Type = new PredefinedTypeSyntax { Type = PredefinedType.Int },
                    AccessorList = new AccessorListSyntax
                    {
                        Accessors =
                        {
                            new AccessorDeclarationSyntax
                            {
                                Modifiers = Modifiers.Private,
                                Kind = AccessorDeclarationKind.Set,
                                Body = new BlockSyntax()
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void WithExplicitInterfaceSpecifier()
        {
            Test(
@"int IInterface.Property
{
    get { }
}
",
                new PropertyDeclarationSyntax
                {
                    Identifier = "Property",
                    Type = new PredefinedTypeSyntax { Type = PredefinedType.Int },
                    ExplicitInterfaceSpecifier = new ExplicitInterfaceSpecifierSyntax
                    {
                        Name = (IdentifierNameSyntax)Syntax.ParseName("IInterface")
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
