namespace Euclid.Service.Helpers
{
    /// <summary>
    /// Класс исключения, связанного с калькулятором.
    /// </summary>
    public class CalculatorException
    {
        /// <summary>
        /// Возвращает исключение, указывающее на неверный формат выражения.
        /// </summary>
        public static Exception ThrowWrongFormatException => new("Wrong expression format.");

        /// <summary>
        /// Возвращает исключение, указывающее на неверный ввод.
        /// </summary>
        public static Exception ThrowWrongInputException => new("Exception! Wrong input.");

        /// <summary>
        /// Возвращает исключение, указывающее на деление на ноль.
        /// </summary>
        public static Exception ThrowDivideByZeroException => new("Exception! Divide by zero.");
    }
}
