using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    [TestFixture]
    public class MiscellaneousTriviaFixture : Test.TestBase
    {
        [Test]
        public void XmlComment()
        {
            var statement = Syntax.ReturnStatement();

            statement.LeadingTrivia.Add(Syntax.XmlComment("Comment"));

            Test(
@"
{
    /// Comment
    return;
}
",
                Syntax.Block(statement)
            );
        }

        [Test]
        public void MultiLineComment()
        {
            var statement = Syntax.ReturnStatement();

            statement.LeadingTrivia.Add(Syntax.Comment("Line 1" + Environment.NewLine + "Line 2"));

            Test(
@"
{
    // Line 1
    // Line 2
    return;
}
",
                Syntax.Block(statement)
            );
        }

        [Test]
        public void MultipleSingleLineComments()
        {
            var statement = Syntax.ReturnStatement();

            statement.LeadingTrivia.Add(Syntax.Comment("Line 1"));
            statement.LeadingTrivia.Add(Syntax.Comment("Line 2"));

            Test(
@"
{
    // Line 1
    // Line 2
    return;
}
",
                Syntax.Block(statement)
            );
        }

        [Test]
        public void MultipleSingleLineCommentsWithNewline()
        {
            var statement = Syntax.ReturnStatement();

            statement.LeadingTrivia.Add(Syntax.Comment("Line 1"));
            statement.LeadingTrivia.Add(Syntax.NewLine());
            statement.LeadingTrivia.Add(Syntax.Comment("Line 2"));

            Test(
@"
{
    // Line 1

    // Line 2
    return;
}
",
                Syntax.Block(statement)
            );
        }

        [Test]
        public void NewLine()
        {
            var statement = Syntax.ReturnStatement();

            statement.LeadingTrivia.Add(Syntax.NewLine());
            statement.TrailingTrivia.Add(Syntax.NewLine());

            Test(
@"
{

    return;

}
",
                Syntax.Block(statement)
            );
        }

        [Test]
        public void BlockComment()
        {
            var statement = Syntax.ReturnStatement();

            statement.LeadingTrivia.Add(Syntax.BlockComment("Comment"));

            Test(
@"
{
    /* Comment */
    return;
}
",
                Syntax.Block(statement)
            );
        }

        [Test]
        public void BlockCommentOnNewLine()
        {
            var statement = Syntax.ReturnStatement();

            statement.LeadingTrivia.Add(Syntax.BlockComment(Environment.NewLine + "Comment" + Environment.NewLine));

            Test(
@"
{
    /*
    Comment
    */
    return;
}
",
                Syntax.Block(statement)
            );
        }

        [Test]
        public void MultiLineBlockComment()
        {
            var statement = Syntax.ReturnStatement();

            statement.LeadingTrivia.Add(Syntax.BlockComment(Environment.NewLine + "Line 1" + Environment.NewLine + "Line 2" + Environment.NewLine));

            Test(
@"
{
    /*
    Line 1
    Line 2
    */
    return;
}
",
                Syntax.Block(statement)
            );
        }

        [Test]
        public void MultiLineBlockCommentWithIndent()
        {
            var statement = Syntax.ReturnStatement();

            statement.LeadingTrivia.Add(Syntax.BlockComment(Environment.NewLine + "  Line 1" + Environment.NewLine + "  Line 2" + Environment.NewLine));
            statement.LeadingTrivia.Add(Syntax.Comment("Line 1"));
            statement.LeadingTrivia.Add(Syntax.NewLine());
            statement.LeadingTrivia.Add(Syntax.Comment("Line 2"));

            Test(
@"
{
    /*
      Line 1
      Line 2
    */
    // Line 1

    // Line 2
    return;
}
",
                Syntax.Block(statement)
            );
        }

        [Test]
        public void Mixed()
        {
            var statement = Syntax.ReturnStatement();

            statement.LeadingTrivia.Add(Syntax.BlockComment(Environment.NewLine + "  Line 1" + Environment.NewLine + "  Line 2" + Environment.NewLine));

            Test(
@"
{
    /*
      Line 1
      Line 2
    */
    return;
}
",
                Syntax.Block(statement)
            );
        }

        [Test]
        public void LineWrap()
        {
            var statement = Syntax.ReturnStatement();

            statement.LeadingTrivia.Add(Syntax.Comment("This is a long line which should not be wrapped. This is a long line which should not be wrapped."));

            Test(
@"
{
    // This is a long line which should not be wrapped. This is a long line which should not be wrapped.
    return;
}
",
                new[] { Syntax.Block(statement) },
                p =>
                {
                    p.LineBreaksAndWrapping.LineWrapping.WrapLongLines = true;
                    p.LineBreaksAndWrapping.LineWrapping.RightMargin = 1;
                }
            );
        }

        [Test]
        public void Block()
        {
            var block = Syntax.Block();

            block.LeadingTrivia.Add(Syntax.Comment("Before"));
            block.TrailingTrivia.Add(Syntax.Comment("After"));

            Test(
@"
{
    // Before
    {
    }
    // After
}
",
                Syntax.Block(block)
            );
        }

        protected void Test(string expected, IEnumerable<SyntaxNode> nodes, Action<Printer.Configuration.SyntaxPrinterConfiguration> configure)
        {
            var configuration = new Printer.Configuration.SyntaxPrinterConfiguration();

            configure(configuration);

            Test(expected, nodes, configuration);
        }
    }
}
