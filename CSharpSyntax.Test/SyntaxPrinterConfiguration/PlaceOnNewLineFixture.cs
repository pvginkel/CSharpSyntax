using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterConfiguration
{
    [TestFixture]
    public class PlaceOnNewLineFixture : TestBase
    {
        [TestCase(
            true,
@"
{
    if (1)
    {
    }
    else
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
    } else
    {
    }
}
"
        )]
        public void PlaceElseOnNewLine(bool onNewLine, string expected)
        {
            Test(
                expected,
                Syntax.Block(
                    Syntax.IfStatement(
                        Syntax.LiteralExpression(1),
                        Syntax.Block(),
                        Syntax.ElseClause(Syntax.Block())
                    )
                ),
                p => p.LineBreaksAndWrapping.PlaceOnNewLine.PlaceElseOnNewLine = onNewLine
            );
        }

        [TestCase(
            true,
@"
{
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
    do
    {
    } while (1);
}
"
        )]
        public void PlaceWhileOnNewLine(bool onNewLine, string expected)
        {
            Test(
                expected,
                Syntax.Block(
                    Syntax.DoStatement(
                        Syntax.Block(),
                        Syntax.LiteralExpression(1)
                    )
                ),
                p => p.LineBreaksAndWrapping.PlaceOnNewLine.PlaceWhileOnNewLine = onNewLine
            );
        }

        [TestCase(
            true,
            true,
@"
{
    try
    {
    }
    catch (Exception)
    {
    }
    catch (Exception)
    {
    }
    finally
    {
    }
}
"
        )]
        [TestCase(
            false,
            true,
@"
{
    try
    {
    } catch (Exception)
    {
    } catch (Exception)
    {
    }
    finally
    {
    }
}
"
        )]
        [TestCase(
            true,
            false,
@"
{
    try
    {
    }
    catch (Exception)
    {
    }
    catch (Exception)
    {
    } finally
    {
    }
}
"
        )]
        [TestCase(
            false,
            false,
@"
{
    try
    {
    } catch (Exception)
    {
    } catch (Exception)
    {
    } finally
    {
    }
}
"
        )]
        public void PlaceCatchFinallyOnNewLine(bool catchOnNewLine, bool finallyOnNewLine, string expected)
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
                            ),
                            Syntax.CatchClause(
                                Syntax.CatchDeclaration(Syntax.ParseName("Exception")),
                                Syntax.Block()
                            )
                        },
                        Syntax.FinallyClause(Syntax.Block())
                    )
                ),
                p =>
                {
                    p.LineBreaksAndWrapping.PlaceOnNewLine.PlaceCatchOnNewLine = catchOnNewLine;
                    p.LineBreaksAndWrapping.PlaceOnNewLine.PlaceFinallyOnNewLine = finallyOnNewLine;
                }
            );
        }

        [TestCase(
            true,
            true,
@"
{
    try
    {
    }
    finally
    {
    }
}
"
        )]
        [TestCase(
            false,
            true,
@"
{
    try
    {
    }
    finally
    {
    }
}
"
        )]
        [TestCase(
            true,
            false,
@"
{
    try
    {
    } finally
    {
    }
}
"
        )]
        [TestCase(
            false,
            false,
@"
{
    try
    {
    } finally
    {
    }
}
"
        )]
        public void PlaceCatchFinallyOnNewLineWithoutCatch(bool catchOnNewLine, bool finallyOnNewLine, string expected)
        {
            Test(
                expected,
                Syntax.Block(
                    Syntax.TryStatement(
                        Syntax.Block(),
                        @finally: Syntax.FinallyClause(Syntax.Block())
                    )
                ),
                p =>
                {
                    p.LineBreaksAndWrapping.PlaceOnNewLine.PlaceCatchOnNewLine = catchOnNewLine;
                    p.LineBreaksAndWrapping.PlaceOnNewLine.PlaceFinallyOnNewLine = finallyOnNewLine;
                }
            );
        }

        [TestCase(
            true,
            true,
@"
{
    try
    {
    }
    catch (Exception)
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
            true,
@"
{
    try
    {
    } catch (Exception)
    {
    } catch (Exception)
    {
    }
}
"
        )]
        [TestCase(
            true,
            false,
@"
{
    try
    {
    }
    catch (Exception)
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
            false,
@"
{
    try
    {
    } catch (Exception)
    {
    } catch (Exception)
    {
    }
}
"
        )]
        public void PlaceCatchFinallyOnNewLineWithoutFinally(bool catchOnNewLine, bool finallyOnNewLine, string expected)
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
                            ),
                            Syntax.CatchClause(
                                Syntax.CatchDeclaration(Syntax.ParseName("Exception")),
                                Syntax.Block()
                            )
                        }
                    )
                ),
                p =>
                {
                    p.LineBreaksAndWrapping.PlaceOnNewLine.PlaceCatchOnNewLine = catchOnNewLine;
                    p.LineBreaksAndWrapping.PlaceOnNewLine.PlaceFinallyOnNewLine = finallyOnNewLine;
                }
            );
        }
    }
}
