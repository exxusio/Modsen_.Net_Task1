using ExpressionEvaluator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ExpressionEvaluator.Functions
{
    /// <summary>
    /// Менеджер функций, управляющий пользовательскими функциями.
    /// </summary>
    public class FunctionManager : IFunctionManager
    {
        /// <summary>
        /// Список для хранения функций.
        /// </summary>
        private readonly List<Function> _functions = new List<Function>();

        /// <summary>
        /// Добавляет новую функцию на основе строки объявления.
        /// </summary>
        /// <param name="declaration">Строка объявления функции.</param>
        /// <exception cref="FormatException">Вызывается, если формат строки объявления функции неверный.</exception>
        public void AddFunction(string declaration)
        {
            if (declaration.Contains('='))
            {
                string[] parts = declaration.Split('=');
                if (parts.Length == 2)
                {
                    string expression = parts[1].Trim();

                    var (name, parameters) = ParseFunctionDeclaration(parts[0].Trim());
                    AddFunction(name, parameters, expression);
                }
                else
                    throw new FormatException("Incorrect function declaration format.");
            }
            else
                throw new FormatException("Incorrect function declaration format.");
        }

        /// <summary>
        /// Добавляет новую функцию.
        /// </summary>
        /// <param name="name">Имя функции.</param>
        /// <param name="parameters">Список параметров функции.</param>
        /// <param name="expression">Выражение функции.</param>
        /// <exception cref="FormatException">Вызывается, если формат имени или выражения функции неверный.</exception>
        public void AddFunction(string name, List<string> parameters, string expression)
        {
            if (Regex.IsMatch(name, @"^(?=.*[a-zA-Z])[a-zA-Z0-9_]+$")
                && Regex.IsMatch(expression, @"^[a-zA-Z0-9_+\-/*(),.=]+$"))
            {
                _functions.RemoveAll(f => f.Name == name && f.Parameters.Count == parameters.Count);

                _functions.Add(new Function(name, parameters, expression));
            }
            else
                throw new FormatException("Incorrect function name or expression format.");
        }

        /// <summary>
        /// Извлекает имя функции и параметры из строки объявления функции.
        /// </summary>
        /// <param name="declaration">Строка объявления функции.</param>
        /// <returns>Кортеж с именем функции и списком параметров.</returns>
        /// <exception cref="FormatException">Вызывается, если формат строки объявления функции неверный.</exception>
        public (string name, List<string> parameters) ParseFunctionDeclaration(string declaration)
        {
            int paramStart = declaration.IndexOf('(');
            int paramEnd = declaration.LastIndexOf(')');

            if (paramStart > 0 && paramEnd > paramStart)
            {
                string name = declaration.Substring(0, paramStart).Trim();
                string paramList = declaration.Substring(paramStart + 1, paramEnd - paramStart - 1);

                List<string> parameters = ParseParameters(paramList);

                return (name, parameters);
            }
            else
            {
                throw new FormatException("Incorrect function declaration format.");
            }
        }

        /// <summary>
        /// Разбирает строку параметров функции на отдельные параметры.
        /// </summary>
        /// <param name="paramList">Строка, содержащая параметры функции.</param>
        /// <returns>Список параметров функции.</returns>
        private List<string> ParseParameters(string paramList)
        {
            List<string> parameters = new List<string>();
            StringBuilder currentParam = new StringBuilder();
            int bracketDepth = 0;

            foreach (char ch in paramList)
            {
                if (ch == ',' && bracketDepth == 0)
                {
                    parameters.Add(currentParam.ToString().Trim());
                    currentParam.Clear();
                }
                else
                {
                    if (ch == '(')
                        bracketDepth++;
                    else if (ch == ')')
                        bracketDepth--;
                    
                    currentParam.Append(ch);
                }
            }

            if (currentParam.Length > 0)
                parameters.Add(currentParam.ToString().Trim());
            
            return parameters;
        }

        /// <summary>
        /// Получает функцию по имени и количеству параметров.
        /// </summary>
        /// <param name="name">Имя функции.</param>
        /// <param name="parameterCount">Количество параметров функции.</param>
        /// <returns>Функция с указанным именем и количеством параметров.</returns>
        /// <exception cref="KeyNotFoundException">Вызывается, если функция с указанным именем и количеством параметров не найдена.</exception>
        public Function GetFunction(string name, int parameterCount)
        {
            var function = _functions.FirstOrDefault(f => f.Name == name && f.Parameters.Count == parameterCount);
            if (function != null)
                return function;
            throw new KeyNotFoundException($"Function with the name '{name}' and {parameterCount} parameters not found.");
        }

        /// <summary>
        /// Проверяет наличие функции с указанным именем и количеством параметров.
        /// </summary>
        /// <param name="name">Имя функции.</param>
        /// <param name="parameterCount">Количество параметров функции.</param>
        /// <returns>true, если функция с указанным именем и количеством параметров существует; в противном случае false.</returns>
        public bool HasFunction(string name, int parameterCount)
        {
            return _functions.Any(f => f.Name == name && f.Parameters.Count == parameterCount);
        }

        /// <summary>
        /// Получает все функции.
        /// </summary>
        /// <returns>Список всех функций.</returns>
        public List<Function> GetAllFunctions()
        {
            return new List<Function>(_functions);
        }

        /// <summary>
        /// Удаляет функцию.
        /// </summary>
        /// <param name="name">Имя функции.</param>
        /// <param name="parameterCount">Количество параметров функции.</param>
        public void DelFunction(string name, int parameterCount)
        {
            var function = _functions.FirstOrDefault(f => f.Name == name && f.Parameters.Count == parameterCount);
            if (function != null)
                _functions.Remove(function);
            else
                throw new KeyNotFoundException($"Function with the name '{name}' and {parameterCount} parameters not found.");
        }
    }
}