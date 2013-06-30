using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterConfiguration
{
    [TestFixture]
    public class BlankLinesFixture : TestBase
    {
        [TestCase(
            0,
@"namespace Namespace1
{
}
namespace Namespace2
{
}
"
        )]
        [TestCase(
            1,
@"namespace Namespace1
{
}

namespace Namespace2
{
}
"
        )]
        [TestCase(
            2,
@"namespace Namespace1
{
}


namespace Namespace2
{
}
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
@"using System;
namespace Namespace1
{
}
"
        )]
        [TestCase(
            1,
@"using System;

namespace Namespace1
{
}
"
        )]
        [TestCase(
            2,
@"using System;


namespace Namespace1
{
}
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
@"namespace Namespace1
{
}
class Class1
{
}
class Class2
{
}
namespace Namespace2
{
}
"
        )]
        [TestCase(
            1,
            0,
@"namespace Namespace1
{
}

class Class1
{
}
class Class2
{
}

namespace Namespace2
{
}
"
        )]
        [TestCase(
            0,
            1,
@"namespace Namespace1
{
}

class Class1
{
}

class Class2
{
}

namespace Namespace2
{
}
"
        )]
        [TestCase(
            2,
            1,
@"namespace Namespace1
{
}


class Class1
{
}

class Class2
{
}


namespace Namespace2
{
}
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
@"using System;
using System.Collections;
using System.Collections.Generic;
using Namespace;
"
        )]
        [TestCase(
            1,
@"using System;
using System.Collections;
using System.Collections.Generic;

using Namespace;
"
        )]
        [TestCase(
            2,
@"using System;
using System.Collections;
using System.Collections.Generic;


using Namespace;
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
@"namespace Namespace
{
    class Class
    {
    }
}
"
        )]
        [TestCase(
            1,
@"namespace Namespace
{

    class Class
    {
    }

}
"
        )]
        [TestCase(
            2,
@"namespace Namespace
{


    class Class
    {
    }


}
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
@"class Class
{
    void Method()
    {
    }
}
"
        )]
        [TestCase(
            1,
@"class Class
{

    void Method()
    {
    }

}
"
        )]
        [TestCase(
            2,
@"class Class
{


    void Method()
    {
    }


}
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
@"class Class
{
}
"
        )]
        [TestCase(
            1,
@"class Class
{

}
"
        )]
        [TestCase(
            2,
@"class Class
{


}
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
@"class Class
{
    int field;

    int field;

    void Method()
    {
    }
    void Method()
    {
    }
    class Class
    {
    }
    class Class
    {
    }
}
"
        )]
        [TestCase(
            0,
            1,
            0,
@"class Class
{
    int field;
    int field;

    void Method()
    {
    }

    void Method()
    {
    }

    class Class
    {
    }
    class Class
    {
    }
}
"
        )]
        [TestCase(
            0,
            0,
            1,
@"class Class
{
    int field;
    int field;
    void Method()
    {
    }
    void Method()
    {
    }

    class Class
    {
    }

    class Class
    {
    }
}
"
        )]
        [TestCase(
            2,
            0,
            0,
@"class Class
{
    int field;


    int field;


    void Method()
    {
    }
    void Method()
    {
    }
    class Class
    {
    }
    class Class
    {
    }
}
"
        )]
        [TestCase(
            0,
            2,
            0,
@"class Class
{
    int field;
    int field;


    void Method()
    {
    }


    void Method()
    {
    }


    class Class
    {
    }
    class Class
    {
    }
}
"
        )]
        [TestCase(
            0,
            0,
            2,
@"class Class
{
    int field;
    int field;
    void Method()
    {
    }
    void Method()
    {
    }


    class Class
    {
    }


    class Class
    {
    }
}
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
@"enum Enum
{
    Name1,
    Name2
}
"
        )]
        [TestCase(
            1,
            0,
@"enum Enum
{

    Name1,
    Name2

}
"
        )]
        [TestCase(
            0,
            1,
@"enum Enum
{
    Name1,

    Name2
}
"
        )]
        [TestCase(
            0,
            2,
@"enum Enum
{
    Name1,


    Name2
}
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
    }
}
