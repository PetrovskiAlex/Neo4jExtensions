using System;
using System.Linq.Expressions;

namespace Neo4jExtensions
{
    public interface IMatchCypherBuilder<T>
    {
        IMatchCypherBuilder<T> Match(string target = "x");
        IRelationCypherBuilder<TRel> Rel<TRel>(Func<IRelationCypherBuilder<TRel>, string> relBuilder, string rel = "r");
        IMatchCypherBuilder<T> Where(Expression<Func<T, bool>> predicate);

        string Build();
    }

    public interface IRelationCypherBuilder<TRel>
    {
        IMatchCypherBuilder<T> To<T>(Func<IMatchCypherBuilder<T>, string> matchBuilder, string target = "xt");
        IMatchCypherBuilder<T> From<T>(Func<IMatchCypherBuilder<T>, string> matchBuilder, string target = "xt");
        IRelationCypherBuilder<TRel> Where(Expression<Func<TRel, bool>> predicate);

        string Build();
    }
}