using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class MethodDeclarationFixture : TestBase
    {
        [Test]
        public void Simple()
        {
            Test(
@"void Method()
{
}
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
@"public void Method()
{
}
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
@"public static void Method()
{
}
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
@"void Method<T>()
{
}
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
@"void Method<T>()
    where T : int
{
}
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
@"void Method<T>()
    where T : int where T : int
{
}
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
