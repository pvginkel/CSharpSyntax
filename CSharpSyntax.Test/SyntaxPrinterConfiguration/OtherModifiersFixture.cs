using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterConfiguration
{
    [TestFixture]
    public class OtherModifiersFixture : TestBase
    {
        [TestCase(
            true,
            true,
@"internal class Class
{
    public void Method()
    {
    }

    private void Method()
    {
    }

    private class SubClass
    {
    }

    private delegate void Delegate();
}
internal delegate void Delegate();
public delegate void Delegate();
"
        )]
        [TestCase(
            false,
            true,
@"class Class
{
    public void Method()
    {
    }

    private void Method()
    {
    }

    private class SubClass
    {
    }

    private delegate void Delegate();
}
delegate void Delegate();
public delegate void Delegate();
"
        )]
        [TestCase(
            true,
            false,
@"internal class Class
{
    public void Method()
    {
    }

    void Method()
    {
    }

    class SubClass
    {
    }

    delegate void Delegate();
}
internal delegate void Delegate();
public delegate void Delegate();
"
        )]
        [TestCase(
            false,
            false,
@"class Class
{
    public void Method()
    {
    }

    void Method()
    {
    }

    class SubClass
    {
    }

    delegate void Delegate();
}
delegate void Delegate();
public delegate void Delegate();
"
        )]
        public void UseExplicitInternalPrivateModifier(bool explicitInternal, bool explicitPrivate, string expected)
        {
            Test(
                expected,
                new SyntaxNode[]
                {
                    Syntax.ClassDeclaration(
                        identifier: "Class",
                        members: new MemberDeclarationSyntax[]
                        {
                            Syntax.MethodDeclaration(
                                returnType: Syntax.ParseName("void"),
                                identifier: "Method",
                                parameterList: Syntax.ParameterList(),
                                body: Syntax.Block(),
                                modifiers: Modifiers.Public
                            ),
                            Syntax.MethodDeclaration(
                                Syntax.ParseName("void"),
                                "Method",
                                Syntax.ParameterList(),
                                Syntax.Block()
                            ),
                            Syntax.ClassDeclaration("SubClass"),
                            Syntax.DelegateDeclaration(
                                Syntax.ParseName("void"),
                                "Delegate",
                                Syntax.ParameterList()
                            )
                        }
                    ),
                    Syntax.DelegateDeclaration(
                        returnType: Syntax.ParseName("void"),
                        identifier: "Delegate",
                        parameterList: Syntax.ParameterList()
                    ),
                    Syntax.DelegateDeclaration(
                        returnType: Syntax.ParseName("void"),
                        identifier: "Delegate",
                        parameterList: Syntax.ParameterList(),
                        modifiers: Modifiers.Public
                    )
                },
                p =>
                {
                    p.Other.Modifiers.UseExplicitInternalModifier = explicitInternal;
                    p.Other.Modifiers.UseExplicitPrivateModifier = explicitPrivate;
                }
            );
        }
    }
}
