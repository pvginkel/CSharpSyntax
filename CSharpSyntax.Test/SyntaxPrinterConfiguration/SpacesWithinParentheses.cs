using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterConfiguration
{
    [TestFixture]
    public class SpacesWithinParentheses : TestBase
    {
        [TestCase(
            true,
@"( 1 )"
        )]
        [TestCase(
            false,
@"(1)"
        )]
        public void Parentheses(bool spaces, string expected)
        {
            Test(
                expected,
                Syntax.ParenthesizedExpression(Syntax.LiteralExpression(1)),
                p => p.Spaces.WithinParentheses.Parentheses = spaces
            );
        }

        [TestCase(
            true,
@"void Method( int i )
{
}
"
        )]
        [TestCase(
            false,
@"void Method(int i)
{
}
"
        )]
        public void MethodDeclarationParentheses(bool spaces, string expected)
        {
            Test(
                expected,
                Syntax.MethodDeclaration(
                    Syntax.ParseName("void"),
                    "Method",
                    Syntax.ParameterList(
                        Syntax.Parameter(
                            type: Syntax.ParseName("int"),
                            identifier: "i"
                        )
                    ),
                    Syntax.Block()
                ),
                p => p.Spaces.WithinParentheses.MethodDeclarationParentheses = spaces
            );
        }

        [TestCase(
            true,
@"void Method( )
{
}
"
        )]
        [TestCase(
            false,
@"void Method()
{
}
"
        )]
        public void MethodDeclarationEmptyParentheses(bool spaces, string expected)
        {
            Test(
                expected,
                Syntax.MethodDeclaration(
                    Syntax.ParseName("void"),
                    "Method",
                    Syntax.ParameterList(),
                    Syntax.Block()
                ),
                p => p.Spaces.WithinParentheses.MethodDeclarationEmptyParentheses = spaces
            );
        }

        [TestCase(
            true,
@"Method( 1 )"
        )]
        [TestCase(
            false,
@"Method(1)"
        )]
        public void MethodCallParentheses(bool spaces, string expected)
        {
            Test(
                expected,
                Syntax.InvocationExpression(
                    Syntax.ParseName("Method"),
                    Syntax.ArgumentList(
                        Syntax.Argument(Syntax.LiteralExpression(1))
                    )
                ),
                p => p.Spaces.WithinParentheses.MethodCallParentheses = spaces
            );
        }

        [TestCase(
            true,
@"Method( )"
        )]
        [TestCase(
            false,
@"Method()"
        )]
        public void MethodCallEmptyParentheses(bool spaces, string expected)
        {
            Test(
                expected,
                Syntax.InvocationExpression(
                    Syntax.ParseName("Method"),
                    Syntax.ArgumentList()
                ),
                p => p.Spaces.WithinParentheses.MethodCallEmptyParentheses = spaces
            );
        }

        [TestCase(
            true,
@"array[ 10 ]"
        )]
        [TestCase(
            false,
@"array[10]"
        )]
        public void ArrayAccessBrackets(bool spaces, string expected)
        {
            Test(
                expected,
                Syntax.ElementAccessExpression(
                    Syntax.ParseName("array"),
                    Syntax.BracketedArgumentList(
                        Syntax.Argument(
                            Syntax.LiteralExpression(10)
                        )
                    )
                ),
                p => p.Spaces.WithinParentheses.ArrayAccessBrackets = spaces
            );
        }

        [TestCase(
            true,
@"( int )1"
        )]
        [TestCase(
            false,
@"(int)1"
        )]
        public void TypeCastParentheses(bool spaces, string expected)
        {
            Test(
                expected,
                Syntax.CastExpression(
                    Syntax.ParseName("int"),
                    Syntax.LiteralExpression(1)
                ),
                p => p.Spaces.WithinParentheses.TypeCastParentheses = spaces
            );
        }

        [TestCase(
            true,
@"
{
    if ( 1 )
    {
    }
}
"
        )]
        [TestCase(
            false,
@"
{
    if (1)
    {
    }
}
"
        )]
        public void IfParentheses(bool spaces, string expected)
        {
            Test(
                expected,
                Syntax.Block(
                    Syntax.IfStatement(
                        Syntax.LiteralExpression(1),
                        Syntax.Block()
                    )
                ),
                p => p.Spaces.WithinParentheses.IfParentheses = spaces
            );
        }

        [TestCase(
            true,
@"
{
    while ( 1 )
    {
    }
    do
    {
    }
    while ( 1 );
}
"
        )]
        [TestCase(
            false,
@"
{
    while (1)
    {
    }
    do
    {
    }
    while (1);
}
"
        )]
        public void WhileParentheses(bool spaces, string expected)
        {
            Test(
                expected,
                Syntax.Block(
                    Syntax.WhileStatement(
                        Syntax.LiteralExpression(1),
                        Syntax.Block()
                    ),
                    Syntax.DoStatement(
                        Syntax.Block(),
                        Syntax.LiteralExpression(1)
                    )
                ),
                p => p.Spaces.WithinParentheses.WhileParentheses = spaces
            );
        }

        [TestCase(
            true,
@"
{
    try
    {
    }
    catch ( Exception )
    {
    }
}
"
        )]
        [TestCase(
            false,
@"
{
    try
    {
    }
    catch (Exception)
    {
    }
}
"
        )]
        public void CatchParentheses(bool spaces, string expected)
        {
            Test(
                expected,
                Syntax.Block(
                    Syntax.TryStatement(
                        Syntax.Block(),
                        new[]
                        {
                            Syntax.CatchClause(
                                Syntax.CatchDeclaration(Syntax.ParseName("Exception")),
                                Syntax.Block()
                            )
                        }
                    )
                ),
                p => p.Spaces.WithinParentheses.CatchParentheses = spaces
            );
        }

        [TestCase(
            true,
@"
{
    switch ( 1 )
    {
    }
}
"
        )]
        [TestCase(
            false,
@"
{
    switch (1)
    {
    }
}
"
        )]
        public void SwitchParentheses(bool spaces, string expected)
        {
            Test(
                expected,
                Syntax.Block(
                    Syntax.SwitchStatement(
                        Syntax.LiteralExpression(1)
                    )
                ),
                p => p.Spaces.WithinParentheses.SwitchParentheses = spaces
            );
        }

        [TestCase(
            true,
@"
{
    for ( int i = 0; i < 10; i++ )
    {
    }
}
"
        )]
        [TestCase(
            false,
@"
{
    for (int i = 0; i < 10; i++)
    {
    }
}
"
        )]
        public void ForParentheses(bool spaces, string expected)
        {
            Test(
                expected,
                Syntax.Block(
                    Syntax.ForStatement(
                        Syntax.VariableDeclaration(
                            Syntax.ParseName("int"),
                            new[]
                            {
                                Syntax.VariableDeclarator(
                                    "i",
                                    initializer: Syntax.EqualsValueClause(
                                        Syntax.LiteralExpression(0)
                                    )
                                )
                            }
                        ),
                        condition: Syntax.BinaryExpression(
                            BinaryOperator.LessThan,
                            Syntax.ParseName("i"),
                            Syntax.LiteralExpression(10)
                        ),
                        incrementors: new[]
                        {
                            Syntax.PostfixUnaryExpression(
                                PostfixUnaryOperator.PlusPlus,
                                Syntax.ParseName("i")
                            )
                        },
                        statement: Syntax.Block()
                    )
                ),
                p => p.Spaces.WithinParentheses.ForParentheses = spaces
            );
        }

        [TestCase(
           true,
@"
{
    foreach ( var x in y )
    {
    }
}
"
        )]
        [TestCase(
           false,
@"
{
    foreach (var x in y)
    {
    }
}
"
        )]
        public void ForEachParentheses(bool space, string expected)
        {
            Test(
                expected,
                Syntax.Block(
                    Syntax.ForEachStatement(
                        Syntax.ParseName("var"),
                        "x",
                        Syntax.ParseName("y"),
                        Syntax.Block()
                    )
                ),
                p => p.Spaces.WithinParentheses.ForEachParentheses = space
            );
        }


        [TestCase(
           true,
@"
{
    using ( x )
    {
    }
}
"
        )]
        [TestCase(
           false,
@"
{
    using (x)
    {
    }
}
"
        )]
        public void UsingParentheses(bool space, string expected)
        {
            Test(
                expected,
                Syntax.Block(
                    Syntax.UsingStatement(
                        expression: Syntax.ParseName("x"),
                        statement: Syntax.Block()
                    )
                ),
                p => p.Spaces.WithinParentheses.UsingParentheses = space
            );
        }

        [TestCase(
           true,
@"
{
    lock ( x )
    {
    }
}
"
        )]
        [TestCase(
           false,
@"
{
    lock (x)
    {
    }
}
"
        )]
        public void LockParentheses(bool space, string expected)
        {
            Test(
                expected,
                Syntax.Block(
                    Syntax.LockStatement(
                        Syntax.ParseName("x"),
                        Syntax.Block()
                    )
                ),
                p => p.Spaces.WithinParentheses.LockParentheses = space
            );
        }

        [TestCase(
            true,
@"typeof( int )"
        )]
        [TestCase(
            false,
@"typeof(int)"
        )]
        public void TypeOfParentheses(bool space, string expected)
        {
            Test(
                expected,
                Syntax.TypeOfExpression(Syntax.ParseName("int")),
                p => p.Spaces.WithinParentheses.TypeOfParentheses = space
            );
        }

        [TestCase(
            true,
@"sizeof( int )"
        )]
        [TestCase(
            false,
@"sizeof(int)"
        )]
        public void SizeOfParentheses(bool space, string expected)
        {
            Test(
                expected,
                Syntax.SizeOfExpression(Syntax.ParseName("int")),
                p => p.Spaces.WithinParentheses.SizeOfParentheses = space
            );
        }

        [TestCase(
            true,
@"Foo< int >()"
        )]
        [TestCase(
            false,
@"Foo<int>()"
        )]
        public void TypeArgumentAngles(bool space, string expected)
        {
            Test(
                expected,
                Syntax.InvocationExpression(
                    Syntax.ParseName("Foo<int>"),
                    Syntax.ArgumentList()
                ),
                p => p.Spaces.WithinParentheses.TypeArgumentAngles = space
            );
        }

        [TestCase(
            true,
@"void Foo< T >()
{
}
class Class< T >
{
}
"
        )]
        [TestCase(
            false,
@"void Foo<T>()
{
}
class Class<T>
{
}
"
        )]
        public void TypeParameterAngles(bool space, string expected)
        {
            Test(
                expected,
                new SyntaxNode[]
                {
                    Syntax.MethodDeclaration(
                        returnType: Syntax.ParseName("void"),
                        identifier: "Foo",
                        parameterList: Syntax.ParameterList(),
                        body: Syntax.Block(),
                        typeParameterList: Syntax.TypeParameterList(
                            Syntax.TypeParameter("T")
                        )
                    ),
                    Syntax.ClassDeclaration(
                        identifier: "Class",
                        typeParameterList: Syntax.TypeParameterList(
                            Syntax.TypeParameter("T")
                        )
                    )
                },
                p => p.Spaces.WithinParentheses.TypeParameterAngles = space
            );
        }
    }
}
