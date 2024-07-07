using ExpressionEvaluator.Functions;
using System.Collections.Generic;

namespace ExpressionEvaluator.Interfaces
{
    /// <summary>
    /// Интерфейс для менеджера функций, управляющего пользовательскими функциями.
    /// </summary>
    public interface IFunctionManager
    {
        /// <summary>
        /// Добавляет новую функцию на основе строки объявления.
        /// </summary>
        /// <param name="declaration">Строка объявления функции.</param>
        void AddFunction(string declaration);

        /// <summary>
        /// Добавляет новую функцию с указанными параметрами.
        /// </summary>
        /// <param name="name">Имя функции.</param>
        /// <param name="parameters">Список параметров функции.</param>
        /// <param name="expression">Выражение функции.</param>
        void AddFunction(string name, List<string> parameters, string expression);

        /// <summary>
        /// Извлекает имя функции и параметры из строки объявления функции.
        /// </summary>
        /// <param name="declaration">Строка объявления функции.</param>
        /// <returns>Кортеж с именем функции и списком параметров.</returns>
        /// <exception cref="FormatException">Вызывается, если формат строки объявления функции неверный.</exception>
        (string name, List<string> parameters) ParseFunctionDeclaration(string declaration);

        /// <summary>
        /// Получает функцию по имени и количеству параметров.
        /// </summary>
        /// <param name="name">Имя функции.</param>
        /// <param name="parameterCount">Количество параметров функции.</param>
        /// <returns>Функция с указанным именем и количеством параметров.</returns>
        /// <exception cref="KeyNotFoundException">Вызывается, если функция с указанным именем и количеством параметров не найдена.</exception>
        Function GetFunction(string name, int parameterCount);

        /// <summary>
        /// Проверяет наличие функции с указанным именем и количеством параметров.
        /// </summary>
        /// <param name="name">Имя функции.</param>
        /// <param name="parameterCount">Количество параметров функции.</param>
        /// <returns>true, если функция с указанным именем и количеством параметров существует; в противном случае false.</returns>
        bool HasFunction(string name, int parameterCount);

        /// <summary>
        /// Получает все функции.
        /// </summary>
        /// <returns>Список всех функций.</returns>
        List<Function> GetAllFunctions();

        /// <summary>
        /// Удаляет функцию.
        /// </summary>
        /// <param name="name">Имя функции.</param>
        /// <param name="parameterCount">Количество параметров функции.</param>
        void DelFunction(string name, int parameterCount);
    }
}