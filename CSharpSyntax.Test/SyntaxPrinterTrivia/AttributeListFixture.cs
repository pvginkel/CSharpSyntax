using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class AttributeListFixture : TestBase
    {
        [Test]
        public void SingleAttributeWithoutArguments()
        {
            Test(
@"[Attribute]
",
                new AttributeListSyntax
                {
                    Attributes =
                    {
                        new AttributeSyntax
                        {
                            Name = (NameSyntax)Syntax.ParseName("Attribute")
                        }
                    }
                }
            );
        }

        [Test]
        public void MultipleAttributes()
        {
            Test(
@"[Attribute, Attribute]
",
                new AttributeListSyntax
                {
                    Attributes =
                    {
                        new AttributeSyntax
                        {
                            Name = (NameSyntax)Syntax.ParseName("Attribute")
                        },
                        new AttributeSyntax
                        {
                            Name = (NameSyntax)Syntax.ParseName("Attribute")
                        }
                    }
                }
            );
        }

        [Test]
        public void MultipleAttributeLines()
        {
            Test(
@"[Attribute, Attribute]
[Attribute, Attribute]
",
                new AttributeListSyntax
                {
                    Attributes =
                    {
                        new AttributeSyntax
                        {
                            Name = (NameSyntax)Syntax.ParseName("Attribute")
                        },
                        new AttributeSyntax
                        {
                            Name = (NameSyntax)Syntax.ParseName("Attribute")
                        }
                    }
                },
                new AttributeListSyntax
                {
                    Attributes =
                    {
                        new AttributeSyntax
                        {
                            Name = (NameSyntax)Syntax.ParseName("Attribute")
                        },
                        new AttributeSyntax
                        {
                            Name = (NameSyntax)Syntax.ParseName("Attribute")
                        }
                    }
                }
            );
        }

        [TestCase("assembly", AttributeTarget.Assembly)]
        [TestCase("event", AttributeTarget.Event)]
        [TestCase("field", AttributeTarget.Field)]
        [TestCase("method", AttributeTarget.Method)]
        [TestCase("module", AttributeTarget.Module)]
        [TestCase("param", AttributeTarget.Param)]
        [TestCase("property", AttributeTarget.Property)]
        [TestCase("return", AttributeTarget.Return)]
        [TestCase("type", AttributeTarget.Type)]
        public void AttributeWithTarget(string code, AttributeTarget target)
        {
            Test(
@"[" + code + @": Attribute]
",
                new AttributeListSyntax
                {
                    Target = target,
                    Attributes =
                    {
                        new AttributeSyntax
                        {
                            Name = (NameSyntax)Syntax.ParseName("Attribute")
                        }
                    }
                }
            );
        }

        private void Test(string expected, params AttributeListSyntax[] attributes)
        {
            var node = new ClassDeclarationSyntax
            {
                Identifier = "Class"
            };

            node.AttributeLists.AddRange(attributes);

            Test(
@"// Before 1
" +
                expected +
@"class Class
{
}
// After 1
",
                node
            );
        }

        [Test]
        public void AttributeWithSingleArgument()
        {
            Test(
@"[Attribute(7)]
",
                new AttributeListSyntax
                {
                    Attributes =
                    {
                        new AttributeSyntax
                        {
                            Name = (NameSyntax)Syntax.ParseName("Attribute"),
                            ArgumentList = new AttributeArgumentListSyntax
                            {
                                Arguments =
                                {
                                    new AttributeArgumentSyntax
                                    {
                                        Expression = Syntax.LiteralExpression(7)
                                    }
                                }
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void AttributeWithMultipleArguments()
        {
            Test(
@"[Attribute(7, 8, 9)]
",
                new AttributeListSyntax
                {
                    Attributes =
                    {
                        new AttributeSyntax
                        {
                            Name = (NameSyntax)Syntax.ParseName("Attribute"),
                            ArgumentList = new AttributeArgumentListSyntax
                            {
                                Arguments =
                                {
                                    new AttributeArgumentSyntax
                                    {
                                        Expression = Syntax.LiteralExpression(7)
                                    },
                                    new AttributeArgumentSyntax
                                    {
                                        Expression = new LiteralExpressionSyntax { Value = 8 }
                                    },
                                    new AttributeArgumentSyntax
                                    {
                                        Expression = new LiteralExpressionSyntax { Value = 9 }
                                    }
                                }
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void AttributeWithNameArgument()
        {
            Test(
@"[Attribute(Name = 7)]
",
                new AttributeListSyntax
                {
                    Attributes =
                    {
                        new AttributeSyntax
                        {
                            Name = (NameSyntax)Syntax.ParseName("Attribute"),
                            ArgumentList = new AttributeArgumentListSyntax
                            {
                                Arguments =
                                {
                                    new AttributeArgumentSyntax
                                    {
                                        NameEquals = new NameEqualsSyntax
                                        {
                                            Name = (IdentifierNameSyntax)Syntax.ParseName("Name")
                                        },
                                        Expression = Syntax.LiteralExpression(7)
                                    }
                                }
                            }
                        }
                    }
                }
            );
        }

        [Test]
        public void AttributeWithNameColonArgument()
        {
            Test(
@"[Attribute(name: 7)]
",
                new AttributeListSyntax
                {
                    Attributes =
                    {
                        new AttributeSyntax
                        {
                            Name = (NameSyntax)Syntax.ParseName("Attribute"),
                            ArgumentList = new AttributeArgumentListSyntax
                            {
                                Arguments =
                                {
                                    new AttributeArgumentSyntax
                                    {
                                        NameColon = new NameColonSyntax
                                        {
                                            Name = (IdentifierNameSyntax)Syntax.ParseName("name")
                                        },
                                        Expression = Syntax.LiteralExpression(7)
                                    }
                                }
                            }
                        }
                    }
                }
            );
        }
    }
}
