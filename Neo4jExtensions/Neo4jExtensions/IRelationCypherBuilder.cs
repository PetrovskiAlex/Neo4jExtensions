using System;
using System.Linq.Expressions;

namespace Neo4jExtensions
{
    public interface IRelationCypherBuilder<TRel>
    {
        INodeCypherBuilder<T> To<T>(Expression<Func<INodeCypherBuilder<T>, string>> matchBuilder, string target);
        INodeCypherBuilder<T> From<T>(Expression<Func<INodeCypherBuilder<T>, string>> matchBuilder, string target);
        IRelationCypherBuilder<TRel> Where(Expression<Func<TRel, bool>> predicate);

        string Build();
    }
}