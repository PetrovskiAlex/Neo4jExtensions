using System;
using System.Linq.Expressions;

namespace Neo4jExtensions
{
    public interface IRelationCypherBuilder<TRel>
    {
        IMatchCypherBuilder<T> To<T>(Expression<Func<IMatchCypherBuilder<T>, string>> matchBuilder, string target);
        IMatchCypherBuilder<T> From<T>(Expression<Func<IMatchCypherBuilder<T>, string>> matchBuilder, string target);
        IRelationCypherBuilder<TRel> Where(Expression<Func<TRel, bool>> predicate);

        string Build();
    }
}