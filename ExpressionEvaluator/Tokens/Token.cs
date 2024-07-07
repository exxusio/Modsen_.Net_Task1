namespace ExpressionEvaluator.Tokens
{
    /// <summary>
    /// Представляет токен математического выражения.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Тип токена.
        /// </summary>
        public TokenType Type { get; }

        /// <summary>
        /// Значение токена.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Инициализирует новый экземпляр класса Token с указанным типом и значением.
        /// </summary>
        /// <param name="type">Тип токена.</param>
        /// <param name="value">Значение токена.</param>
        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }
    }

    /// <summary>
    /// Перечисление, представляющее типы токенов.
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// Число.
        /// </summary>
        Number,

        /// <summary>
        /// Оператор.
        /// </summary>
        Operator,

        /// <summary>
        /// Скобка.
        /// </summary>
        Bracket,

        /// <summary>
        /// Переменная.
        /// </summary>
        Variable,

        /// <summary>
        /// Функция.
        /// </summary>
        Function
    }
}
