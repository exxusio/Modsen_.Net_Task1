using ExpressionEvaluator.Variables;
using ExpressionEvaluator.Interfaces;
using Expr = ExpressionEvaluator.Expressions;
using ExpressionEvaluator.Functions;

namespace Modsen_dotnet_Task1.Models
{
    /// <summary>
    /// Калькулятор для управления переменными, функциями и вычисления математических выражений.
    /// </summary>
    public class Calculator
    {
        private readonly IVariableManager _variableManager;
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IFunctionManager _functionManager;

        /// <summary>
        /// Инициализирует новый экземпляр класса Calculator.
        /// </summary>
        public Calculator()
        {
            _variableManager = new VariableManager();
            _functionManager = new FunctionManager();
            _expressionEvaluator = new Expr.ExpressionEvaluator(_variableManager, _functionManager);
        }

        /// <summary>
        /// Менеджер переменных для вычислений.
        /// </summary>
        public IVariableManager Variable => _variableManager;

        /// <summary>
        /// Менеджер функций для вычислений.
        /// </summary>
        public IFunctionManager Function => _functionManager;

        /// <summary>
        /// Вычислитель математических выражений.
        /// </summary>
        public IExpressionEvaluator Evaluator => _expressionEvaluator;
    }

}
