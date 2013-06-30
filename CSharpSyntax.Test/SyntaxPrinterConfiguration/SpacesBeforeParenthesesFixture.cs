using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterConfiguration
{
    [TestFixture]
    public class SpacesBeforeParenthesesFixture : TestBase
    {
        [TestCase(
            true,
@"Console.WriteLine (""Hello world!"")"
        )]
        [TestCase(
            false,
@"Console.WriteLine(""Hello world!"")"
        )]
        public void MethodCallParentheses(bool space, string expected)
        {
            Test(
                expected,
                Syntax.InvocationExpression(
                    Syntax.ParseName("Console.WriteLine"),
                    Syntax.ArgumentList(
                        Syntax.Argument(
                            Syntax.LiteralExpression("Hello world!")
                        )
                    )
                ),
                p => p.Spaces.BeforeParentheses.MethodCallParentheses = space
            );
        }

        [TestCase(
            true,
@"Console.WriteLine ()"
)]
        [TestCase(
            false,
@"Console.WriteLine()"
        )]
        public void MethodCallEmptyParentheses(bool space, string expected)
        {
            Test(
                expected,
                Syntax.InvocationExpression(
                    Syntax.ParseName("Console.WriteLine"),
                    Syntax.ArgumentList()
                ),
                p => p.Spaces.BeforeParentheses.MethodCallEmptyParentheses = space
            );
        }

        [TestCase(
            true,
@"array [10]"
)]
        [TestCase(
            false,
@"array[10]"
        )]
        public void ArrayAccessBrackets(bool space, string expected)
        {
            Test(
                expected,
                Syntax.ElementAccessExpression(
                    Syntax.ParseName("array"),
                    Syntax.BracketedArgumentList(
                        Syntax.Argument(Syntax.LiteralExpression(10))
                    )
                ),
                p => p.Spaces.BeforeParentheses.ArrayAccessBrackets = space
            );
        }

        [TestCase(
            true,
@"void Method (int i)
{
}
Class (int i)
{
}
public static operator bool + (int i)
{
}
public static explicit operator int (int i)
{
}
"
        )]
        [TestCase(
            false,
@"void Method(int i)
{
}
Class(int i)
{
}
public static operator bool +(int i)
{
}
public static explicit operator int(int i)
{
}
"
        )]
        public void MethodDeclarationParentheses(bool space, string expected)
        {
            Test(
                expected,
                new SyntaxNode[]
                {
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
                    Syntax.ConstructorDeclaration(
                        "Class",
                        Syntax.ParameterList(
                            Syntax.Parameter(
                                type: Syntax.ParseName("int"),
                                identifier: "i"
                            )
                        ),
                        Syntax.Block()
                    ),
                    Syntax.OperatorDeclaration(
                        returnType: Syntax.ParseName("bool"),
                        modifiers: Modifiers.Public | Modifiers.Static,
                        @operator: Operator.Plus,
                        parameterList: Syntax.ParameterList(
                            Syntax.Parameter(
                                type: Syntax.ParseName("int"),
                                identifier: "i"
                            )
                        ),
                        body: Syntax.Block()
                    ),
                    Syntax.ConversionOperatorDeclaration(
                        modifiers: Modifiers.Static | Modifiers.Public,
                        type: Syntax.ParseName("int"),
                        parameterList: Syntax.ParameterList(
                            Syntax.Parameter(
                                type: Syntax.ParseName("int"),
                                identifier: "i"
                            )
                        ),
                        body: Syntax.Block()
                    )
                },
                p => p.Spaces.BeforeParentheses.MethodDeclarationParentheses = space
            );
        }

        [TestCase(
            true,
@"void Method ()
{
}
~Class ()
{
}
"
        )]
        [TestCase(
            false,
@"void Method()
{
}
~Class()
{
}
"
        )]
        public void MethodDeclarationEmptyParentheses(bool space, string expected)
        {
            Test(
                expected,
                new SyntaxNode[]
                {
                    Syntax.MethodDeclaration(
                        Syntax.ParseName("void"),
                        "Method",
                        Syntax.ParameterList(),
                        Syntax.Block()
                    ),
                    Syntax.DestructorDeclaration(
                        "Class",
                        Syntax.ParameterList(),
                        Syntax.Block()
                    )
                },
                p => p.Spaces.BeforeParentheses.MethodDeclarationEmptyParentheses = space
            );
        }

        [TestCase(
            true,
@"
{
    if (1)
    {
    }
}
"
        )]
        [TestCase(
            false,
@"
{
    if(1)
    {
    }
}
"
        )]
        public void IfParentheses(bool space, string expected)
        {
            Test(
                expected,
                Syntax.Block(
                    Syntax.IfStatement(
                        Syntax.LiteralExpression(1),
                        Syntax.Block()
                    )
                ),
                p => p.Spaces.BeforeParentheses.IfParentheses = space
            );
        }

        [TestCase(
            true,
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
        [TestCase(
            false,
@"
{
    while(1)
    {
    }
    do
    {
    }
    while(1);
}
"
        )]
        public void WhileParentheses(bool space, string expected)
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
                p => p.Spaces.BeforeParentheses.WhileParentheses = space
            );
        }

        [TestCase(
            true,
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
        [TestCase(
            false,
@"
{
    try
    {
    }
    catch(Exception)
    {
    }
}
"
        )]
        public void CatchParentheses(bool space, string expected)
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
                p => p.Spaces.BeforeParentheses.CatchParentheses = space
            );
        }

        [TestCase(
            true,
@"
{
    switch (1)
    {
    }
}
"
        )]
        [TestCase(
            false,
@"
{
    switch(1)
    {
    }
}
"
        )]
        public void SwitchParentheses(bool space, string expected)
        {
            Test(
                expected,
                Syntax.Block(
                    Syntax.SwitchStatement(
                        Syntax.LiteralExpression(1)
                    )
                ),
                p => p.Spaces.BeforeParentheses.SwitchParentheses = space
            );
        }

        [TestCase(
            true,
@"
{
    for (; ; )
    {
    }
}
"
        )]
        [TestCase(
            false,
@"
{
    for(; ; )
    {
    }
}
"
        )]
        public void ForParentheses(bool space, string expected)
        {
            Test(
                expected,
                Syntax.Block(
                    Syntax.ForStatement(Syntax.Block())
                ),
                p => p.Spaces.BeforeParentheses.ForParentheses = space
            );
        }

        [TestCase(
           true,
@"
{
    foreach (var x in y)
    {
    }
}
"
        )]
        [TestCase(
           false,
@"
{
    foreach(var x in y)
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
                p => p.Spaces.BeforeParentheses.ForEachParentheses = space
            );
        }

        [TestCase(
           true,
@"
{
    using (x)
    {
    }
}
"
        )]
        [TestCase(
           false,
@"
{
    using(x)
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
                p => p.Spaces.BeforeParentheses.UsingParentheses = space
            );
        }

        [TestCase(
           true,
@"
{
    lock (x)
    {
    }
}
"
        )]
        [TestCase(
           false,
@"
{
    lock(x)
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
                p => p.Spaces.BeforeParentheses.LockParentheses = space
            );
        }

        [TestCase(
            true,
@"typeof (int)"
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
                p => p.Spaces.BeforeParentheses.TypeOfParentheses = space
            );
        }

        [TestCase(
            true,
@"sizeof (int)"
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
                p => p.Spaces.BeforeParentheses.SizeOfParentheses = space
            );
        }

        [TestCase(
            true,
@"Foo <int>()"
        )]
        [TestCase(
            false,
@"Foo<int>()"
        )]
        public void BeforeTypeArgumentListAngle(bool space, string expected)
        {
            Test(
                expected,
                Syntax.InvocationExpression(
                    Syntax.ParseName("Foo<int>"),
                    Syntax.ArgumentList()
                ),
                p => p.Spaces.BeforeParentheses.BeforeTypeArgumentListAngle = space
            );
        }

        [TestCase(
            true,
@"void Foo <T>()
{
}
class Class <T>
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
        public void BeforeTypeParameterListAngle(bool space, string expected)
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
                p => p.Spaces.BeforeParentheses.BeforeTypeParameterListAngle = space
            );
        }
    }
}
