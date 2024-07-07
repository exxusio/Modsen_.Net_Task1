using ExpressionEvaluator.Variables;
using System.Collections.Generic;

namespace ExpressionEvaluator.Interfaces
{
    /// <summary>
    /// Интерфейс для менеджера переменных, предоставляющий методы для добавления, обновления и получения переменных.
    /// </summary>
    public interface IVariableManager
    {
        /// <summary>
        /// Добавляет или обновляет переменную.
        /// </summary>
        /// <param name="variableDeclaration">Объявление переменной.</param>
        void SetVariable(string variableDeclaration);

        /// <summary>
        /// Добавляет или обновляет переменную.
        /// </summary>
        /// <param name="name">Имя переменной.</param>
        /// <param name="stringValue">Значение переменной.</param>
        void SetVariable(string name, string stringValue);

        /// <summary>
        /// Получает значение переменной по имени.
        /// </summary>
        /// <param name="name">Имя переменной.</param>
        /// <returns>Значение переменной.</returns>
        double GetVariableValue(string name);

        /// <summary>
        /// Посмотреть строковое значение переменной по имени.
        /// </summary>
        /// <param name="name">Имя переменной.</param>
        /// <returns>Значение переменной.</returns>
        /// <exception cref="KeyNotFoundException">Выбрасывается, если переменная с указанным именем не найдена.</exception>
        string ViewVariableStringValue(string name);

        /// <summary>
        /// Проверяет, существует ли переменная с заданным именем.
        /// </summary>
        /// <param name="name">Имя переменной.</param>
        /// <returns>true, если переменная существует; иначе false.</returns>
        bool HasVariable(string name);

        /// <summary>
        /// Возвращает все переменные.
        /// </summary>
        /// <returns>Список всех переменных.</returns>
        IEnumerable<Variable> GetAllVariables();

        /// <summary>
        /// Удаляет переменную.
        /// </summary>
        /// <param name="name">Имя переменной.</param>
        void DelVariable(string name);
    }
}