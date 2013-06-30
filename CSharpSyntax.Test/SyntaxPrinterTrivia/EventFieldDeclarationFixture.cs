using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class EventFieldDeclarationFixture : TestBase
    {
        [Test]
        public void SimpleEvent()
        {
            Test(
@"// Before 1
public event EventHandler Event;
// After 1
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
@"// Before 1
public event EventHandler Event1, Event2;
// After 1
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
