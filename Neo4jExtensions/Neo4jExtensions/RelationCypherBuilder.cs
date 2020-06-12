using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Neo4jExtensions
{
    public class RelationCypherBuilder<TRel> : IRelationCypherBuilder<TRel>
    {
        private string _relation;
        private string _toNode;
        private string _fromNode;
        private List<string> _patterns = new List<string>();
        
        public INodeCypherBuilder<T> To<T>(Action<INodeCypherBuilder<T>> matchBuilder, string target)
        {
            INodeCypherBuilder<T> nodeCypherBuilder = new NodeCypherBuilder<T>();

            _relation = $"{target}:" + CypherExtensions.GetNodeName<TRel>();
            matchBuilder(nodeCypherBuilder);
            _toNode = nodeCypherBuilder.Build();
            return nodeCypherBuilder;
        }

        public INodeCypherBuilder<T> From<T>(Action<INodeCypherBuilder<T>> matchBuilder, string target)
        {
            INodeCypherBuilder<T> nodeCypherBuilder = new NodeCypherBuilder<T>();

            _relation = $"{target}:" + CypherExtensions.GetNodeName<TRel>();
            matchBuilder(nodeCypherBuilder);
            _fromNode = nodeCypherBuilder.Build();

            return nodeCypherBuilder;
        }

        public IRelationCypherBuilder<TRel> Where(Expression<Func<TRel, bool>> predicate)
        {
            var patterns = CypherExtensions.BuildFilterPatterns(new [] {predicate});
            _patterns.AddRange(patterns);

            return this;
        }

        public string Build()
        {
            if (!string.IsNullOrWhiteSpace(_toNode))
            {
                return $"-[{_relation}{{ {string.Join(", ", _patterns)}}}]->{_toNode}";
            }
            else if (!string.IsNullOrWhiteSpace(_fromNode))
            {
                return $"<-[{_relation}{{ {string.Join(", ", _patterns)}}}]-{_fromNode}";
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}