using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class CompilationUnitFixture : TestBase
    {
        [Test]
        public void Simple()
        {
            Test(
@"",
                Syntax.CompilationUnit()
            );
        }

        [Test]
        public void SingleUsing()
        {
            Test(
@"using System;
",
                Syntax.CompilationUnit(
                    usings: new[] { Syntax.UsingDirective((NameSyntax)Syntax.ParseName("System")) }
                )
            );
        }

        [Test]
        public void AliasedUsing()
        {
            Test(
@"using Alias = System;
",
                Syntax.CompilationUnit(
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
@"using System;
using System.Text;
",
                Syntax.CompilationUnit(
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
@"extern alias System1;
",
                Syntax.CompilationUnit(
                    externs: new[] { Syntax.ExternAliasDirective("System1") }
                )
            );
        }

        [Test]
        public void WithType()
        {
            Test(
@"class Class
{
}
",
                Syntax.CompilationUnit(
                    members: new[] { Syntax.ClassDeclaration("Class") }
                )
            );
        }

        [Test]
        public void WithAll()
        {
            Test(
@"using System;
using Alias = System;
extern alias System1;

[assembly: Attribute]

class Class
{
}
",
                Syntax.CompilationUnit(
                    usings: new[]
                    {
                        Syntax.UsingDirective((NameSyntax)Syntax.ParseName("System")),
                        Syntax.UsingDirective(
                            name: (NameSyntax)Syntax.ParseName("System"),
                            alias: Syntax.NameEquals("Alias")
                        )
                    },
                    externs: new[] { Syntax.ExternAliasDirective("System1") },
                    attributeLists: new[]
                    {
                        Syntax.AttributeList(
                            AttributeTarget.Assembly,
                            new[]
                            {
                                Syntax.Attribute((NameSyntax)Syntax.ParseName("Attribute"))
                            }
                        )
                    },
                    members: new[] { Syntax.ClassDeclaration("Class") }
                )
            );
        }
    }
}
