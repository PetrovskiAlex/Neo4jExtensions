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

        public RelationCypherBuilder(string target)
        {
            _relation = $"{target}:" + CypherExtensions.GetNodeName<TRel>();
        }

        public RelationCypherBuilder() : this("r")
        {
            
        }

        public void To<T>(string target, Action<INodeCypherBuilder<T>> matchBuilder = null)
        {
            var nodeCypherBuilder = new NodeCypherBuilder<T>(target);

            matchBuilder?.Invoke(nodeCypherBuilder);

            _toNode = nodeCypherBuilder.Build();
        }

        public void From<T>(string target, Action<INodeCypherBuilder<T>> matchBuilder = null)
        {
            var nodeCypherBuilder = new NodeCypherBuilder<T>(target);

            matchBuilder?.Invoke(nodeCypherBuilder);

            _fromNode = nodeCypherBuilder.Build();
        }

        public IRelationCypherBuilder<TRel> Where(Expression<Func<TRel, bool>> predicate)
        {
            var pattern = CypherExtensions.BuildFilterPattern(predicate);
            _patterns.Add(pattern);

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