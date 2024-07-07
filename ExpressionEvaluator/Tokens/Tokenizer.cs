using System.Collections.Generic;

namespace ExpressionEvaluator.Tokens
{
    public class Tokenizer
    {
        private readonly string _expression;
        private readonly List<Token> _tokens;

        /// <summary>
        /// Инициализирует новый экземпляр класса Tokenizer с указанным выражением.
        /// </summary>
        /// <param name="expression">Математическое выражение.</param>
        public Tokenizer(string expression)
        {
            _expression = expression;
            _tokens = new List<Token>();
            Tokenize();
        }

        /// <summary>
        /// Разбирает выражение на токены.
        /// </summary>
        private void Tokenize()
        {
            string currentNumber = ""; // Текущее цисло
            string variableOrFunction = ""; // Текушая переменная или функция

            for (int i = 0; i < _expression.Length; i++)
            {
                char ch = _expression[i];

                if (char.IsDigit(ch) || ch == ',' || ch == '.')
                {
                    if (!string.IsNullOrEmpty(variableOrFunction))
                        variableOrFunction += ch;
                    else
                        currentNumber += ch;
                }
                else if (char.IsLetter(ch) || ch == '_')
                {
                    AddToken(ref currentNumber, TokenType.Number);
                    variableOrFunction += ch;
                }
                else
                {
                    AddToken(ref currentNumber, TokenType.Number);

                    if (!string.IsNullOrEmpty(variableOrFunction) && ch == '(')
                    {
                        int bracketCount = 0;
                        for (int j = i; j < _expression.Length; j++)
                        {
                            ch = _expression[j];
                            variableOrFunction += ch;
                            
                            if(ch == '(')
                                bracketCount++;
                            else if (ch == ')')
                                bracketCount--;
                            
                            if(bracketCount == 0)
                            {
                                AddToken(ref variableOrFunction, TokenType.Function);
                                i = j;
                                break;
                            }
                        }
                        continue;
                    }

                    AddToken(ref variableOrFunction, TokenType.Variable);

                    if (IsOperator(ch))
                        _tokens.Add(new Token(TokenType.Operator, ch.ToString()));
                    else if (IsBracket(ch))
                        _tokens.Add(new Token(TokenType.Bracket, ch.ToString()));
                }
            }

            AddToken(ref currentNumber, TokenType.Number);
            AddToken(ref variableOrFunction, TokenType.Variable);
        }

        /// <summary>
        /// Добавляет текущую строку в список токенов и сбрасывает её значение.
        /// </summary>
        /// <param name="current">Текущая строка, представляющая число или переменную.</param>
        /// <param name="type">Тип токена (число или переменная).</param>
        private void AddToken(ref string current, TokenType type)
        {
            if (!string.IsNullOrEmpty(current))
            {
                _tokens.Add(new Token(type, current));
                current = "";
            }
        }

        /// <summary>
        /// Возвращает все токены, полученные в результате разбора выражения.
        /// </summary>
        /// <returns>Коллекция токенов.</returns>
        public IEnumerable<Token> GetTokens()
        {
            return _tokens;
        }

        /// <summary>
        /// Проверяет, является ли символ оператором.
        /// </summary>
        /// <param name="ch">Символ для проверки.</param>
        /// <returns>true, если символ является оператором; иначе false.</returns>
        private bool IsOperator(char ch)
        {
            return ch == '+' || ch == '-' || ch == '*' || ch == '/';
        }

        /// <summary>
        /// Проверяет, является ли символ скобкой.
        /// </summary>
        /// <param name="ch">Символ для проверки.</param>
        /// <returns>true, если символ является скобкой; иначе false.</returns>
        private bool IsBracket(char ch)
        {
            return ch == '(' || ch == ')';
        }
    }
}
