using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class InvocationExpressionFixture : TestBase
    {
        [Test]
        public void WithoutArguments()
        {
            Test(
"WriteLine()",
                new InvocationExpressionSyntax
                {
                    Expression = Syntax.ParseName("WriteLine"),
                    ArgumentList = new ArgumentListSyntax()
                }
            );
        }

        [Test]
        public void MemberAccessWithoutArguments()
        {
            Test(
"Console.WriteLine()",
                new InvocationExpressionSyntax
                {
                    Expression = new MemberAccessExpressionSyntax
                    {
                        Expression = Syntax.ParseName("Console"),
                        Name = (SimpleNameSyntax)Syntax.ParseName("WriteLine")
                    },
                    ArgumentList = new ArgumentListSyntax()
                }
            );
        }

        [Test]
        public void SingleSimpleArgument()
        {
            Test(
"WriteLine(1)",
                new InvocationExpressionSyntax
                {
                    Expression = Syntax.ParseName("WriteLine"),
                    ArgumentList = new ArgumentListSyntax
                    {
                        Arguments =
                        {
                            new ArgumentSyntax
                            {
                                Expression = new LiteralExpressionSyntax { Value = 1 }
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void MultipleSingleArguments()
        {
            Test(
"WriteLine(1, 1)",
                new InvocationExpressionSyntax
                {
                    Expression = Syntax.ParseName("WriteLine"),
                    ArgumentList = new ArgumentListSyntax
                    {
                        Arguments =
                        {
                            new ArgumentSyntax
                            {
                                Expression = new LiteralExpressionSyntax { Value = 1 }
                            },
                            new ArgumentSyntax
                            {
                                Expression = new LiteralExpressionSyntax { Value = 1 }
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void SingleWithModifier()
        {
            Test(
"WriteLine(ref 1)",
                new InvocationExpressionSyntax
                {
                    Expression = Syntax.ParseName("WriteLine"),
                    ArgumentList = new ArgumentListSyntax
                    {
                        Arguments =
                        {
                            new ArgumentSyntax
                            {
                                Expression = new LiteralExpressionSyntax { Value = 1 },
                                Modifier = ParameterModifier.Ref
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void SingleWithName()
        {
            Test(
"WriteLine(arg: 1)",
                new InvocationExpressionSyntax
                {
                    Expression = Syntax.ParseName("WriteLine"),
                    ArgumentList = new ArgumentListSyntax
                    {
                        Arguments =
                        {
                            new ArgumentSyntax
                            {
                                Expression = new LiteralExpressionSyntax { Value = 1 },
                                NameColon = new NameColonSyntax
                                {
                                    Name = (IdentifierNameSyntax)Syntax.ParseName("arg")
                                }
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void OnExpression()
        {
            Test(
"1.ToString()",
                new InvocationExpressionSyntax
                {
                    Expression = new MemberAccessExpressionSyntax
                    {
                        Expression = new LiteralExpressionSyntax { Value = 1 },
                        Name = (SimpleNameSyntax)Syntax.ParseName("ToString")
                    },
                    ArgumentList = new ArgumentListSyntax()
                }
            );
        }
    }
}
