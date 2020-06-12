using System;
using System.Linq.Expressions;

namespace Neo4jExtensions
{
    public interface INodeCypherBuilder<T>
    {
        void Rel<TRel>(string target, Action<IRelationCypherBuilder<TRel>> relBuilder = null);
        void Rel<TRel>(Action<IRelationCypherBuilder<TRel>> relBuilder = null);
        INodeCypherBuilder<T> Where(Expression<Func<T, bool>> predicate);

        string Build();
    }
}