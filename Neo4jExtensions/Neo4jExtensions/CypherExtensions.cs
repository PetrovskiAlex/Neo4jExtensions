using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Neo4jExtensions
{
    internal static class CypherExtensions
    {
        public static string BuildFilterPattern<T>(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) return null;

            if (predicate.NodeType != ExpressionType.Lambda)
                throw new NotSupportedException("Only Lambda expression is supported");

            var binaryExpression = predicate.Body as BinaryExpression;
            if (binaryExpression == null)
            {
                throw new NotSupportedException("Only binary expression is supported");
            }

            if (binaryExpression.NodeType != ExpressionType.Equal)
            {
                throw new NotSupportedException("Only Equal operator is supported");
            }

            var (type, memberName) = GetMemberName(binaryExpression.Left);

            var stringValue = GetValue(binaryExpression);

            return $"{memberName} : {stringValue}";

            (Type Type, string Name) GetMemberName(Expression expression)
            {
                MemberExpression memberExpression;
                if (expression is UnaryExpression unaryExp)
                {
                    memberExpression = (MemberExpression) unaryExp.Operand;
                }
                else
                {
                    memberExpression = (MemberExpression) expression;
                }

                return (memberExpression.Type, memberExpression.Member.Name);
            }

            string GetValue(BinaryExpression expression)
            {
                var objectMember = Expression.Convert(expression.Right, typeof(object));
                var getterLambda = Expression.Lambda<Func<object>>(objectMember);
                var value = getterLambda.Compile()();

                var stringVal = value.ToString();
                if (value is bool)
                {
                    stringVal = value.ToString().ToLower();
                }
                else if (type.IsEnum)
                {
                    stringVal = Enum.GetName(type, value);
                }

                if (type.IsEnum || type == typeof(string) || type == typeof(Guid))
                {
                    stringVal = $"\"{stringVal}\"";
                }

                return stringVal;
            }
        }

        public static string GetNodeName<T>()
        {
            var type = typeof(T);
            var graphElementAttribute = type.GetCustomAttribute<GraphElementAttribute>();
            return graphElementAttribute?.Name ?? typeof(T).Name;
        }
    }
}