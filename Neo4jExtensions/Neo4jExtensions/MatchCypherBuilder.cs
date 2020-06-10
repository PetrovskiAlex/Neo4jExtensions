using System;
using System.Linq.Expressions;

namespace Neo4jExtensions
{
    class MatchCypherBuilder<T> : IMatchCypherBuilder<T>
    {
        public IMatchCypherBuilder<T> Match(string target = "x")
        {
            return this;
        }

        public IRelationCypherBuilder<TRel> Rel<TRel>(Expression<Func<IRelationCypherBuilder<TRel>, string>> relBuilder, string rel)
        {
            var relationCypherBuilder = new RelationCypherBuilder<TRel>();
            var invoke = relBuilder.Compile().Invoke(relationCypherBuilder);
            return relationCypherBuilder;
        }

        public IMatchCypherBuilder<T> Where(Expression<Func<T, bool>> predicate)
        {
            return this;
        }

        public string Build()
        {
            return "Match";
        }
    }
}