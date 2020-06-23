using System.Linq.Expressions;

namespace NP.Concepts.Expressions
{
    /// <summary>
    /// used in order to plugin parameter value getter
    /// into a MethodCall expression, instead of ParamExpresion
    /// </summary>
    public interface IParamValGetterExpressionInfo
    {
        string ParamName { get; }

        Expression ValueGetterExpression { get; }
    }

    public class ParamValGetterExpressionInfo : IParamValGetterExpressionInfo
    {
        public string ParamName { get; }

        public Expression ValueGetterExpression { get; }

        public ParamValGetterExpressionInfo(string paramName, Expression valueGetterExpression)
        {
            ParamName = paramName;
            ValueGetterExpression = valueGetterExpression;
        }
    }

    public static class ParamValGetterExpressionHelper
    {
        public static Expression GetCallExpression<T>(T methodContainer, string methodName)
        {
            Expression callExpression = Expression.Call
            (
                Expression.Constant(methodContainer),
                typeof(T).GetMethod(methodName)
            );

            return callExpression;
        }
    }
}
