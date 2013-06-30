using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class ClassDeclarationFixture : TestBase
    {
        [Test]
        public void SimpleClass()
        {
            Test(
@"class Class
{
}
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
@"struct Class
{
}
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
@"interface Class
{
}
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
@"class Class : Base
{
}
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
@"class Class : Base, IInterface
{
}
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
@"class Class<T>
{
}
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
@"class Class<T, T1>
{
}
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
@"class Class<in T>
{
}
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
@"class Class<out T>
{
}
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
@"class Class<T>
    where T : int
{
}
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
@"class Class<T>
    where T : struct
{
}
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
@"class Class<T>
    where T : class
{
}
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
@"class Class<T>
    where T : new()
{
}
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
@"class Class<T>
    where T : class, int, new()
{
}
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
@"class Class<T>
    where T : struct where T : struct
{
}
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
