using System;
using System.Linq.Expressions;

namespace Neo4jExtensions
{
    public interface IMatchCypherBuilder<T>
    {
        IMatchCypherBuilder<T> Match(string target = "x");
        IRelationCypherBuilder<TRel> Rel<TRel>(Expression<Func<IRelationCypherBuilder<TRel>, string>> relBuilder, string rel);
        IMatchCypherBuilder<T> Where(Expression<Func<T, bool>> predicate);

        string Build();
    }
}