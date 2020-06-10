using System;
using System.Linq.Expressions;

namespace Neo4jExtensions
{
    class RelationCypherBuilder<TRel> : IRelationCypherBuilder<TRel>
    {
        public string Query { get; private set; }
        
        public IMatchCypherBuilder<T> To<T>(Expression<Func<IMatchCypherBuilder<T>, string>> matchBuilder, string target)
        {
            IMatchCypherBuilder<T> matchCypherBuilder = new MatchCypherBuilder<T>();

            var relName = CypherExtensions.GetNodeName<TRel>();
            var s = $"-[{target}:{relName}]->";

            var matchResult = matchBuilder.Compile().Invoke(matchCypherBuilder);
            Query = s + matchResult;

            return matchCypherBuilder;
        }

        public IMatchCypherBuilder<T> From<T>(Expression<Func<IMatchCypherBuilder<T>, string>> matchBuilder, string target)
        {
            IMatchCypherBuilder<T> matchCypherBuilder = new MatchCypherBuilder<T>();

            var relName = CypherExtensions.GetNodeName<TRel>();
            var s = $"<-[{target}:{relName}]-";

            var matchResult = matchBuilder.Compile().Invoke(matchCypherBuilder);
            Query = s + matchResult;

            return matchCypherBuilder;
        }

        public IRelationCypherBuilder<TRel> Where(Expression<Func<TRel, bool>> predicate)
        {
            return this;
        }

        public string Build()
        {
            return Query;
        }
    }
}