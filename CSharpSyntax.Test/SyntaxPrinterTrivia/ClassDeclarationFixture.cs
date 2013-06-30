using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class ClassDeclarationFixture : TestBase
    {
        [Test]
        public void SimpleClass()
        {
            Test(
@"// Before 1
class Class
{
}
// After 1
",
                new ClassDeclarationSyntax
                {
                    Identifier = "Class"
                }
            );
        }

        [Test]
        public void SimpleStruct()
        {
            Test(
@"// Before 1
struct Class
{
}
// After 1
",
                new StructDeclarationSyntax
                {
                    Identifier = "Class"
                }
            );
        }

        [Test]
        public void SimpleInterface()
        {
            Test(
@"// Before 1
interface Class
{
}
// After 1
",
                new InterfaceDeclarationSyntax
                {
                    Identifier = "Class"
                }
            );
        }

        [Test]
        public void WithBaseClass()
        {
            Test(
@"// Before 1
class Class : Base
{
}
// After 1
",
                new ClassDeclarationSyntax
                {
                    Identifier = "Class",
                    BaseList = new BaseListSyntax
                    {
                        Types =
                        {
                            Syntax.ParseName("Base")
                        }
                    }
                }
            );
        }

        [Test]
        public void WithMultipleBaseTypes()
        {
            Test(
@"// Before 1
class Class : Base, IInterface
{
}
// After 1
",
                new ClassDeclarationSyntax
                {
                    Identifier = "Class",
                    BaseList = new BaseListSyntax
                    {
                        Types =
                        {
                            Syntax.ParseName("Base"),
                            Syntax.ParseName("IInterface")
                        }
                    }
                }
            );
        }

        [Test]
        public void WithTypeParameter()
        {
            Test(
@"// Before 1
class Class<T>
{
}
// After 1
",
                new ClassDeclarationSyntax
                {
                    Identifier = "Class",
                    TypeParameterList = new TypeParameterListSyntax
                    {
                        Parameters =
                        {
                            new TypeParameterSyntax
                            {
                                Identifier = "T"
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void WithMultipleTypeParameters()
        {
            Test(
@"// Before 1
class Class<T, T1>
{
}
// After 1
",
                new ClassDeclarationSyntax
                {
                    Identifier = "Class",
                    TypeParameterList = new TypeParameterListSyntax
                    {
                        Parameters =
                        {
                            new TypeParameterSyntax
                            {
                                Identifier = "T"
                            },
                            new TypeParameterSyntax
                            {
                                Identifier = "T1"
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void WithTypeInParameters()
        {
            Test(
@"// Before 1
class Class<in T>
{
}
// After 1
",
                new ClassDeclarationSyntax
                {
                    Identifier = "Class",
                    TypeParameterList = new TypeParameterListSyntax
                    {
                        Parameters =
                        {
                            new TypeParameterSyntax
                            {
                                Identifier = "T",
                                Variance = Variance.In
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void WithTypeOutParameters()
        {
            Test(
@"// Before 1
class Class<out T>
{
}
// After 1
",
                new ClassDeclarationSyntax
                {
                    Identifier = "Class",
                    TypeParameterList = new TypeParameterListSyntax
                    {
                        Parameters =
                        {
                            new TypeParameterSyntax
                            {
                                Identifier = "T",
                                Variance = Variance.Out
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void WithTypeConstraints()
        {
            Test(
@"// Before 1
class Class<T>
    where T : int
{
}
// After 1
",
                new ClassDeclarationSyntax
                {
                    Identifier = "Class",
                    TypeParameterList = new TypeParameterListSyntax
                    {
                        Parameters =
                        {
                            new TypeParameterSyntax
                            {
                                Identifier = "T"
                            }
                        }
                    },
                    ConstraintClauses =
                    {
                        new TypeParameterConstraintClauseSyntax
                        {
                            Name = (IdentifierNameSyntax)Syntax.ParseName("T"),
                            Constraints =
                            {
                                new TypeConstraintSyntax
                                {
                                    Type = Syntax.ParseName("int")
                                }
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void WithStructConstraints()
        {
            Test(
@"// Before 1
class Class<T>
    where T : struct
{
}
// After 1
",
                new ClassDeclarationSyntax
                {
                    Identifier = "Class",
                    TypeParameterList = new TypeParameterListSyntax
                    {
                        Parameters =
                        {
                            new TypeParameterSyntax
                            {
                                Identifier = "T"
                            }
                        }
                    },
                    ConstraintClauses =
                    {
                        new TypeParameterConstraintClauseSyntax
                        {
                            Name = (IdentifierNameSyntax)Syntax.ParseName("T"),
                            Constraints =
                            {
                                new ClassOrStructConstraintSyntax
                                {
                                    Kind = ClassOrStruct.Struct
                                }
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void WithClassConstraints()
        {
            Test(
@"// Before 1
class Class<T>
    where T : class
{
}
// After 1
",
                new ClassDeclarationSyntax
                {
                    Identifier = "Class",
                    TypeParameterList = new TypeParameterListSyntax
                    {
                        Parameters =
                        {
                            new TypeParameterSyntax
                            {
                                Identifier = "T"
                            }
                        }
                    },
                    ConstraintClauses =
                    {
                        new TypeParameterConstraintClauseSyntax
                        {
                            Name = (IdentifierNameSyntax)Syntax.ParseName("T"),
                            Constraints =
                            {
                                new ClassOrStructConstraintSyntax
                                {
                                    Kind = ClassOrStruct.Class
                                }
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void WithConstructorConstraints()
        {
            Test(
@"// Before 1
class Class<T>
    where T : new()
{
}
// After 1
",
                new ClassDeclarationSyntax
                {
                    Identifier = "Class",
                    TypeParameterList = new TypeParameterListSyntax
                    {
                        Parameters =
                        {
                            new TypeParameterSyntax
                            {
                                Identifier = "T"
                            }
                        }
                    },
                    ConstraintClauses =
                    {
                        new TypeParameterConstraintClauseSyntax
                        {
                            Name = (IdentifierNameSyntax)Syntax.ParseName("T"),
                            Constraints =
                            {
                                new ConstructorConstraintSyntax()
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void WithMultipleConstraints()
        {
            Test(
@"// Before 1
class Class<T>
    where T : class, int, new()
{
}
// After 1
",
                new ClassDeclarationSyntax
                {
                    Identifier = "Class",
                    TypeParameterList = new TypeParameterListSyntax
                    {
                        Parameters =
                        {
                            new TypeParameterSyntax
                            {
                                Identifier = "T"
                            }
                        }
                    },
                    ConstraintClauses =
                    {
                        new TypeParameterConstraintClauseSyntax
                        {
                            Name = (IdentifierNameSyntax)Syntax.ParseName("T"),
                            Constraints =
                            {
                                new ClassOrStructConstraintSyntax
                                {
                                    Kind = ClassOrStruct.Class
                                },
                                new TypeConstraintSyntax
                                {
                                    Type = Syntax.ParseName("int")
                                },
                                new ConstructorConstraintSyntax()
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void WithStructConstraintClausess()
        {
            Test(
@"// Before 1
class Class<T>
    where T : struct where T : struct
{
}
// After 1
",
                new ClassDeclarationSyntax
                {
                    Identifier = "Class",
                    TypeParameterList = new TypeParameterListSyntax
                    {
                        Parameters =
                        {
                            new TypeParameterSyntax
                            {
                                Identifier = "T"
                            }
                        }
                    },
                    ConstraintClauses =
                    {
                        new TypeParameterConstraintClauseSyntax
                        {
                            Name = (IdentifierNameSyntax)Syntax.ParseName("T"),
                            Constraints =
                            {
                                new ClassOrStructConstraintSyntax
                                {
                                    Kind = ClassOrStruct.Struct
                                }
                            }
                        },
                        new TypeParameterConstraintClauseSyntax
                        {
                            Name = (IdentifierNameSyntax)Syntax.ParseName("T"),
                            Constraints =
                            {
                                new ClassOrStructConstraintSyntax
                                {
                                    Kind = ClassOrStruct.Struct
                                }
                            }
                        }
                    }
                }
            );
        }
    }
}
