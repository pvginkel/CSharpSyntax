using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class MethodDeclarationFixture : TestBase
    {
        [Test]
        public void Simple()
        {
            Test(
@"// Before 1
void Method()
{
}
// After 1
",
                Syntax.MethodDeclaration(
                    Syntax.ParseName("void"),
                    "Method",
                    Syntax.ParameterList(),
                    Syntax.Block()
                )
            );
        }

        [Test]
        public void WithModifier()
        {
            Test(
@"// Before 1
public void Method()
{
}
// After 1
",
                Syntax.MethodDeclaration(
                    identifier: "Method",
                    returnType: Syntax.ParseName("void"),
                    modifiers: Modifiers.Public,
                    parameterList: Syntax.ParameterList(),
                    body: Syntax.Block()
                )
            );
        }

        [Test]
        public void WithStaticModifier()
        {
            Test(
@"// Before 1
public static void Method()
{
}
// After 1
",
                Syntax.MethodDeclaration(
                    identifier: "Method",
                    returnType: Syntax.ParseName("void"),
                    modifiers: Modifiers.Public | Modifiers.Static,
                    parameterList: Syntax.ParameterList(),
                    body: Syntax.Block()
                )
            );
        }

        [Test]
        public void WithType()
        {
            Test(
@"// Before 1
void Method<T>()
{
}
// After 1
",
                Syntax.MethodDeclaration(
                    identifier: "Method",
                    returnType: Syntax.ParseName("void"),
                    parameterList: new ParameterListSyntax(),
                    body: Syntax.Block(),
                    typeParameterList: Syntax.TypeParameterList(
                        Syntax.TypeParameter("T")
                    )
                )
            );
        }

        [Test]
        public void WithTypeAndConstraint()
        {
            Test(
@"// Before 1
void Method<T>()
    where T : int
{
}
// After 1
",
                Syntax.MethodDeclaration(
                    identifier: "Method",
                    returnType: Syntax.ParseName("void"),
                    parameterList: Syntax.ParameterList(),
                    body: Syntax.Block(),
                    typeParameterList: Syntax.TypeParameterList(
                        Syntax.TypeParameter("T")
                    ),
                    constraintClauses: new[]
                    {
                        Syntax.TypeParameterConstraintClause(
                            (IdentifierNameSyntax)Syntax.ParseName("T"),
                            new[] { Syntax.TypeConstraint(Syntax.ParseName("int")) }
                        )
                    }
                )
            );
        }

        [Test]
        public void WithTypeAndMultipleConstraints()
        {
            Test(
@"// Before 1
void Method<T>()
    where T : int where T : int
{
}
// After 1
",
                Syntax.MethodDeclaration(
                    identifier: "Method",
                    returnType: Syntax.ParseName("void"),
                    parameterList: new ParameterListSyntax(),
                    body: Syntax.Block(),
                    typeParameterList: Syntax.TypeParameterList(
                        Syntax.TypeParameter("T")
                    ),
                    constraintClauses: new[]
                    {
                        Syntax.TypeParameterConstraintClause(
                            (IdentifierNameSyntax)Syntax.ParseName("T"),
                            new[] { Syntax.TypeConstraint(Syntax.ParseName("int")) }
                        ),
                        Syntax.TypeParameterConstraintClause(
                            (IdentifierNameSyntax)Syntax.ParseName("T"),
                            new[] { Syntax.TypeConstraint(Syntax.ParseName("int")) }
                        )
                    }
                )
            );
        }
    }
}
