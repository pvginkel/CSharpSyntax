using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class PropertyDeclarationFixture : TestBase
    {
        [Test]
        public void GetAndSet()
        {
            Test(
@"// Before 1
public int Property
{
    // Before 2
    get { }
    // After 2
    // Before 3
    set { }
    // After 3
}
// After 1
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
@"// Before 1
public int Property
{
    // Before 2
    get { }
    // After 2
}
// After 1
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
@"// Before 1
public int Property
{
    // Before 2
    set { }
    // After 2
}
// After 1
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
@"// Before 1
int Property
{
    // Before 2
    set { }
    // After 2
}
// After 1
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
@"// Before 1
int Property
{
    // Before 2
    private set { }
    // After 2
}
// After 1
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
@"// Before 1
int IInterface.Property
{
    // Before 2
    get { }
    // After 2
}
// After 1
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
