using System.Linq.Expressions;
using System.Numerics;

namespace AoC.Common.AoCMath;

public static class ExpressionParser
{
    public static Func<T, T> ParseSimpleMathExpression<T>(string expression) where T : INumber<T>
    {
        var (parameter, body) = expression.Split("=>", StringSplitOptions.RemoveEmptyEntries);

        if (parameter.IsNullOrWhitespace())
            throw new ArgumentNullException(nameof(expression), "No parameter name was supplied for the input parameter");

        if (body.IsNullOrWhitespace())
            throw new ArgumentNullException(nameof(expression), "No body was supplied");

        parameter = parameter.Trim();

        var (left, @operator, right) = body.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (left.IsNullOrWhitespace())
            throw new ArgumentNullException(nameof(expression), "No left argument was specified in the body");

        if (right.IsNullOrWhitespace())
            throw new ArgumentNullException(nameof(expression), "No right argument was specified in the body");

        var paramExpression = Expression.Parameter(typeof(T), parameter);
        Expression leftVariable = left == parameter ? paramExpression : Expression.Constant(T.Parse(left, null));
        Expression rightVariable = right == parameter ? paramExpression : Expression.Constant(T.Parse(right, null));

        Expression operation = @operator switch
        {
            "+" => Expression.Add(leftVariable, rightVariable),
            "-" => Expression.Subtract(leftVariable, rightVariable),
            "/" => Expression.Divide(leftVariable, rightVariable),
            "*" => Expression.Multiply(leftVariable, rightVariable),
            "%" => Expression.Modulo(leftVariable, rightVariable),
            _ => throw new NotSupportedException($"Operator '{@operator}' is not a supported operator")
        };

        var lambda = Expression.Lambda<Func<T, T>>(operation, paramExpression);
        return lambda.Compile();
    }
}
