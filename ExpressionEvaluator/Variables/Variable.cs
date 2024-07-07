using Expr = ExpressionEvaluator.Expressions;
using ExpressionEvaluator.Interfaces;
using System.Linq.Expressions;

namespace ExpressionEvaluator.Variables
{
    /// <summary>
    /// Представляет переменную с именем и значением.
    /// </summary>
    public class Variable
    {
        /// <summary>
        /// Имя переменной.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Строковое значение переменной.
        /// </summary>
        public string StringValue { get; set; }

        /// <summary>
        /// Значение переменной.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса Variable с указанным именем и значением.
        /// </summary>
        /// <param name="name">Имя переменной.</param>
        /// <param name="stringValue">Строковое значение переменной.</param>
        /// <param name="variableManager">Менеджер переменных.</param>
        public Variable(string name, string stringValue)
        {
            Name = name;
            StringValue = stringValue;
        }

        public string FormattedText
        {
            get
            {
                return $"{Name}={StringValue}";
            }
        }
    }
}
