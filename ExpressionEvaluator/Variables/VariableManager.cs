using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ExpressionEvaluator.Interfaces;
using ExpressionEvaluator.Functions;
using Expr = ExpressionEvaluator.Expressions;

namespace ExpressionEvaluator.Variables
{
    /// <summary>
    /// Менеджер переменных, управляющий переменными и предоставляющий методы для добавления, обновления и получения переменных.
    /// </summary>
    public class VariableManager : IVariableManager
    {
        private readonly List<Variable> _variables = new List<Variable>();

        /// <summary>
        /// Добавляет или обновляет переменную.
        /// </summary>
        /// <param name="variableDeclaration">Объявление переменной.</param>
        public void SetVariable(string variableDeclaration)
        {
            if (variableDeclaration.Contains("="))
            {
                string[] parts = variableDeclaration.Split('=');

                if (parts.Length == 2)
                {
                    string name = parts[0];
                    string result = parts[1];

                    SetVariable(name, result);
                }
                else
                    throw new FormatException("Invalid variable declaration format.");
            }
            else
                throw new FormatException("Invalid variable declaration format.");
        }

        /// <summary>
        /// Добавляет или обновляет переменную.
        /// </summary>
        /// <param name="name">Имя переменной.</param>
        /// <param name="stringValue">Строковое значение переменной.</param>
        public void SetVariable(string name, string stringValue)
        {
            if (Regex.IsMatch(name, @"^(?=.*[a-zA-Z])[a-zA-Z0-9_]+$")
                        && Regex.IsMatch(stringValue, @"^[a-zA-Z0-9_+\-/*(),.=]+$"))
            {
                IExpressionEvaluator expressionEvaluator = new Expr.ExpressionEvaluator(this, new FunctionManager());
                var variable = _variables.FirstOrDefault(v => v.Name == name);
                if (variable != null)
                {
                    variable.StringValue = stringValue;
                    variable.Value = expressionEvaluator.Evaluate(stringValue);
                }
                else
                {
                    variable = new Variable(name, stringValue);
                    variable.Value = expressionEvaluator.Evaluate(stringValue);
                    _variables.Add(variable);
                }
            }
            else
                throw new FormatException("Invalid variable declaration format.");
        }

        /// <summary>
        /// Получает значение переменной по имени.
        /// </summary>
        /// <param name="name">Имя переменной.</param>
        /// <returns>Значение переменной.</returns>
        /// <exception cref="KeyNotFoundException">Выбрасывается, если переменная с указанным именем не найдена.</exception>
        public double GetVariableValue(string name)
        {
            var variable = _variables.FirstOrDefault(v => v.Name == name);
            if (variable != null)
                return variable.Value;
            else
                throw new KeyNotFoundException($"Variable with name '{name}' not found.");
        }

        /// <summary>
        /// Посмотреть строковое значение переменной по имени.
        /// </summary>
        /// <param name="name">Имя переменной.</param>
        /// <returns>Значение переменной.</returns>
        /// <exception cref="KeyNotFoundException">Выбрасывается, если переменная с указанным именем не найдена.</exception>
        public string ViewVariableStringValue(string name)
        {
            var variable = _variables.FirstOrDefault(v => v.Name == name);
            if (variable != null)
                return variable.StringValue;
            else
                throw new KeyNotFoundException($"Variable with name '{name}' not found.");
        }

        /// <summary>
        /// Проверяет, существует ли переменная с заданным именем.
        /// </summary>
        /// <param name="name">Имя переменной.</param>
        /// <returns>true, если переменная существует; иначе false.</returns>
        public bool HasVariable(string name)
        {
            return _variables.Any(v => v.Name == name);
        }

        /// <summary>
        /// Возвращает все переменные.
        /// </summary>
        /// <returns>Список всех переменных.</returns>
        public IEnumerable<Variable> GetAllVariables()
        {
            return _variables;
        }

        /// <summary>
        /// Удаляет переменную.
        /// </summary>
        /// <param name="name">Имя переменной.</param>
        public void DelVariable(string name)
        {
            var variable = _variables.FirstOrDefault(v => v.Name == name);
            if (variable != null)
                _variables.Remove(variable);
            else
                throw new KeyNotFoundException($"Variable with name '{name}' not found.");
        }
    }
}