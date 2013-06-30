using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterConfiguration
{
    [TestFixture]
    public class OtherLineBreaksAndWrapping : TestBase
    {
        [TestCase(
            true,
@"string Property { get; set; }
"
        )]
        [TestCase(
            false,
@"string Property
{
    get;
    set;
}
"
        )]
        public void PlaceAbstractAutoPropertyIndexerEventDeclarationOnSingleLine(bool apply, string expected)
        {
            Test(
                expected,
                Syntax.PropertyDeclaration(
                    type: Syntax.ParseName("string"),
                    identifier: "Property",
                    accessorList: Syntax.AccessorList(
                        Syntax.AccessorDeclaration(
                            AccessorDeclarationKind.Get,
                            null
                        ),
                        Syntax.AccessorDeclaration(
                            AccessorDeclarationKind.Set,
                            null
                        )
                    )
                ),
                p => p.LineBreaksAndWrapping.Other.PlaceAbstractAutoPropertyIndexerEventDeclarationOnSingleLine = apply
            );
        }

        [TestCase(
            true,
            true,
@"string Property { get { return 1; } set { return 1; } }
string Property
{
    get
    {
        break;
        return 1;
    }
    set
    {
        break;
        return 1;
    }
}
"
        )]
        [TestCase(
            false,
            true,
@"string Property { get { return 1; } set { return 1; } }
string Property
{
    get
    {
        break;
        return 1;
    }
    set
    {
        break;
        return 1;
    }
}
"
        )]
        [TestCase(
            true,
            false,
@"string Property
{
    get { return 1; }
    set { return 1; }
}
string Property
{
    get
    {
        break;
        return 1;
    }
    set
    {
        break;
        return 1;
    }
}
"
        )]
        [TestCase(
            false,
            false,
@"string Property
{
    get
    {
        return 1;
    }
    set
    {
        return 1;
    }
}
string Property
{
    get
    {
        break;
        return 1;
    }
    set
    {
        break;
        return 1;
    }
}
"
        )]
        public void PlaceSimpleAccessorOnSingleLine(bool simpleOnSingleLine, bool simpleDeclarationOnSingleLine, string expected)
        {
            Test(
                expected,
                new[]
                {
                    Syntax.PropertyDeclaration(
                        type: Syntax.ParseName("string"),
                        identifier: "Property",
                        accessorList: Syntax.AccessorList(
                            Syntax.AccessorDeclaration(
                                AccessorDeclarationKind.Get,
                                Syntax.Block(Syntax.ReturnStatement(Syntax.LiteralExpression(1)))
                            ),
                            Syntax.AccessorDeclaration(
                                AccessorDeclarationKind.Set,
                                Syntax.Block(Syntax.ReturnStatement(Syntax.LiteralExpression(1)))
                            )
                        )
                    ),
                    Syntax.PropertyDeclaration(
                        type: Syntax.ParseName("string"),
                        identifier: "Property",
                        accessorList: Syntax.AccessorList(
                            Syntax.AccessorDeclaration(
                                AccessorDeclarationKind.Get,
                                Syntax.Block(
                                    Syntax.BreakStatement(),
                                    Syntax.ReturnStatement(Syntax.LiteralExpression(1))
                                )
                            ),
                            Syntax.AccessorDeclaration(
                                AccessorDeclarationKind.Set,
                                Syntax.Block(
                                    Syntax.BreakStatement(),
                                    Syntax.ReturnStatement(Syntax.LiteralExpression(1))
                                )
                            )
                        )
                    )
                },
                p =>
                {
                    p.LineBreaksAndWrapping.Other.PlaceSimpleAccessorOnSingleLine = simpleOnSingleLine;
                    p.LineBreaksAndWrapping.Other.PlaceSimplePropertyIndexerEventDeclarationOnSingleLine = simpleDeclarationOnSingleLine;
                }
            );
        }

        [TestCase(
            true,
@"void Method() { return 1; }
"
        )]
        [TestCase(
            false,
@"void Method()
{
    return 1;
}
"
        )]
        public void PlaceSimpleMethodOnSingleLine(bool apply, string expected)
        {
            Test(
                expected,
                Syntax.MethodDeclaration(
                    Syntax.ParseName("void"),
                    "Method",
                    Syntax.ParameterList(),
                    Syntax.Block(Syntax.ReturnStatement(Syntax.LiteralExpression(1)))
                ),
                p => p.LineBreaksAndWrapping.Other.PlaceSimpleMethodOnSingleLine = apply
            );
        }

        [TestCase(
            true,
@"delegate() { return 1; }"
        )]
        [TestCase(
            false,
@"delegate()
{
    return 1;
}"
        )]
        public void PlaceSimpleAnonymousMethodOnSingleLine(bool apply, string expected)
        {
            Test(
                expected,
                Syntax.AnonymousMethodExpression(
                    block: Syntax.Block(Syntax.ReturnStatement(Syntax.LiteralExpression(1))),
                    parameterList: Syntax.ParameterList()
                ),
                p => p.LineBreaksAndWrapping.Other.PlaceSimpleAnonymousMethodOnSingleLine = apply
            );
        }

        [TestCase(
            true,
@"Constructor() : this()
{
}
"
        )]
        [TestCase(
            false,
@"Constructor()
    : this()
{
}
"
        )]
        public void PlaceConstructorInitializerOnSameLine(bool apply, string expected)
        {
            Test(
                expected,
                Syntax.ConstructorDeclaration(
                    identifier: "Constructor",
                    parameterList: Syntax.ParameterList(),
                    initializer: Syntax.ConstructorInitializer(ThisOrBase.This, Syntax.ArgumentList()),
                    body: Syntax.Block()
                ),
                p => p.LineBreaksAndWrapping.Other.PlaceConstructorInitializerOnSameLine = apply
            );
        }

        [TestCase(
            true,
@"class Class where T : class
{
    void Method() where T : class
    {
    }

    delegate void Delegate() where T : class where T : int;
}
"
        )]
        [TestCase(
            false,
@"class Class
    where T : class
{
    void Method()
        where T : class
    {
    }

    delegate void Delegate()
        where T : class where T : int;
}
"
        )]
        public void PlaceTypeConstraintsOnSameLine(bool apply, string expected)
        {
            Test(
                expected,
                Syntax.ClassDeclaration(
                    identifier: "Class",
                    constraintClauses: new[]
                    {
                        Syntax.TypeParameterConstraintClause(
                            (IdentifierNameSyntax)Syntax.ParseName("T"),
                            new[] { Syntax.ClassOrStructConstraint() }
                        )
                    },
                    members: new MemberDeclarationSyntax[]
                    {
                        Syntax.MethodDeclaration(
                            returnType: Syntax.ParseName("void"),
                            identifier: "Method",
                            parameterList: Syntax.ParameterList(),
                            body: Syntax.Block(),
                            constraintClauses: new[]
                            {
                                Syntax.TypeParameterConstraintClause(
                                    (IdentifierNameSyntax)Syntax.ParseName("T"),
                                    new[] { Syntax.ClassOrStructConstraint() }
                                )
                            }
                        ),
                        Syntax.DelegateDeclaration(
                            returnType: Syntax.ParseName("void"),
                            identifier: "Delegate",
                            parameterList: Syntax.ParameterList(),
                            constraintClauses: new[]
                            {
                                Syntax.TypeParameterConstraintClause(
                                    (IdentifierNameSyntax)Syntax.ParseName("T"),
                                    new[] { Syntax.ClassOrStructConstraint() }
                                ),
                                Syntax.TypeParameterConstraintClause(
                                    (IdentifierNameSyntax)Syntax.ParseName("T"),
                                    new[] { Syntax.TypeConstraint(Syntax.ParseName("int")) }
                                )
                            }
                        )
                    }
                ),
                p => p.LineBreaksAndWrapping.Other.PlaceTypeConstraintsOnSameLine = apply
            );
        }

        [TestCase(
            false,
@"var i = from i in j from i in j join k in l on i equals k into m select i;
"
        )]
        [TestCase(
            true,
@"var i = from i in j
    from i in j
    join k in l on i equals k into m
    select i;
"
        )]
        public void PlaceLinqExpressionOnSingleLine(bool placeOnNewLine, string expected)
        {
            Test(
                expected,
                Syntax.LocalDeclarationStatement(
                    Syntax.VariableDeclaration(
                        Syntax.ParseName("var"),
                        new[]
                        {
                            Syntax.VariableDeclarator(
                                "i",
                                null,
                                Syntax.EqualsValueClause(
                                    Syntax.QueryExpression(
                                        Syntax.FromClause("i", Syntax.ParseName("j")),
                                        Syntax.QueryBody(
                                            new QueryClauseSyntax[]
                                            {
                                                Syntax.FromClause("i", Syntax.ParseName("j")),
                                                Syntax.JoinClause(
                                                    "k",
                                                    Syntax.ParseName("l"),
                                                    Syntax.ParseName("i"),
                                                    Syntax.ParseName("k"),
                                                    Syntax.JoinIntoClause("m")
                                                )
                                            },
                                            Syntax.SelectClause(Syntax.ParseName("i"))
                                        )
                                    )
                                )
                            )
                        }
                    )
                ),
                p => p.LineBreaksAndWrapping.Other.PlaceLinqExpressionOnSingleLine = !placeOnNewLine
            );
        }

        [Ignore("Not yet implemented")]
        public void PlaceSimpleArrayObjectCollectionOnSingleLine()
        {
        }

        [TestCase(
            true,
@"namespace Namespace
{
    [Attribute] [Attribute] class Class
    {
    }

    [Attribute] [Attribute] struct Struct
    {
    }

    [Attribute] [Attribute] enum Enum
    {
    }
}
"
        )]
        [TestCase(
            false,
@"namespace Namespace
{
    [Attribute]
    [Attribute]
    class Class
    {
    }

    [Attribute]
    [Attribute]
    struct Struct
    {
    }

    [Attribute]
    [Attribute]
    enum Enum
    {
    }
}
"
        )]
        public void PlaceTypeAttributeOnSingleLine(bool apply, string expected)
        {
            Test(
                expected,
                Syntax.NamespaceDeclaration(
                    (NameSyntax)Syntax.ParseName("Namespace"),
                    members: new MemberDeclarationSyntax[]
                    {
                        Syntax.ClassDeclaration(
                            identifier: "Class",
                            attributeLists: CreateTwoAttributes()
                        ),
                        Syntax.StructDeclaration(
                            identifier: "Struct",
                            attributeLists: CreateTwoAttributes()
                        ),
                        Syntax.EnumDeclaration(
                            identifier: "Enum",
                            attributeLists: CreateTwoAttributes()
                        )
                    }
                ),
                p => p.LineBreaksAndWrapping.Other.PlaceTypeAttributeOnSingleLine = apply
            );
        }

        private static IEnumerable<AttributeListSyntax> CreateTwoAttributes()
        {
            return new[]
            {
                Syntax.AttributeList(Syntax.Attribute((NameSyntax)Syntax.ParseName("Attribute"))),
                Syntax.AttributeList(Syntax.Attribute((NameSyntax)Syntax.ParseName("Attribute")))
            };
        }

        [TestCase(
            true,
@"class Class
{
    string Property
    {
        [Attribute] [Attribute] get { }
    }

    [Attribute] [Attribute] Constructor()
    {
    }

    [Attribute] [Attribute] ~Destructor()
    {
    }

    [Attribute] [Attribute] implicit operator int()
    {
    }

    [Attribute] [Attribute] operator int +()
    {
    }

    [Attribute] [Attribute] delegate void Delegate();

    [Attribute] [Attribute] void Method()
    {
    }
}
"
        )]
        [TestCase(
            false,
@"class Class
{
    string Property
    {
        [Attribute]
        [Attribute]
        get { }
    }

    [Attribute]
    [Attribute]
    Constructor()
    {
    }

    [Attribute]
    [Attribute]
    ~Destructor()
    {
    }

    [Attribute]
    [Attribute]
    implicit operator int()
    {
    }

    [Attribute]
    [Attribute]
    operator int +()
    {
    }

    [Attribute]
    [Attribute]
    delegate void Delegate();

    [Attribute]
    [Attribute]
    void Method()
    {
    }
}
"
        )]
        public void PlaceMethodAttributeOnSameLine(bool apply, string expected)
        {
            Test(
                expected,
                Syntax.ClassDeclaration(
                    identifier: "Class",
                    members: new MemberDeclarationSyntax[]
                    {
                        Syntax.PropertyDeclaration(
                            type: Syntax.ParseName("string"),
                            identifier: "Property",
                            accessorList: Syntax.AccessorList(
                                Syntax.AccessorDeclaration(
                                    AccessorDeclarationKind.Get,
                                    body: Syntax.Block(),
                                    attributeLists: CreateTwoAttributes()
                                )
                            )
                        ),
                        Syntax.ConstructorDeclaration(
                            identifier: "Constructor",
                            parameterList: Syntax.ParameterList(),
                            attributeLists: CreateTwoAttributes(),
                            body: Syntax.Block()
                        ),
                        Syntax.DestructorDeclaration(
                            identifier: "Destructor",
                            parameterList: Syntax.ParameterList(),
                            attributeLists: CreateTwoAttributes(),
                            body: Syntax.Block()
                        ),
                        Syntax.ConversionOperatorDeclaration(
                            kind: ImplicitOrExplicit.Implicit,
                            type: Syntax.ParseName("int"),
                            parameterList: Syntax.ParameterList(),
                            body: Syntax.Block(),
                            attributeLists: CreateTwoAttributes()
                        ),
                        Syntax.OperatorDeclaration(
                            returnType: Syntax.ParseName("int"),
                            @operator: Operator.Plus,
                            parameterList: Syntax.ParameterList(),
                            body: Syntax.Block(),
                            attributeLists: CreateTwoAttributes()
                        ),
                        Syntax.DelegateDeclaration(
                            returnType: Syntax.ParseName("void"),
                            identifier: "Delegate",
                            parameterList: Syntax.ParameterList(),
                            attributeLists: CreateTwoAttributes()
                        ),
                        Syntax.MethodDeclaration(
                            returnType: Syntax.ParseName("void"),
                            identifier: "Method",
                            body: Syntax.Block(),
                            parameterList: Syntax.ParameterList(),
                            attributeLists: CreateTwoAttributes()
                        )
                    }
                ),
                p => p.LineBreaksAndWrapping.Other.PlaceMethodAttributeOnSameLine = apply
            );
        }

        [TestCase(
            true,
@"class Class
{
    [Attribute] [Attribute] string Property { get; }

    [Attribute] [Attribute] event EventHandler Event { add; }

    [Attribute] [Attribute] string this[int index] { get; }
}
"
        )]
        [TestCase(
            false,
@"class Class
{
    [Attribute]
    [Attribute]
    string Property { get; }

    [Attribute]
    [Attribute]
    event EventHandler Event { add; }

    [Attribute]
    [Attribute]
    string this[int index] { get; }
}
"
        )]
        public void PlacePropertyIndexerEventAttributeOnSameLine(bool apply, string expected)
        {
            Test(
                expected,
                Syntax.ClassDeclaration(
                    identifier: "Class",
                    members: new MemberDeclarationSyntax[]
                    {
                        Syntax.PropertyDeclaration(
                            type: Syntax.ParseName("string"),
                            identifier: "Property",
                            accessorList: Syntax.AccessorList(
                                Syntax.AccessorDeclaration(
                                    AccessorDeclarationKind.Get,
                                    null
                                )
                            ),
                            attributeLists: CreateTwoAttributes()
                        ),
                        Syntax.EventDeclaration(
                            type: Syntax.ParseName("EventHandler"),
                            identifier: "Event",
                            accessorList: Syntax.AccessorList(
                                Syntax.AccessorDeclaration(
                                    AccessorDeclarationKind.Add,
                                    null
                                )
                            ),
                            attributeLists: CreateTwoAttributes()
                        ),
                        Syntax.IndexerDeclaration(
                            type: Syntax.ParseName("string"),
                            parameterList: Syntax.BracketedParameterList(
                                Syntax.Parameter(
                                    type: Syntax.ParseName("int"),
                                    identifier: "index"
                                )
                            ),
                            accessorList: Syntax.AccessorList(
                                Syntax.AccessorDeclaration(
                                    AccessorDeclarationKind.Get,
                                    null
                                )
                            ),
                            attributeLists: CreateTwoAttributes()
                        )
                    }
                ),
                p => p.LineBreaksAndWrapping.Other.PlacePropertyIndexerEventAttributeOnSameLine = apply
            );
        }

        [TestCase(
            true,
@"namespace Namespace
{
    class Class
    {
        [Attribute] [Attribute] int field;

        [Attribute] [Attribute] event EventHandler Event;
    }

    enum Enum
    {
        [Attribute] [Attribute] Name
    }
}
"
        )]
        [TestCase(
            false,
@"namespace Namespace
{
    class Class
    {
        [Attribute]
        [Attribute]
        int field;

        [Attribute]
        [Attribute]
        event EventHandler Event;
    }

    enum Enum
    {
        [Attribute]
        [Attribute]
        Name
    }
}
"
        )]
        public void PlaceFieldAttributeOnSameLine(bool apply, string expected)
        {
            Test(
                expected,
                Syntax.NamespaceDeclaration(
                    name: (NameSyntax)Syntax.ParseName("Namespace"),
                    members: new MemberDeclarationSyntax[]
                    {
                        Syntax.ClassDeclaration(
                            identifier: "Class",
                            members: new MemberDeclarationSyntax[]
                            {
                                Syntax.FieldDeclaration(
                                    declaration: Syntax.VariableDeclaration(
                                        Syntax.ParseName("int"),
                                        new[] { Syntax.VariableDeclarator("field") }
                                    ),
                                    attributeLists: CreateTwoAttributes()
                                ),
                                Syntax.EventFieldDeclaration(
                                    declaration: Syntax.VariableDeclaration(
                                        Syntax.ParseName("EventHandler"),
                                        new[] { Syntax.VariableDeclarator("Event") }
                                    ),
                                    attributeLists: CreateTwoAttributes()
                                )
                            }
                        ),
                        Syntax.EnumDeclaration(
                            identifier: "Enum",
                            members: new[]
                            {
                                Syntax.EnumMemberDeclaration(
                                    identifier: "Name",
                                    attributeLists: CreateTwoAttributes()
                                )
                            }
                        )
                    }
                ),
                p => p.LineBreaksAndWrapping.Other.PlaceFieldAttributeOnSameLine = apply
            );
        }

        [TestCase(
            true,
            true,
@"class Class
{
    string Property { [Attribute] [Attribute] get; }

    string Property { [Attribute] [Attribute] get { } }

    string Property { [Attribute] [Attribute] get { return 1; } }

    string Property
    {
        [Attribute] [Attribute] get
        {
            break;
            return 1;
        }
    }
}
"
        )]
        [TestCase(
            false,
            false,
@"class Class
{
    string Property
    {
        [Attribute]
        [Attribute]
        get;
    }

    string Property
    {
        [Attribute]
        [Attribute]
        get { }
    }

    string Property
    {
        [Attribute]
        [Attribute]
        get { return 1; }
    }

    string Property
    {
        [Attribute]
        [Attribute]
        get
        {
            break;
            return 1;
        }
    }
}
"
        )]
        public void PlaceSingleMultipleLineAccessorAttributeOnSameLine(bool singleLine, bool multipleLine, string expected)
        {
            Test(
                expected,
                Syntax.ClassDeclaration(
                    identifier: "Class",
                    members: new[]
                    {
                        Syntax.PropertyDeclaration(
                            type: Syntax.ParseName("string"),
                            identifier: "Property",
                            accessorList: Syntax.AccessorList(
                                Syntax.AccessorDeclaration(
                                    kind: AccessorDeclarationKind.Get,
                                    attributeLists: CreateTwoAttributes()
                                )
                            )
                        ),
                        Syntax.PropertyDeclaration(
                            type: Syntax.ParseName("string"),
                            identifier: "Property",
                            accessorList: Syntax.AccessorList(
                                Syntax.AccessorDeclaration(
                                    kind: AccessorDeclarationKind.Get,
                                    attributeLists: CreateTwoAttributes(),
                                    body: Syntax.Block()
                                )
                            )
                        ),
                        Syntax.PropertyDeclaration(
                            type: Syntax.ParseName("string"),
                            identifier: "Property",
                            accessorList: Syntax.AccessorList(
                                Syntax.AccessorDeclaration(
                                    kind: AccessorDeclarationKind.Get,
                                    attributeLists: CreateTwoAttributes(),
                                    body: Syntax.Block(Syntax.ReturnStatement(Syntax.LiteralExpression(1)))
                                )
                            )
                        ),
                        Syntax.PropertyDeclaration(
                            type: Syntax.ParseName("string"),
                            identifier: "Property",
                            accessorList: Syntax.AccessorList(
                                Syntax.AccessorDeclaration(
                                    kind: AccessorDeclarationKind.Get,
                                    attributeLists: CreateTwoAttributes(),
                                    body: Syntax.Block(
                                        Syntax.BreakStatement(),
                                        Syntax.ReturnStatement(Syntax.LiteralExpression(1))
                                    )
                                )
                            )
                        ),
                    }
                ),
                p =>
                {
                    p.LineBreaksAndWrapping.Other.PlaceSimpleAccessorOnSingleLine = true;
                    p.LineBreaksAndWrapping.Other.PlaceSimplePropertyIndexerEventDeclarationOnSingleLine = true;
                    p.LineBreaksAndWrapping.Other.PlaceAbstractAutoPropertyIndexerEventDeclarationOnSingleLine = true;
                    p.LineBreaksAndWrapping.Other.PlaceSingleLineAccessorAttributeOnSameLine = singleLine;
                    p.LineBreaksAndWrapping.Other.PlaceMultiLineAccessorAttributeOnSameLine = multipleLine;
                }
            );
        }
    }
}
