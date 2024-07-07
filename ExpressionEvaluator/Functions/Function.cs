using System.Collections.Generic;

namespace ExpressionEvaluator.Functions
{
    /// <summary>
    /// Представляет пользовательскую функцию с именем и списком параметров.
    /// </summary>
    public class Function
    {
        /// <summary>
        /// Получает имя функции.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Получает список параметров функции.
        /// </summary>
        public List<string> Parameters { get; }

        /// <summary>
        /// Получает выражение функции.
        /// </summary>
        public string Expression { get; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Function"/> с указанным именем, параметрами и выражением.
        /// </summary>
        /// <param name="name">Имя функции.</param>
        /// <param name="parameters">Список параметров функции.</param>
        /// <param name="expression">Выражение функции.</param>
        public Function(string name, List<string> parameters, string expression)
        {
            Name = name;
            Parameters = parameters;
            Expression = expression;
        }
        public string FormattedText
        {
            get
            {
                string parametersJoined = string.Join(",", Parameters);
                return $"{Name}({parametersJoined})={Expression}";
            }
        }
    }

}
