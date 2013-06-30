using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class EventDeclarationFixture : TestBase
    {
        [Test]
        public void SimpleEvent()
        {
            Test(
@"// Before 1
public event EventHandler Event
{
    // Before 2
    add { }
    // After 2
    // Before 3
    remove { }
    // After 3
}
// After 1
",
                Syntax.EventDeclaration(
                    type: Syntax.ParseName("EventHandler"),
                    identifier: "Event",
                    modifiers: Modifiers.Public,
                    accessorList: Syntax.AccessorList(
                        Syntax.AccessorDeclaration(
                            AccessorDeclarationKind.Add,
                            Syntax.Block()
                        ),
                        Syntax.AccessorDeclaration(
                            AccessorDeclarationKind.Remove,
                            Syntax.Block()
                        )
                    )
                )
            );
        }

        [Test]
        public void ExplicitInterface()
        {
            Test(
@"// Before 1
event EventHandler IInterface.Event
{
    // Before 2
    add { }
    // After 2
    // Before 3
    remove { }
    // After 3
}
// After 1
",
                Syntax.EventDeclaration(
                    type: Syntax.ParseName("EventHandler"),
                    identifier: "Event",
                    explicitInterfaceSpecifier: Syntax.ExplicitInterfaceSpecifier(
                        (NameSyntax)Syntax.ParseName("IInterface")
                    ),
                    accessorList: Syntax.AccessorList(
                        Syntax.AccessorDeclaration(
                            AccessorDeclarationKind.Add,
                            Syntax.Block()
                        ),
                        Syntax.AccessorDeclaration(
                            AccessorDeclarationKind.Remove,
                            Syntax.Block()
                        )
                    )
                )
            );
        }

        [Test]
        public void WithAddRemove()
        {
            Test(
@"// Before 1
public event EventHandler Event
{
    // Before 2
    add { }
    // After 2
    // Before 3
    remove { }
    // After 3
}
// After 1
",
                Syntax.EventDeclaration(
                    type: Syntax.ParseName("EventHandler"),
                    identifier: "Event",
                    modifiers: Modifiers.Public,
                    accessorList: Syntax.AccessorList(
                        Syntax.AccessorDeclaration(
                            AccessorDeclarationKind.Add,
                            Syntax.Block()
                        ),
                        Syntax.AccessorDeclaration(
                            AccessorDeclarationKind.Remove,
                            Syntax.Block()
                        )
                    )
                )
            );
        }
    }
}
