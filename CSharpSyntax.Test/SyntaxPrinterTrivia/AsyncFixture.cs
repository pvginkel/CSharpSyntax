using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class AsyncFixture : TestBase
    {
        [Test]
        public void Method()
        {
            Test(
@"// Before 1
async void Method()
{
}
// After 1
",
                Syntax.MethodDeclaration(
                    modifiers: Modifiers.Async,
                    identifier: "Method",
                    returnType: Syntax.ParseName("void"),
                    parameterList: Syntax.ParameterList(),
                    body: Syntax.Block()
                )
            );
        }
    }
}
