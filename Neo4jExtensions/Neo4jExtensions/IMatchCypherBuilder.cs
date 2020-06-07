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

    public interface IRelationCypherBuilder<TRel>
    {
        IMatchCypherBuilder<T> To<T>(Expression<Func<IMatchCypherBuilder<T>, string>> matchBuilder, string target);
        IMatchCypherBuilder<T> From<T>(Expression<Func<IMatchCypherBuilder<T>, string>> matchBuilder, string target);
        IRelationCypherBuilder<TRel> Where(Expression<Func<TRel, bool>> predicate);

        string Build();
    }

    class RelationCypherBuilder<TRel> : IRelationCypherBuilder<TRel>
    {
        public IMatchCypherBuilder<T> To<T>(Expression<Func<IMatchCypherBuilder<T>, string>> matchBuilder, string target)
        {
            IMatchCypherBuilder<T> matchCypherBuilder = new MatchCypherBuilder<T>();
            matchBuilder.Compile().Invoke(matchCypherBuilder);
            return matchCypherBuilder;
        }

        public IMatchCypherBuilder<T> From<T>(Expression<Func<IMatchCypherBuilder<T>, string>> matchBuilder, string target)
        {
            IMatchCypherBuilder<T> matchCypherBuilder = new MatchCypherBuilder<T>();
            matchBuilder.Compile().Invoke(matchCypherBuilder);
            return matchCypherBuilder;
        }

        public IRelationCypherBuilder<TRel> Where(Expression<Func<TRel, bool>> predicate)
        {
            return this;
        }

        public string Build()
        {
            return "Rel";
        }
    }
}