using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class EventDeclarationFixture : TestBase
    {
        [Test]
        public void SimpleEvent()
        {
            Test(
@"public event EventHandler Event
{
    add { }
    remove { }
}
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
@"event EventHandler IInterface.Event
{
    add { }
    remove { }
}
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
@"public event EventHandler Event
{
    add { }
    remove { }
}
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
