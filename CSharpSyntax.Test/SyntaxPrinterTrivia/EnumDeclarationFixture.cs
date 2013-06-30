using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class EnumDeclarationFixture : TestBase
    {
        [Test]
        public void EmptyDeclaration()
        {
            Test(
@"// Before 1
enum Enum
{
}
// After 1
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
@"// Before 1
enum Enum
{
    // Before 2
    Name
    // After 2
}
// After 1
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
@"// Before 1
enum Enum
{
    // Before 2
    Name = 1
    // After 2
}
// After 1
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
@"// Before 1
enum Enum : short
{
}
// After 1
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
@"// Before 1
internal enum Enum : short
{
}
// After 1
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
