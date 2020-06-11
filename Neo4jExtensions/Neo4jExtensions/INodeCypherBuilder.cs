using System;
using System.Linq.Expressions;

namespace Neo4jExtensions
{
    public interface INodeCypherBuilder<T>
    {
        INodeCypherBuilder<T> Match(string target);
        IRelationCypherBuilder<TRel> Rel<TRel>(Expression<Func<IRelationCypherBuilder<TRel>, string>> relBuilder);
        INodeCypherBuilder<T> Where(Expression<Func<T, bool>> predicate);

        string Build();
    }
}