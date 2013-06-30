using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class OtherLineBreaksAndWrapping : TestBase
    {
        [TestCase(
            true,
@"// Before 1
string Property
{
    // Before 2
    get;
    // After 2
    // Before 3
    set;
    // After 3
}
// After 1
"
        )]
        [TestCase(
            false,
@"// Before 1
string Property
{
    // Before 2
    get;
    // After 2
    // Before 3
    set;
    // After 3
}
// After 1
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
@"// Before 1
string Property
{
    // Before 2
    get
    {
        // Before 3
        return 1;
        // After 3
    }
    // After 2
    // Before 4
    set
    {
        // Before 5
        return 1;
        // After 5
    }
    // After 4
}
// After 1
// Before 6
string Property
{
    // Before 7
    get
    {
        // Before 8
        break;
        // After 8
        // Before 9
        return 1;
        // After 9
    }
    // After 7
    // Before 10
    set
    {
        // Before 11
        break;
        // After 11
        // Before 12
        return 1;
        // After 12
    }
    // After 10
}
// After 6
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
@"// Before 1
void Method()
{
    // Before 2
    return 1;
    // After 2
}
// After 1
"
        )]
        [TestCase(
            false,
@"// Before 1
void Method()
{
    // Before 2
    return 1;
    // After 2
}
// After 1
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
@"delegate()
{
    // Before 1
    return 1;
    // After 1
}"
        )]
        [TestCase(
            false,
@"delegate()
{
    // Before 1
    return 1;
    // After 1
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

        [Ignore("Not yet implemented")]
        public void PlaceSimpleArrayObjectCollectionOnSingleLine()
        {
        }

        [TestCase(
            true,
@"// Before 1
namespace Namespace
{
    // Before 2
    [Attribute] [Attribute] class Class
    {
    }
    // After 2

    // Before 3
    [Attribute] [Attribute] struct Struct
    {
    }
    // After 3

    // Before 4
    [Attribute] [Attribute] enum Enum
    {
    }
    // After 4
}
// After 1
"
        )]
        [TestCase(
            false,
@"// Before 1
namespace Namespace
{
    // Before 2
    [Attribute]
    [Attribute]
    class Class
    {
    }
    // After 2

    // Before 3
    [Attribute]
    [Attribute]
    struct Struct
    {
    }
    // After 3

    // Before 4
    [Attribute]
    [Attribute]
    enum Enum
    {
    }
    // After 4
}
// After 1
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
@"// Before 1
class Class
{
    // Before 2
    [Attribute] [Attribute] string Property
    {
        // Before 3
        get;
        // After 3
    }
    // After 2

    // Before 4
    [Attribute] [Attribute] event EventHandler Event
    {
        // Before 5
        add;
        // After 5
    }
    // After 4

    // Before 6
    [Attribute] [Attribute] string this[int index]
    {
        // Before 7
        get;
        // After 7
    }
    // After 6
}
// After 1
"
        )]
        [TestCase(
            false,
@"// Before 1
class Class
{
    // Before 2
    [Attribute]
    [Attribute]
    string Property
    {
        // Before 3
        get;
        // After 3
    }
    // After 2

    // Before 4
    [Attribute]
    [Attribute]
    event EventHandler Event
    {
        // Before 5
        add;
        // After 5
    }
    // After 4

    // Before 6
    [Attribute]
    [Attribute]
    string this[int index]
    {
        // Before 7
        get;
        // After 7
    }
    // After 6
}
// After 1
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
    }
}
