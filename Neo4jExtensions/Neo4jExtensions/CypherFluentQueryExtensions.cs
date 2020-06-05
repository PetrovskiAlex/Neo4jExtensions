using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Neo4jClient.Cypher;

namespace Neo4jExtensions
{
    public static class CypherFluentQueryExtensions
    {
        public static IMatchCypherBuilder<T> MatchBuilder<T>(this ICypherFluentQuery query)
        {
            throw new NotImplementedException();
        }
        
        public static ICypherFluentQuery Match<T>([NotNull] this ICypherFluentQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return InnerMatch<T>(query);
        }

        public static ICypherFluentQuery Match<T>([NotNull] this ICypherFluentQuery query, string targetName)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return InnerMatch<T>(query, targetName);
        }

        public static ICypherFluentQuery Match<T>([NotNull] this ICypherFluentQuery query, params Expression<Func<T, bool>>[] selectors)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return InnerMatch(query, null, selectors);
        }

        public static ICypherFluentQuery Match<T>([NotNull] this ICypherFluentQuery query, string targetName, params Expression<Func<T, bool>>[] selectors)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return InnerMatch<T>(query, targetName, selectors);
        }

        private static ICypherFluentQuery InnerMatch<T>(this ICypherFluentQuery query, string targetName = null, params Expression<Func<T, bool>>[] selectors)
        {
            var nodeName = GetNodeName<T>();

            var patterns = BuildFilterPatterns(selectors);
            query = query.Match($"({targetName}:{nodeName}{ patterns })");

            return query;
        }

        public static ICypherFluentQuery HasRightRelationshipTo<TRel, TNode>([NotNull] this ICypherFluentQuery query, 
            string targetNameRel = null,
            Expression<Func<TRel, bool>>[] predicatesRel = null,
            string targetNameNode = null,
            Expression<Func<TNode, bool>>[] predicatesNode = null)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return HasRelationship(query, RelationshipDirection.Right, targetNameRel, predicatesRel, targetNameNode, predicatesNode);
        }
        
        public static ICypherFluentQuery HasLeftRelationshipTo<TRel, TNode>([NotNull] this ICypherFluentQuery query, 
            string targetNameRel = null,
            Expression<Func<TRel, bool>>[] predicatesRel = null,
            string targetNameNode = null,
            Expression<Func<TNode, bool>>[] predicatesNode = null)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return HasRelationship(query, RelationshipDirection.Left, targetNameRel, predicatesRel, targetNameNode, predicatesNode);
        }
        
        private static ICypherFluentQuery HasRelationship<TRel, TNode>([NotNull] this ICypherFluentQuery query, 
            RelationshipDirection direction,
            string targetNameRel = null,
            Expression<Func<TRel, bool>>[] predicatesRel = null,
            string targetNameNode = null,
            Expression<Func<TNode, bool>>[] predicatesNode = null)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var queryWriter = (QueryWriter) query.GetType()
                .GetField("QueryWriter", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(query);

            var relationshipName = GetNodeName<TRel>();
            var nodeName = GetNodeName<TNode>();

            var relationshipPatterns = BuildFilterPatterns(predicatesRel);
            var nodePatterns = BuildFilterPatterns(predicatesNode);

            if (direction == RelationshipDirection.Left)
            {
                queryWriter
                    .AppendClause($"<-[{targetNameRel}:{relationshipName}{relationshipPatterns}]-({targetNameNode}:{nodeName}{nodePatterns})");

                return query;
            }
            else if(direction == RelationshipDirection.Right)
            {
                queryWriter
                    .AppendClause($"-[{targetNameRel}:{relationshipName}{relationshipPatterns}]->({targetNameNode}:{nodeName}{nodePatterns})");

                return query;
            }

            throw new NotImplementedException();
        }
        
        private static string BuildFilterPatterns<T>(Expression<Func<T, bool>>[] selectors)
        {
            if (selectors == null || !selectors.Any()) return null;

            var patterns = new List<string>();

            foreach (var selector in selectors)
            {
                if (selector.NodeType != ExpressionType.Lambda)
                    throw new NotSupportedException("Only Lambda expression is supported");

                var binaryExpression = selector.Body as BinaryExpression;
                if (binaryExpression == null)
                {
                    throw new NotSupportedException("Only binary expression is supported");
                }

                if (binaryExpression.NodeType != ExpressionType.Equal)
                {
                    throw new NotSupportedException("Only Equal operator is supported");
                }

                var (type, memberName) = GetMemberName(binaryExpression.Left);

                var stringValue = GetValue(binaryExpression, type);

                var pattern = $"{memberName} : {stringValue}";
                patterns.Add(pattern);
            }

            return "{ " + string.Join(", ", patterns) + " }";

            (Type Type, string Name) GetMemberName(Expression binaryExpression)
            {
                MemberExpression memberExpression;
                if (binaryExpression is UnaryExpression unaryExp)
                {
                    memberExpression = (MemberExpression) unaryExp.Operand;
                }
                else
                {
                    memberExpression = (MemberExpression) binaryExpression;
                }

                var memberName = memberExpression.Member.Name;
                return (memberExpression.Type, memberName);
            }

            string GetValue(BinaryExpression binaryExpression, Type type)
            {
                var objectMember = Expression.Convert(binaryExpression.Right, typeof(object));
                var getterLambda = Expression.Lambda<Func<object>>(objectMember);
                var value = getterLambda.Compile()();

                var stringValue = value.ToString();
                if (value is bool)
                {
                    stringValue = value.ToString().ToLower();
                }
                else if (type.IsEnum)
                {
                    stringValue = Enum.GetName(type, value);
                }

                if (type.IsEnum || type == typeof(string) || type == typeof(Guid))
                {
                    stringValue = $"\"{stringValue}\"";
                }

                return stringValue;
            }
        }

        private static string GetNodeName<T>()
        {
            var type = typeof(T);
            var graphElementAttribute = type.GetCustomAttribute<GraphElementAttribute>();
            return graphElementAttribute?.Name ?? typeof(T).Name;
        }
        
        private enum RelationshipDirection
        {
            Left,
            Right
        }
    }
}