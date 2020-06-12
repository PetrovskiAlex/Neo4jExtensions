using System;
using System.Linq.Expressions;

namespace Neo4jExtensions
{
    public interface IRelationCypherBuilder<TRel>
    {
        INodeCypherBuilder<T> To<T>(Action<INodeCypherBuilder<T>> matchBuilder, string target);
        INodeCypherBuilder<T> From<T>(Action<INodeCypherBuilder<T>> matchBuilder, string target);
        IRelationCypherBuilder<TRel> Where(Expression<Func<TRel, bool>> predicate);

        string Build();
    }
}