using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using ExpressionEvaluator.Functions;
using ExpressionEvaluator.Interfaces;
using ExpressionEvaluator.Tokens;
using ExpressionEvaluator.Utilities;
using ExpressionEvaluator.Variables;

namespace ExpressionEvaluator.Expressions
{
    /// <summary>
    /// Класс для вычисления математических выражений.
    /// </summary>
    public class ExpressionEvaluator : IExpressionEvaluator
    {
        private OperatorPrecedence _precedence = new OperatorPrecedence();
        private IVariableManager _variableManager = new VariableManager();
        private IFunctionManager _functionManager = new FunctionManager();
        private Stack<double> _numbers = new Stack<double>(); // Стек для хранения чисел
        private Stack<char> _sings = new Stack<char>(); // Стек для хранения операторов и скобок

        /// <summary>
        /// Инициализирует новый экземпляр класса ExpressionEvaluator с указанным менеджером переменных.
        /// </summary>
        /// <param name="variableManager">Менеджер переменных для вычисления выражений.</param>
        public ExpressionEvaluator(IVariableManager variableManager, IFunctionManager functionManager) 
        {
            _variableManager = variableManager;
            _functionManager = functionManager;
        }

        /// <summary>
        /// Вычисляет значение математического выражения.
        /// </summary>
        /// <param name="expression">Математическое выражение.</param>
        /// <returns>Результат вычисления выражения.</returns>
        public double Evaluate(string expression)
        {
            Validate(ref expression);

            Tokenizer tokenizer = new Tokenizer(expression);
            bool expectUnary = true; // Ожидается ли унарный оператор

            foreach (var token in tokenizer.GetTokens())
            {
                switch (token.Type)
                {
                    case TokenType.Number:
                        PushNumber(token.Value);
                        expectUnary = false;
                        break;

                    case TokenType.Variable:
                        PushVariable(token.Value);
                        expectUnary = false;
                        break;

                    case TokenType.Function:
                        HandleFunction(token.Value);
                        expectUnary = false;
                        break;

                    case TokenType.Operator:
                        HandleOperator(token.Value[0], ref expectUnary);
                        break;

                    case TokenType.Bracket:
                        HandleBracket(token.Value[0], ref expectUnary);
                        break;

                    default:
                        throw new ArgumentException($"Invalid operator type: {token.Type}");
                }
            }

            while (_sings.Count > 0)
                ApplyOperator();

            return _numbers.Pop();
        }

        /// <summary>
        /// Проверка корректности математического выражения.
        /// </summary>
        /// <param name="expression">Математическое выражение.</param>
        private void Validate(ref string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw new ArgumentNullException("Mathematical expression cannot be null.");

            if (!expression.Any(c => char.IsLetter(c) || char.IsDigit(c)))
                throw new FormatException("Incorrect format of the mathematical expression.");

            int countBracket = 0;
            foreach (char c in expression)
            {
                if (c == '(')
                    countBracket++;
                else if (c == ')')
                    countBracket--;
            }
            if (countBracket > 0)
                expression = expression.PadRight(expression.Length + countBracket, ')');
        }

        /// <summary>
        /// Обработка оператора.
        /// </summary>
        /// <param name="currentNumber">Текущее считываемое число.</param>
        /// <param name="ch">Символ оператора.</param>
        /// <param name="expectUnary">Ожидание унарного оператора.</param>
        private void HandleOperator(char ch, ref bool expectUnary)
        {
            if (ch == '-' && expectUnary)
            {
                _sings.Push('~'); // Используем '~' для обозначения унарного минуса
                expectUnary = true;
            }
            else
            {
                while (_sings.Count > 0 && _precedence.HasPrecedence(_sings.Peek(), ch))
                    ApplyOperator();

                _sings.Push(ch);
                expectUnary = true;
            }
        }

        /// <summary>
        /// Обработка скобки.
        /// </summary>
        /// <param name="currentNumber">Текущее считываемое число.</param>
        /// <param name="ch">Символ скобки.</param>
        /// <param name="expectUnary">Ожидание унарного оператора.</param>
        private void HandleBracket(char ch, ref bool expectUnary)
        {
            if (ch == '(')
            {
                _sings.Push(ch);
                expectUnary = true;
            }
            else if (ch == ')')
            {
                while (_sings.Peek() != '(')
                    ApplyOperator();

                _sings.Pop();
                expectUnary = false;
            }
        }

        /// <summary>
        /// Обработка функции.
        /// </summary>
        /// <param name="currentFunction">Текущее считываемая функция.</param>
        private void HandleFunction(string currentFunction)
        {
            var (name, parameters) = _functionManager.ParseFunctionDeclaration(currentFunction);
            var function = _functionManager.GetFunction(name, parameters.Count);

            IVariableManager variableManager = new VariableManager();
            IExpressionEvaluator expressionEvaluator = new ExpressionEvaluator(_variableManager, _functionManager);

            if (function.Parameters.Count != 0)
                for (int i = 0; i < function.Parameters.Count; i++)
                    variableManager.SetVariable(function.Parameters[i],
                        Convert.ToString(expressionEvaluator.Evaluate(parameters[i])));

            expressionEvaluator = new ExpressionEvaluator(variableManager, null);
            double result = expressionEvaluator.Evaluate(function.Expression);

            _numbers.Push(result);
        }

        /// <summary>
        /// Добавляет число в стек чисел.
        /// </summary>
        /// <param name="currentNumber">Текущее считываемое число.</param>
        private void PushNumber(string currentNumber)
        {
            currentNumber = currentNumber.Replace(',', '.');
            _numbers.Push(double.Parse(currentNumber, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Добавляет переменную в стек чисел.
        /// </summary>
        /// <param name="currentVariable">Текущея считываемая переменная.</param>
        private void PushVariable(string currentVariable)
        {
            double variable = _variableManager.GetVariableValue(currentVariable);
            _numbers.Push(variable);
        }

        /// <summary>
        /// Применяет оператор из стека операторов к двум верхним числам в стеке чисел.
        /// </summary>
        private void ApplyOperator()
        {
            double a, b;

            char sing = _sings.Pop();
            if (sing == '~')
            {
                a = _numbers.Pop();
                _numbers.Push(-a);
            }
            else
            {
                if (_numbers.Count > 1)
                {
                    b = _numbers.Pop();
                    a = _numbers.Pop();
                }
                else
                    throw new ArgumentException("Incorrect format of the mathematical expression.");

                if (sing == '/' && b == 0)
                    throw new DivideByZeroException("Division by zero is not possible.");

                _numbers.Push(MathematicalOperation(sing, a, b));
            }
        }

        /// <summary>
        /// Выполняет математическую операцию между двумя числами с указанным оператором.
        /// </summary>
        /// <param name="a">Первое число.</param>
        /// <param name="b">Второе число.</param>
        /// <param name="sing">Оператор.</param>
        /// <returns>Результат операции.</returns>
        private static double MathematicalOperation(char sing, double a, double b)
        {
            switch (sing)
            {
                case '+':
                    return a + b;
                case '-':
                    return a - b;
                case '*':
                    return a * b;
                case '/':
                    return a / b;
                default:
                    throw new ArgumentException($"Invalid operator: {sing}");
            }
        }
    }
}
