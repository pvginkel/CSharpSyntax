using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class BlankLinesFixture : TestBase
    {
        [TestCase(
            0,
@"// Before 1

// Before 2
namespace Namespace1
{
}
// After 2
// Before 3
namespace Namespace2
{
}
// After 3
// After 1
"
        )]
        [TestCase(
            1,
@"// Before 1

// Before 2
namespace Namespace1
{
}
// After 2

// Before 3
namespace Namespace2
{
}
// After 3
// After 1
"
        )]
        [TestCase(
            2,
@"// Before 1

// Before 2
namespace Namespace1
{
}
// After 2


// Before 3
namespace Namespace2
{
}
// After 3
// After 1
"
        )]
        public void AroundNamespace(int blankLines, string expected)
        {
            Test(
                expected,
                Syntax.CompilationUnit(
                    members: new[]
                    {
                        Syntax.NamespaceDeclaration((NameSyntax)Syntax.ParseName("Namespace1")),
                        Syntax.NamespaceDeclaration((NameSyntax)Syntax.ParseName("Namespace2"))
                    }
                ),
                p => p.BlankLines.AroundNamespace = blankLines
            );
        }

        [TestCase(
            0,
@"// Before 1

// Before 3
using System;
// After 3
// Before 2
namespace Namespace1
{
}
// After 2
// After 1
"
        )]
        [TestCase(
            1,
@"// Before 1

// Before 3
using System;
// After 3

// Before 2
namespace Namespace1
{
}
// After 2
// After 1
"
        )]
        [TestCase(
            2,
@"// Before 1

// Before 3
using System;
// After 3


// Before 2
namespace Namespace1
{
}
// After 2
// After 1
"
        )]
        public void AfterUsingList(int blankLines, string expected)
        {
            Test(
                expected,
                Syntax.CompilationUnit(
                    usings: new[] { Syntax.UsingDirective((NameSyntax)Syntax.ParseName("System")) },
                    members: new[]
                    {
                        Syntax.NamespaceDeclaration((NameSyntax)Syntax.ParseName("Namespace1"))
                    }
                ),
                p => p.BlankLines.AfterUsingList = blankLines
            );
        }

        [TestCase(
            0,
            0,
@"// Before 1

// Before 2
namespace Namespace1
{
}
// After 2
// Before 3
class Class1
{
}
// After 3
// Before 4
class Class2
{
}
// After 4
// Before 5
namespace Namespace2
{
}
// After 5
// After 1
"
        )]
        [TestCase(
            1,
            0,
@"// Before 1

// Before 2
namespace Namespace1
{
}
// After 2

// Before 3
class Class1
{
}
// After 3
// Before 4
class Class2
{
}
// After 4

// Before 5
namespace Namespace2
{
}
// After 5
// After 1
"
        )]
        [TestCase(
            0,
            1,
@"// Before 1

// Before 2
namespace Namespace1
{
}
// After 2

// Before 3
class Class1
{
}
// After 3

// Before 4
class Class2
{
}
// After 4

// Before 5
namespace Namespace2
{
}
// After 5
// After 1
"
        )]
        [TestCase(
            2,
            1,
@"// Before 1

// Before 2
namespace Namespace1
{
}
// After 2


// Before 3
class Class1
{
}
// After 3

// Before 4
class Class2
{
}
// After 4


// Before 5
namespace Namespace2
{
}
// After 5
// After 1
"
        )]
        public void AroundType(int aroundNamespace, int aroundType, string expected)
        {
            Test(
                expected,
                Syntax.CompilationUnit(
                    members: new MemberDeclarationSyntax[]
                    {
                        Syntax.NamespaceDeclaration((NameSyntax)Syntax.ParseName("Namespace1")),
                        Syntax.ClassDeclaration("Class1"),
                        Syntax.ClassDeclaration("Class2"),
                        Syntax.NamespaceDeclaration((NameSyntax)Syntax.ParseName("Namespace2"))
                    }
                ),
                p =>
                {
                    p.BlankLines.AroundNamespace = aroundNamespace;
                    p.BlankLines.AroundType = aroundType;
                }
            );
        }

        [TestCase(
            0,
@"// Before 1

// Before 2
using System;
// After 2
// Before 3
using System.Collections;
// After 3
// Before 4
using System.Collections.Generic;
// After 4
// Before 5
using Namespace;
// After 5
// After 1
"
        )]
        [TestCase(
            1,
@"// Before 1

// Before 2
using System;
// After 2
// Before 3
using System.Collections;
// After 3
// Before 4
using System.Collections.Generic;
// After 4

// Before 5
using Namespace;
// After 5
// After 1
"
        )]
        [TestCase(
            2,
@"// Before 1

// Before 2
using System;
// After 2
// Before 3
using System.Collections;
// After 3
// Before 4
using System.Collections.Generic;
// After 4


// Before 5
using Namespace;
// After 5
// After 1
"
        )]
        public void AroundDifferentUsingGroups(int blankLines, string expected)
        {
            Test(
                expected,
                Syntax.CompilationUnit(
                    usings: new[]
                    {
                        Syntax.UsingDirective((NameSyntax)Syntax.ParseName("System")),
                        Syntax.UsingDirective((NameSyntax)Syntax.ParseName("System.Collections")),
                        Syntax.UsingDirective((NameSyntax)Syntax.ParseName("System.Collections.Generic")),
                        Syntax.UsingDirective((NameSyntax)Syntax.ParseName("Namespace"))
                    }
                ),
                p => p.BlankLines.BetweenDifferentUsingGroups = blankLines
            );
        }

        [TestCase(
            0,
@"// Before 1

// Before 2
namespace Namespace
{
    // Before 3
    class Class
    {
    }
    // After 3
}
// After 2
// After 1
"
        )]
        [TestCase(
            1,
@"// Before 1

// Before 2
namespace Namespace
{

    // Before 3
    class Class
    {
    }
    // After 3

}
// After 2
// After 1
"
        )]
        [TestCase(
            2,
@"// Before 1

// Before 2
namespace Namespace
{


    // Before 3
    class Class
    {
    }
    // After 3


}
// After 2
// After 1
"
        )]
        public void InsideNamespace(int blankLines, string expected)
        {
            Test(
                expected,
                Syntax.CompilationUnit(
                    members: new[]
                    {
                        Syntax.NamespaceDeclaration(
                            (NameSyntax)Syntax.ParseName("Namespace"),
                            members: new[] { Syntax.ClassDeclaration("Class") }
                        )
                    }
                ),
                p => p.BlankLines.InsideNamespace = blankLines
            );
        }

        [TestCase(
            0,
@"// Before 1
class Class
{
    // Before 2
    void Method()
    {
    }
    // After 2
}
// After 1
"
        )]
        [TestCase(
            1,
@"// Before 1
class Class
{

    // Before 2
    void Method()
    {
    }
    // After 2

}
// After 1
"
        )]
        [TestCase(
            2,
@"// Before 1
class Class
{


    // Before 2
    void Method()
    {
    }
    // After 2


}
// After 1
"
        )]
        public void InsideType(int blankLines, string expected)
        {
            Test(
                expected,
                Syntax.ClassDeclaration(
                    identifier: "Class",
                    members: new[]
                    {
                        Syntax.MethodDeclaration(
                            Syntax.ParseName("void"),
                            "Method",
                            Syntax.ParameterList(),
                            Syntax.Block()
                        )
                    }
                ),
                p => p.BlankLines.InsideType = blankLines
            );
        }

        [TestCase(
            0,
@"// Before 1
class Class
{
}
// After 1
"
        )]
        [TestCase(
            1,
@"// Before 1
class Class
{

}
// After 1
"
        )]
        [TestCase(
            2,
@"// Before 1
class Class
{


}
// After 1
"
        )]
        public void InsideTypeWithoutMembers(int blankLines, string expected)
        {
            Test(
                expected,
                Syntax.ClassDeclaration("Class"),
                p => p.BlankLines.InsideType = blankLines
            );
        }

        [TestCase(
            1,
            0,
            0,
@"// Before 1
class Class
{
    // Before 2
    int field;
    // After 2

    // Before 3
    int field;
    // After 3

    // Before 4
    void Method()
    {
    }
    // After 4
    // Before 5
    void Method()
    {
    }
    // After 5
    // Before 6
    class Class
    {
    }
    // After 6
    // Before 7
    class Class
    {
    }
    // After 7
}
// After 1
"
        )]
        [TestCase(
            0,
            1,
            0,
@"// Before 1
class Class
{
    // Before 2
    int field;
    // After 2
    // Before 3
    int field;
    // After 3

    // Before 4
    void Method()
    {
    }
    // After 4

    // Before 5
    void Method()
    {
    }
    // After 5

    // Before 6
    class Class
    {
    }
    // After 6
    // Before 7
    class Class
    {
    }
    // After 7
}
// After 1
"

        )]
        [TestCase(
            0,
            0,
            1,
@"// Before 1
class Class
{
    // Before 2
    int field;
    // After 2
    // Before 3
    int field;
    // After 3
    // Before 4
    void Method()
    {
    }
    // After 4
    // Before 5
    void Method()
    {
    }
    // After 5

    // Before 6
    class Class
    {
    }
    // After 6

    // Before 7
    class Class
    {
    }
    // After 7
}
// After 1
"
        )]
        [TestCase(
            2,
            0,
            0,
@"// Before 1
class Class
{
    // Before 2
    int field;
    // After 2


    // Before 3
    int field;
    // After 3


    // Before 4
    void Method()
    {
    }
    // After 4
    // Before 5
    void Method()
    {
    }
    // After 5
    // Before 6
    class Class
    {
    }
    // After 6
    // Before 7
    class Class
    {
    }
    // After 7
}
// After 1
"
        )]
        [TestCase(
            0,
            2,
            0,
@"// Before 1
class Class
{
    // Before 2
    int field;
    // After 2
    // Before 3
    int field;
    // After 3


    // Before 4
    void Method()
    {
    }
    // After 4


    // Before 5
    void Method()
    {
    }
    // After 5


    // Before 6
    class Class
    {
    }
    // After 6
    // Before 7
    class Class
    {
    }
    // After 7
}
// After 1
"
        )]
        [TestCase(
            0,
            0,
            2,
@"// Before 1
class Class
{
    // Before 2
    int field;
    // After 2
    // Before 3
    int field;
    // After 3
    // Before 4
    void Method()
    {
    }
    // After 4
    // Before 5
    void Method()
    {
    }
    // After 5


    // Before 6
    class Class
    {
    }
    // After 6


    // Before 7
    class Class
    {
    }
    // After 7
}
// After 1
"
        )]
        public void AroundTypeMembers(int aroundField, int aroundMethod, int aroundType, string expected)
        {
            Test(
                expected,
                Syntax.ClassDeclaration(
                    identifier: "Class",
                    members: new MemberDeclarationSyntax[]
                    {
                        Syntax.FieldDeclaration(
                            Syntax.VariableDeclaration(
                                Syntax.ParseName("int"),
                                new[] { Syntax.VariableDeclarator("field") }
                            )
                        ),
                        Syntax.FieldDeclaration(
                            Syntax.VariableDeclaration(
                                Syntax.ParseName("int"),
                                new[] { Syntax.VariableDeclarator("field") }
                            )
                        ),
                        Syntax.MethodDeclaration(
                            Syntax.ParseName("void"),
                            "Method",
                            Syntax.ParameterList(),
                            Syntax.Block()
                        ),
                        Syntax.MethodDeclaration(
                            Syntax.ParseName("void"),
                            "Method",
                            Syntax.ParameterList(),
                            Syntax.Block()
                        ),
                        Syntax.ClassDeclaration("Class"),
                        Syntax.ClassDeclaration("Class")
                    }
                ),
                p =>
                {
                    p.BlankLines.AroundField = aroundField;
                    p.BlankLines.AroundMethod = aroundMethod;
                    p.BlankLines.AroundType = aroundType;
                }
            );
        }

        [TestCase(
            0,
            0,
@"// Before 1
enum Enum
{
    // Before 2
    Name1,
    // After 2
    // Before 3
    Name2
    // After 3
}
// After 1
"
        )]
        [TestCase(
            1,
            0,
@"// Before 1
enum Enum
{

    // Before 2
    Name1,
    // After 2
    // Before 3
    Name2
    // After 3

}
// After 1
"
        )]
        [TestCase(
            0,
            1,
@"// Before 1
enum Enum
{
    // Before 2
    Name1,
    // After 2

    // Before 3
    Name2
    // After 3
}
// After 1
"
        )]
        [TestCase(
            0,
            2,
@"// Before 1
enum Enum
{
    // Before 2
    Name1,
    // After 2


    // Before 3
    Name2
    // After 3
}
// After 1
"
        )]
        public void AroundEnumMembers(int insideType, int aroundEnum, string expected)
        {
            Test(
                expected,
                Syntax.EnumDeclaration(
                    identifier: "Enum",
                    members: new[]
                    {
                        Syntax.EnumMemberDeclaration("Name1"),
                        Syntax.EnumMemberDeclaration("Name2")
                    }
                ),
                p =>
                {
                    p.BlankLines.InsideType = insideType;
                    p.BlankLines.AroundField = aroundEnum;
                }
            );
        }

        [TestCase(
            0,
@"// Before 1
// Before 2
namespace Namespace1
{
}
// After 2
// After 1
"
        )]
        [TestCase(
            1,
@"// Before 1

// Before 2
namespace Namespace1
{
}
// After 2
// After 1
"
        )]
        [TestCase(
            2,
@"// Before 1


// Before 2
namespace Namespace1
{
}
// After 2
// After 1
"
        )]
        public void AfterFileHeaderComment(int blankLines, string expected)
        {
            Test(
                expected,
                Syntax.CompilationUnit(
                    members: new[]
                    {
                        Syntax.NamespaceDeclaration((NameSyntax)Syntax.ParseName("Namespace1"))
                    }
                ),
                p => p.BlankLines.AfterFileHeaderComment = blankLines
            );
        }
    }
}
