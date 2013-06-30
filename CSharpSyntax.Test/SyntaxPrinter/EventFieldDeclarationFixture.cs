using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class EventFieldDeclarationFixture : TestBase
    {
        [Test]
        public void SimpleEvent()
        {
            Test(
@"public event EventHandler Event;
",
                Syntax.EventFieldDeclaration(
                    declaration: Syntax.VariableDeclaration(
                        Syntax.ParseName("EventHandler"),
                        new[] { Syntax.VariableDeclarator("Event") }
                    ),
                    modifiers: Modifiers.Public
                )
            );
        }

        [Test]
        public void MultipleEvents()
        {
            Test(
@"public event EventHandler Event1, Event2;
",
                Syntax.EventFieldDeclaration(
                    declaration: Syntax.VariableDeclaration(
                        Syntax.ParseName("EventHandler"),
                        new[]
                        {
                            Syntax.VariableDeclarator("Event1"),
                            Syntax.VariableDeclarator("Event2")
                        }
                    ),
                    modifiers: Modifiers.Public
                )
            );
        }
    }
}
