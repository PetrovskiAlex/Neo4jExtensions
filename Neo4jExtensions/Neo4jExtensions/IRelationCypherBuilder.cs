using System;
using System.Linq.Expressions;

namespace Neo4jExtensions
{
    public interface IRelationCypherBuilder<TRel>
    {
        void To<T>(string target, Action<INodeCypherBuilder<T>> matchBuilder = null);
        void From<T>(string target, Action<INodeCypherBuilder<T>> matchBuilder = null);
        IRelationCypherBuilder<TRel> Where(Expression<Func<TRel, bool>> predicate);

        string Build();
    }
}