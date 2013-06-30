using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class EnumDeclarationFixture : TestBase
    {
        [Test]
        public void EmptyDeclaration()
        {
            Test(
@"enum Enum
{
}
",
                Syntax.EnumDeclaration(
                    identifier: "Enum"
                )
            );
        }

        [Test]
        public void WithName()
        {
            Test(
@"enum Enum
{
    Name
}
",
                Syntax.EnumDeclaration(
                    identifier: "Enum",
                    members: new[] { Syntax.EnumMemberDeclaration("Name") }
                )
            );
        }

        [Test]
        public void WithMultipleNames()
        {
            Test(
@"enum Enum
{
    Name1,
    Name2
}
",
                Syntax.EnumDeclaration(
                    identifier: "Enum",
                    members: new[]
                    {
                        Syntax.EnumMemberDeclaration("Name1"),
                        Syntax.EnumMemberDeclaration("Name2")
                    }
                )
            );
        }

        [Test]
        public void WithNamesAndValues()
        {
            Test(
@"enum Enum
{
    Name = 1
}
",
                Syntax.EnumDeclaration(
                    identifier: "Enum",
                    members: new[]
                    {
                        Syntax.EnumMemberDeclaration(
                            identifier: "Name",
                            equalsValue: Syntax.EqualsValueClause(Syntax.LiteralExpression(1))
                        )
                    }
                )
            );
        }

        [Test]
        public void WithBaseType()
        {
            Test(
@"enum Enum : short
{
}
",
                Syntax.EnumDeclaration(
                    identifier: "Enum",
                    baseList: Syntax.BaseList(Syntax.ParseName("short"))
                )
            );
        }

        [Test]
        public void WithModifier()
        {
            Test(
@"internal enum Enum : short
{
}
",
                Syntax.EnumDeclaration(
                    identifier: "Enum",
                    baseList: Syntax.BaseList(Syntax.ParseName("short")),
                    modifiers: Modifiers.Internal
                )
            );
        }
    }
}
