using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSyntax.Test.SyntaxPrinterTrivia
{
    public abstract class TestBase : Test.TestBase
    {
        protected void Test(string expected, SyntaxNode node, Action<Printer.Configuration.SyntaxPrinterConfiguration> configure)
        {
            Test(expected, new[] { node }, configure);
        }

        protected virtual void Test(string expected, IEnumerable<SyntaxNode> nodes, Action<Printer.Configuration.SyntaxPrinterConfiguration> configure)
        {
            var configuration = new Printer.Configuration.SyntaxPrinterConfiguration();

            configure(configuration);

            Test(expected, nodes, configuration);
        }

        protected override void Test(string expected, IEnumerable<SyntaxNode> nodes, Printer.Configuration.SyntaxPrinterConfiguration configuration)
        {
            var triviaAdder = new TriviaAdder();

            foreach (var node in nodes)
            {
                node.Accept(triviaAdder);
            }

            base.Test(expected, nodes, configuration);
        }

        private class TriviaAdder : SyntaxWalker
        {
            private int _nextIndex = 1;

            public override void DefaultVisit(SyntaxNode node)
            {
                if (!(node is BlockSyntax))
                {
                    var trivia = node as SyntaxTriviaNode;

                    if (trivia != null)
                    {
                        bool isElseIf =
                            trivia is IfStatementSyntax &&
                            trivia.Parent is ElseClauseSyntax;

                        if (!isElseIf)
                        {
                            int index = _nextIndex++;

                            trivia.LeadingTrivia.Add(Syntax.Comment("Before " + index));
                            trivia.TrailingTrivia.Add(Syntax.Comment("After " + index));
                        }
                    }
                }

                base.DefaultVisit(node);
            }
        }
    }
}
