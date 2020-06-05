using System;
using System.Linq.Expressions;
using Neo4jClient.Cypher;

namespace Neo4jExtensions
{
    public interface IMatchCypherBuilder<T>
    {
        IMatchCypherBuilder<T> Match(string target = "x");
        IRelationCypherBuilder<TRel> Rel<TRel>(string rel = "r");
        IMatchCypherBuilder<T> Where(Expression<Func<T, bool>> predicate);

        ICypherFluentQuery Build();
    }

    public interface IRelationCypherBuilder<TRel>
    {
        IMatchCypherBuilder<T> To<T>(string target = "xt");
        IMatchCypherBuilder<T> From<T>(string target = "xt");
        IRelationCypherBuilder<TRel> Where(Expression<Func<TRel, bool>> predicate);
    }
}