using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace TestProject1
{
    public static class PropertyPath<TSource>
    {
        private class Segment
        {
            public string Name { get; set; }
            public bool IsArrayIndex { get; init; }
        }

        public static string Get<TProperty>(Expression<Func<TSource, TProperty>> expression)
        {
            var path = string.Join(".", GetSegments(expression));
            return path.Length > 0 ? path : null;
        }

        public static IEnumerable<string> GetSegments<TProperty>(Expression<Func<TSource, TProperty>> expression)
            => GetSegments(expression.Body).Select(o => o.Name);

        private static IEnumerable<Segment> GetSegments(Expression expression)
        {
            var segments = new LinkedList<Segment>();

            do
            {
                if (expression.NodeType == ExpressionType.MemberAccess)
                {
                    var memberExpression = (MemberExpression) expression;
                    var lastProcessedSegment = segments.First?.Value;
                    var currentMemberName = memberExpression.Member.Name;

                    if (lastProcessedSegment?.IsArrayIndex == true)
                        lastProcessedSegment.Name = $"{currentMemberName}[{lastProcessedSegment.Name}]";
                    else
                        segments.AddFirst(new Segment {Name = currentMemberName});

                    expression = memberExpression.Expression;
                }
                else if (expression.NodeType == ExpressionType.ArrayIndex)
                {
                    var binaryExpression = (BinaryExpression) expression;
                    var arrayIndex = GetArrayIndex(binaryExpression.Right);

                    segments.AddFirst(new Segment
                    {
                        Name = arrayIndex.ToString(),
                        IsArrayIndex = true
                    });

                    expression = binaryExpression.Left;
                }
                else
                    expression = null; // todo: throw
            } while (expression != null);

            return segments;
        }

        private static object GetArrayIndex(Expression expression)
        {
            if (expression is MemberExpression memberExpression)
            {
                if (memberExpression.Expression is ConstantExpression constantExpression)
                {
                    if (memberExpression.Member is FieldInfo fieldInfo)
                        return fieldInfo.GetValue(constantExpression.Value);

                    if (memberExpression.Member is PropertyInfo propertyInfo)
                        return propertyInfo.GetValue(constantExpression.Value);
                }
            }
            else if (expression is ConstantExpression constantExpression)
                return constantExpression.Value;

            return null; // todo: throw
        }
    }
}