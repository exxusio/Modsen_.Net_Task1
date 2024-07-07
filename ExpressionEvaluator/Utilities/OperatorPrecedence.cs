using System.Collections.Generic;

namespace ExpressionEvaluator.Utilities
{
    public class OperatorPrecedence
    {
        /// <summary>
        /// Словарь, содержащий приоритеты операторов.
        /// </summary>
        private readonly Dictionary<char, int> _precedence = new Dictionary<char, int>
        {
            { '(', 0 },
            { ')', 0 },
            { '~', 1 },
            { '+', 1 },
            { '-', 1 },
            { '*', 2 },
            { '/', 2 }
        };

        /// <summary>
        /// Определяет, имеет ли оператор op1 больший или равный приоритет, чем оператор op2.
        /// </summary>
        /// <param name="op1">Первый оператор для сравнения.</param>
        /// <param name="op2">Второй оператор для сравнения.</param>
        /// <returns>True, если оператор op1 имеет больший или равный приоритет по сравнению с оператором op2, иначе False.</returns>
        public bool HasPrecedence(char op1, char op2)
        {
            return _precedence[op1] >= _precedence[op2];
        }
    }
}
