# CSharpSyntax

LGPL License.

[Download from NuGet](http://nuget.org/packages/CSharpSyntax).

## Introduction

CSharpSyntax is a C# code generator library based on the
[Roslyn](http://msdn.microsoft.com/en-us/vstudio/hh543936) API.

Roslyn is a project by Microsoft to implement and expose a C# (and Visual Basic)
compiler as a .NET library. As part of this project, Roslyn contains an AST
and parser for C# code which can be used to build a C# source file in memory.

Even though the API of Roslyn allows you to create a in memory representation of
a C# source file, it has a few disadvantages which makes it hard to use for
code generation:

* The Roslyn API is immutable. This means that once you've created a syntax
  node, you cannot change it. Instead, you need to create a new version of that
  node which incorporates the changes you need;

* The Roslyn API allows you to specify every little detail of the C# source code.
  This can make it very cumbersome to build a C# source file in memory.

The CSharpSyntax library tries to solve these issues. Firstly, the syntax tree
is fully mutable. Roslyn uses the Syntax static class to create syntax nodes.
The syntax nodes returned from these class are immutable. The CSharpSyntax library
also provides a Syntax static class with (roughly) the same methods, but the
syntax nodes that are returned are fully mutable. You can even create the syntax
nodes directly, e.g. using C# object initializers.

Second, the CSharpSyntax library is very limited in the kind of trivia it allows
you to specify on syntax nodes. It does not give you control over where spaces
are located, or the indentation of a specific line of code. Instead, the
SyntaxPrinter class exposes configuration which allows you to specify how the
resulting code should be formatted. This configuration is based on the code
formatting options of ReSharper and are quite extensive.

## Creating a syntax tree

There are two ways to create a syntax tree. You can instantiate syntax node directly,
or you can use the Syntax static class.

    // Instantiation using object initializers.
    
    var methodDeclaration = new MethodDeclarationSyntax
    {
        Identifier = "Method",
        ReturnType = Syntax.ParseName("void"),
        ParameterList = new ParameterListSyntax
        {
            Parameters =
            {
                new ParameterSyntax
                {
                    Identifier = "value",
                    Type = Syntax.ParseName("int")
                }
            }
        },
        Body = new BlockSyntax()
    };
    
    // Instantiation using the Syntax static class.

    var methodDeclaration = Syntax.MethodDeclaration(
        "void",
        "Method",
        Syntax.ParameterList(
            Syntax.Parameter(
                type: "int",
                identifier: "value"
            )
        ),
        Syntax.Block()
    );

The result of both ways of instantiating syntax node is the same. However, most
of the times, it's easier to go through the Syntax static class. The primary
reason for this is that the Syntax static class provides helpful overrides
which allows you to write shorter code. E.g. the ParameterList method has
an params overload which takes the parameters of the parameter list. Also,
all syntax nodes that take a TypeSyntax provide an overload with a string parameter
which parses the input using Syntax.ParseName (see "ParseName" section).

All methods on the Syntax static class provide default values on the parameters.
This allows you to specify only the parameters you need to provide and skip
the ones for which the default value is OK.

The API of CSharpSyntax is roughly the same as the Roslyn API. To find out how
you represent certain constructs through the API, either consult the Rosly API
documentation, or have a look at the unit tests. The unit tests contain (at least
one) test for every possible construct supported by the library.

## Generating source files

Once you have a syntax tree, you can generate source code from this. You
can generate source code from any syntax node. However, if you want to generate
a complete source file, you should generate code from a CompilationUnitSyntax
syntax node. This syntax node provides all the elements of a source file and
allows you to specify source file level comments.

The following code generates the source code for the previous example:

    using (var writer = new StringWriter())
    {
        using (var printer = new SyntaxPrinter(new SyntaxWriter(writer)))
        {
            printer.Visit(methodDeclaration);
        }

        Console.WriteLine(writer.GetStringBuilder().ToString());
    }

This outputs the following text on the console:

    void Method(int value)
    {
    }

If you want to deviate from the default configuration, you can specify a
configuration to the constructor of the SyntaxWriter class, e.g.:

    var configuration = new SyntaxPrinterConfiguration
    {
        BracesLayout =
        {
            MethodDeclaration = BraceFormatting.EndOfLineKr
        }
    };

    using (var writer = new StringWriter())
    {
        using (var printer = new SyntaxPrinter(new SyntaxWriter(writer, configuration)))
        {
            printer.Visit(methodDeclaration);
        }

        Console.WriteLine(writer.GetStringBuilder().ToString());
    }

This outputs the following text on the console:

    void Method(int value) {
    }

The configuration class provides many options to format the source code. The unit
tests provided with the project is the primary source of documentation on the
syntax printer configuration.

The syntax printer configuration classes support XML serialization. This means
that you can store the configuration as XML and de-serialize it using
[XmlSerializer](http://msdn.microsoft.com/en-us/library/System.Xml.Serialization.XmlSerializer.aspx) class.

## ParseName

Since Roslyn is a full C# compiler, it also includes a parser. The CSharpSyntax
library does not include a parser, except for parsing names. This parser is
exposed through the Syntax.ParseName method, just like it is in the Roslyn
library.

The reason the CSharpSyntax library includes a parser for names, is because it is
very cumbersome to build syntax trees for names. E.g., to build a syntax
tree for the name "Namespace.Class<T1, T2>[,]", you would need to write the following
code:

    var type = Syntax.ArrayType(
        Syntax.QualifiedName(
            Syntax.IdentifierName("Namespace"),
            Syntax.GenericName(
                "Class",
                Syntax.TypeArgumentList(
                    Syntax.IdentifierName("T1"),
                    Syntax.IdentifierName("T2")
                )
            )
        ),
        new[]
        {
            Syntax.ArrayRankSpecifier(
                Syntax.OmittedArraySizeExpression(),
                Syntax.OmittedArraySizeExpression()
            )
        }
    );

This can become quite irritating. Instead, you can write the following:

    var type = Syntax.ParseName("Namespace.Class<T1, T2>[,]");

This produces the exact same syntax tree as when you would have created it
manually.

Many syntax nodes have a TypeSyntax property or a property with a type that
derives from TypeSyntax. The factory methods on the Syntax static class provide
overloads that take a string instead of a TypeSyntax parameter. When you use this
overload, the Syntax.ParseName method is used to parse the string and assign
the resulting syntax tree.

The name parser is complete and supports all constructs that can be created by
hand using the TypeSyntax hierarchy. It e.g. also understands predefined types and
creates the correct PredefinedTypeSyntax syntax node for these.

## Trivia

Even though the CSharpSyntax library does not allow you to specify trivia
at the level of detail as Roslyn does, it does allow you to specify some trivia.
The only reason the CSharpSyntax library allows you to specify trivia, is to allow
you to specify comments and newlines in the generated source code.

Syntax nodes have a LeadingTrivia and a TrailingTrivia property which allows you
to specify trivia on that syntax node. For trivia, the Syntax static class also
contains a number of factory methods, specifically Comment, BlockComment, NewLine
and XmlComment.

## Reporting issues

The CSharpSyntax library is currently in beta. If you find issues,
please report them in the [issues section](https://github.com/pvginkel/CSharpSyntax/issues)
on the GitHub website.
