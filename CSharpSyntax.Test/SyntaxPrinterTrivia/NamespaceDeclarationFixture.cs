using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class NamespaceDeclarationFixture : TestBase
    {
        [Test]
        public void Simple()
        {
            Test(
@"// Before 1
namespace Namespace
{
}
// After 1
",
                Syntax.NamespaceDeclaration(
                    name: (NameSyntax)Syntax.ParseName("Namespace")
                )
            );
        }

        [Test]
        public void NestedNamespace()
        {
            Test(
@"// Before 1
namespace Namespace1
{
    // Before 2
    namespace Namespace2
    {
    }
    // After 2
}
// After 1
",
                Syntax.NamespaceDeclaration(
                    name: (NameSyntax)Syntax.ParseName("Namespace1"),
                    members: new[]
                    {
                        Syntax.NamespaceDeclaration(
                            name: (NameSyntax)Syntax.ParseName("Namespace2")
                        )
                    }
                )
            );
        }

        [Test]
        public void SingleUsing()
        {
            Test(
@"// Before 1
namespace Namespace
{
    // Before 2
    using System;
    // After 2
}
// After 1
",
                Syntax.NamespaceDeclaration(
                    name: (NameSyntax)Syntax.ParseName("Namespace"),
                    usings: new[] { Syntax.UsingDirective((NameSyntax)Syntax.ParseName("System")) }
                )
            );
        }

        [Test]
        public void AliasedUsing()
        {
            Test(
@"// Before 1
namespace Namespace
{
    // Before 2
    using Alias = System;
    // After 2
}
// After 1
",
                Syntax.NamespaceDeclaration(
                    name: (NameSyntax)Syntax.ParseName("Namespace"),
                    usings: new[]
                    {
                        Syntax.UsingDirective(
                            name: (NameSyntax)Syntax.ParseName("System"),
                            alias: Syntax.NameEquals("Alias")
                        )
                    }
                )
            );
        }

        [Test]
        public void MultipleUsings()
        {
            Test(
@"// Before 1
namespace Namespace
{
    // Before 2
    using System;
    // After 2
    // Before 3
    using System.Text;
    // After 3
}
// After 1
",
                Syntax.NamespaceDeclaration(
                    name: (NameSyntax)Syntax.ParseName("Namespace"),
                    usings: new[]
                    {
                        Syntax.UsingDirective((NameSyntax)Syntax.ParseName("System")),
                        Syntax.UsingDirective((NameSyntax)Syntax.ParseName("System.Text"))
                    }
                )
            );
        }

        [Test]
        public void SingleExtern()
        {
            Test(
@"// Before 1
namespace Namespace
{
    // Before 2
    extern alias System1;
    // After 2
}
// After 1
",
                Syntax.NamespaceDeclaration(
                    name: (NameSyntax)Syntax.ParseName("Namespace"),
                    externs: new[] { Syntax.ExternAliasDirective("System1") }
                )
            );
        }

        [Test]
        public void WithType()
        {
            Test(
@"// Before 1
namespace Namespace
{
    // Before 2
    class Class
    {
    }
    // After 2
}
// After 1
",
                Syntax.NamespaceDeclaration(
                    name: (NameSyntax)Syntax.ParseName("Namespace"),
                    members: new[] { Syntax.ClassDeclaration("Class") }
                )
            );
        }

        [Test]
        public void WithAll()
        {
            Test(
@"// Before 1
namespace Namespace
{
    // Before 4
    using System;
    // After 4
    // Before 5
    using Alias = System;
    // After 5
    // Before 2
    extern alias System1;
    // After 2

    // Before 3
    class Class
    {
    }
    // After 3
}
// After 1
",
                Syntax.NamespaceDeclaration(
                    name: (NameSyntax)Syntax.ParseName("Namespace"),
                    usings: new[]
                    {
                        Syntax.UsingDirective((NameSyntax)Syntax.ParseName("System")),
                        Syntax.UsingDirective(
                            name: (NameSyntax)Syntax.ParseName("System"),
                            alias: Syntax.NameEquals("Alias")
                        )
                    },
                    externs: new[] { Syntax.ExternAliasDirective("System1") },
                    members: new[] { Syntax.ClassDeclaration("Class") }
                )
            );
        }
    }
}
