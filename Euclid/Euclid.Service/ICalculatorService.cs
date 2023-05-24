
namespace Euclid.Service
{
    /// <summary>
    /// Интерфейс сервиса калькулятора.
    /// </summary>
    public interface ICalculatorService
    {
        /// <summary>
        /// Выполняет вычисление выражения.
        /// </summary>
        /// <param name="expression">Выражение для вычисления.</param>
        /// <returns>Результат вычисления.</returns>
        string Calculation(string expression);
    }
}
