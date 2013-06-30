using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class LinqExpressionFixture : TestBase
    {
        [Test]
        public void SimpleSelect()
        {
            Test(
@"from i in j
    select i",
                Syntax.QueryExpression(
                    Syntax.FromClause("i", Syntax.ParseName("j")),
                    Syntax.QueryBody(
                        Syntax.SelectClause(Syntax.ParseName("i"))
                    )
                )
            );
        }

        [Test]
        public void SelectWithType()
        {
            Test(
@"from int i in j
    select i",
                Syntax.QueryExpression(
                    Syntax.FromClause(Syntax.ParseName("int"), "i", Syntax.ParseName("j")),
                    Syntax.QueryBody(
                        Syntax.SelectClause(Syntax.ParseName("i"))
                    )
                )
            );
        }

        [Test]
        public void SimpleJoin()
        {
            Test(
@"from i in j
    join k in l on i equals k
    select i",
                Syntax.QueryExpression(
                    Syntax.FromClause("i", Syntax.ParseName("j")),
                    Syntax.QueryBody(
                        new[]
                        {
                            Syntax.JoinClause(
                                "k",
                                Syntax.ParseName("l"),
                                Syntax.ParseName("i"),
                                Syntax.ParseName("k")
                            )
                        },
                        Syntax.SelectClause(Syntax.ParseName("i"))
                    )
                )
            );
        }

        [Test]
        public void JoinWithType()
        {
            Test(
@"from i in j
    join int k in l on i equals k
    select i",
                Syntax.QueryExpression(
                    Syntax.FromClause("i", Syntax.ParseName("j")),
                    Syntax.QueryBody(
                        new[]
                        {
                            Syntax.JoinClause(
                                Syntax.ParseName("int"),
                                "k",
                                Syntax.ParseName("l"),
                                Syntax.ParseName("i"),
                                Syntax.ParseName("k")
                            )
                        },
                        Syntax.SelectClause(Syntax.ParseName("i"))
                    )
                )
            );
        }

        [Test]
        public void JoinWithInto()
        {
            Test(
@"from i in j
    join k in l on i equals k into m
    select i",
                Syntax.QueryExpression(
                    Syntax.FromClause("i", Syntax.ParseName("j")),
                    Syntax.QueryBody(
                        new[]
                        {
                            Syntax.JoinClause(
                                "k",
                                Syntax.ParseName("l"),
                                Syntax.ParseName("i"),
                                Syntax.ParseName("k"),
                                Syntax.JoinIntoClause("m")
                            )
                        },
                        Syntax.SelectClause(Syntax.ParseName("i"))
                    )
                )
            );
        }

        [Test]
        public void WithLet()
        {
            Test(
@"from i in j
    let k = i
    select i",
                Syntax.QueryExpression(
                    Syntax.FromClause("i", Syntax.ParseName("j")),
                    Syntax.QueryBody(
                        new[]
                        {
                            Syntax.LetClause("k", Syntax.ParseName("i"))
                        },
                        Syntax.SelectClause(Syntax.ParseName("i"))
                    )
                )
            );
        }

        [Test]
        public void SingleOrderBy()
        {
            Test(
@"from i in j
    orderby i
    select i",
                Syntax.QueryExpression(
                    Syntax.FromClause("i", Syntax.ParseName("j")),
                    Syntax.QueryBody(
                        new[]
                        {
                            Syntax.OrderByClause(
                                Syntax.Ordering(expression: Syntax.ParseName("i"))
                            )
                        },
                        Syntax.SelectClause(Syntax.ParseName("i"))
                    )
                )
            );
        }

        [Test]
        public void OrderByDescending()
        {
            Test(
@"from i in j
    orderby i descending
    select i",
                Syntax.QueryExpression(
                    Syntax.FromClause("i", Syntax.ParseName("j")),
                    Syntax.QueryBody(
                        new[]
                        {
                            Syntax.OrderByClause(
                                Syntax.Ordering(
                                    AscendingOrDescending.Descending,
                                    Syntax.ParseName("i")
                                )
                            )
                        },
                        Syntax.SelectClause(Syntax.ParseName("i"))
                    )
                )
            );
        }

        [Test]
        public void MultipleOrderBys()
        {
            Test(
@"from i in j
    orderby i, i
    select i",
                Syntax.QueryExpression(
                    Syntax.FromClause("i", Syntax.ParseName("j")),
                    Syntax.QueryBody(
                        new[]
                        {
                            Syntax.OrderByClause(
                                Syntax.Ordering(expression: Syntax.ParseName("i")),
                                Syntax.Ordering(expression: Syntax.ParseName("i"))
                            )
                        },
                        Syntax.SelectClause(Syntax.ParseName("i"))
                    )
                )
            );
        }

        [Test]
        public void WithWhere()
        {
            Test(
@"from i in j
    where i > 0
    select i",
                Syntax.QueryExpression(
                    Syntax.FromClause("i", Syntax.ParseName("j")),
                    Syntax.QueryBody(
                        new[]
                        {
                            Syntax.WhereClause(
                                Syntax.BinaryExpression(
                                    BinaryOperator.GreaterThan,
                                    Syntax.ParseName("i"),
                                    Syntax.LiteralExpression(0)
                                )
                            )
                        },
                        Syntax.SelectClause(Syntax.ParseName("i"))
                    )
                )
            );
        }

        [Test]
        public void GroupBy()
        {
            Test(
@"from i in j
    group i by k
    select i",
                Syntax.QueryExpression(
                    Syntax.FromClause("i", Syntax.ParseName("j")),
                    Syntax.QueryBody(
                        selectOrGroup: Syntax.GroupClause(
                            Syntax.ParseName("i"),
                            Syntax.ParseName("k")
                        ),
                        continuation: Syntax.QueryContinuation(
                            body: Syntax.QueryBody(
                                Syntax.SelectClause(Syntax.ParseName("i"))
                            )
                        )
                    )
                )
            );
        }

        [Test]
        public void GroupInto()
        {
            Test(
@"from i in j
    group i by k into l
    select i",
                Syntax.QueryExpression(
                    Syntax.FromClause("i", Syntax.ParseName("j")),
                    Syntax.QueryBody(
                        selectOrGroup: Syntax.GroupClause(
                            Syntax.ParseName("i"),
                            Syntax.ParseName("k")
                        ),
                        continuation: Syntax.QueryContinuation(
                            "l",
                            Syntax.QueryBody(
                                Syntax.SelectClause(Syntax.ParseName("i"))
                            )
                        )
                    )
                )
            );
        }
    }
}
