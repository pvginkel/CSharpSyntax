using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class NamespaceDeclarationFixture : TestBase
    {
        [Test]
        public void Simple()
        {
            Test(
@"namespace Namespace
{
}
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
@"namespace Namespace1
{
    namespace Namespace2
    {
    }
}
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
@"namespace Namespace
{
    using System;
}
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
@"namespace Namespace
{
    using Alias = System;
}
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
@"namespace Namespace
{
    using System;
    using System.Text;
}
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
@"namespace Namespace
{
    extern alias System1;
}
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
@"namespace Namespace
{
    class Class
    {
    }
}
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
@"namespace Namespace
{
    using System;
    using Alias = System;
    extern alias System1;

    class Class
    {
    }
}
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
