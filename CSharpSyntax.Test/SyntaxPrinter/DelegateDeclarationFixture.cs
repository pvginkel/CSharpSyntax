using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class DelegateDeclarationFixture : TestBase
    {
        [Test]
        public void Simple()
        {
            Test(
@"delegate void Delegate();
",
                Syntax.DelegateDeclaration(
                    Syntax.ParseName("void"),
                    "Delegate",
                    Syntax.ParameterList()
                )
            );
        }

        [Test]
        public void WithModifier()
        {
            Test(
@"public delegate void Delegate();
",
                Syntax.DelegateDeclaration(
                    identifier: "Delegate",
                    returnType: Syntax.ParseName("void"),
                    modifiers: Modifiers.Public,
                    parameterList: Syntax.ParameterList()
                )
            );
        }

        [Test]
        public void WithType()
        {
            Test(
@"delegate void Delegate<T>();
",
                Syntax.DelegateDeclaration(
                    identifier: "Delegate",
                    returnType: Syntax.ParseName("void"),
                    parameterList: new ParameterListSyntax(),
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
@"delegate void Delegate<T>()
    where T : int;
",
                Syntax.DelegateDeclaration(
                    identifier: "Delegate",
                    returnType: Syntax.ParseName("void"),
                    parameterList: Syntax.ParameterList(),
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
@"delegate void Delegate<T>()
    where T : int where T : int;
",
                Syntax.DelegateDeclaration(
                    identifier: "Delegate",
                    returnType: Syntax.ParseName("void"),
                    parameterList: new ParameterListSyntax(),
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
