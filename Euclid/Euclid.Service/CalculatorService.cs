using Euclid.Data;
using Euclid.Repository;
using Euclid.Service.Helpers;
using System.Globalization;

namespace Euclid.Service
{
    /// <summary>
    /// Сервис калькулятора, реализующий интерфейс ICalculatorService.
    /// </summary>
    public class CalculatorService : ICalculatorService
    {
        private readonly Stack<string> _numbersStack;
        private readonly Stack<char> _operatorsStack;
        private readonly Dictionary<char, uint> _operationPriority = new()
        {
            {'(', 0},
            {')', 0},
            {'+', 1},
            {'-', 1},
            {'*', 2},
            {'/', 2},
            {'^', 3},
        };

        /// <summary>
        /// Конструктор класса CalculatorService.
        /// </summary>
        public CalculatorService()
        {
            _numbersStack = new Stack<string>();
            _operatorsStack = new Stack<char>();
        }

        /// <inheritdoc/>
        public string Calculation(string expression)
        {
            _numbersStack.Clear();
            _operatorsStack.Clear();
            if (string.IsNullOrEmpty(expression))
                return expression;
            if (!HandleValidInputCheck(expression))
                throw CalculatorException.ThrowWrongFormatException;

            return FillStacksForCalculation(expression.Replace(" ", ""));
        }

        /// <summary>
        /// Проверяет корректность ввода, чтобы в выражении не содержались буквы.
        /// </summary>
        /// <param name="expression">Выражение для проверки.</param>
        /// <returns>True, если ввод корректный; в противном случае - false.</returns>
        private static bool HandleValidInputCheck(string expression)
        {
            for (int i = 0; i < expression.Length; i++)
                if (char.IsLetter(expression[i]))
                    return false;

            return true;
        }

        /// <summary>
        /// Заполняет стеки для вычислений на основе выражения.
        /// </summary>
        /// <param name="expr">Выражение, для которого заполняются стеки.</param>
        /// <returns>Результат вычислений из стеков.</returns>
        private string FillStacksForCalculation(string expr)
        {
            for (int i = 0; i < expr.Length; i++)
            {
                if (char.IsDigit(expr[i]) || (i == 0 && expr[i] == '-') ||
                    (expr[i] == '-' && _operationPriority.ContainsKey(expr[i - 1]) && expr[i - 1] != ')'))
                {
                    _numbersStack.Push(GetStringNumber(expr, ref i) + " ");
                }
                else if (_operationPriority.ContainsKey(expr[i]))
                {
                    if (_operatorsStack.Count == 0)
                    {
                        _operatorsStack.Push(expr[i]);
                    }
                    else if (_operationPriority[_operatorsStack.Peek()] == _operationPriority[expr[i]] && expr[i] != '(')
                    {
                        _numbersStack.Push(ParseToCalculate().ToString());
                        _operatorsStack.Push(expr[i]);
                    }
                    else if (_operationPriority[_operatorsStack.Peek()] < _operationPriority[expr[i]] || expr[i] == '(')
                    {
                        _operatorsStack.Push(expr[i]);
                    }
                    else if (_operationPriority[_operatorsStack.Peek()] > _operationPriority[expr[i]])
                    {
                        _numbersStack.Push(ParseToCalculate().ToString());
                        if (expr[i] == ')')
                        {
                            if (_operatorsStack.Peek() == '(')
                            {
                                _operatorsStack.Pop();
                                continue;
                            }
                            i--;
                            continue;
                        }
                        else if (_operatorsStack.Count > 0 &&
                            _operationPriority[_operatorsStack.Peek()] == _operationPriority[expr[i]] && expr[i] != '(')
                        {
                            _numbersStack.Push(ParseToCalculate().ToString());
                        }

                        _operatorsStack.Push(expr[i]);
                    }
                }
            }
            return StackCalculation();
        }

        /// <summary>
        /// Получает числовую строку из выражения, начиная с указанной позиции.
        /// </summary>
        /// <param name="expr">Выражение, из которого извлекается числовая строка.</param>
        /// <param name="pos">Текущая позиция в выражении.</param>
        /// <returns>Числовая строка, извлеченная из выражения.</returns>
        private string GetStringNumber(string expr, ref int pos)
        {
            string strNumber = string.Empty;
            for (; pos < expr.Length; pos++)
            {
                char num = expr[pos];
                if (Char.IsDigit(num) || num == '.' || num == ',' || num == '-'
                    && (pos == 0 || (pos > 1 && _operationPriority.ContainsKey(expr[pos - 1]))))
                {
                    strNumber = string.Concat(strNumber, num);
                    continue;
                }

                pos--;
                break;
            }

            return strNumber;
        }

        /// <summary>
        /// Выполняет вычисление стеков операторов и чисел.
        /// </summary>
        /// <returns>Результат вычисления.</returns>
        private string StackCalculation()
        {
            while (_operatorsStack.Count > 0)
                _numbersStack.Push(ParseToCalculate().ToString());

            if (_numbersStack.Count > 1)
                throw CalculatorException.ThrowWrongFormatException;

            return _numbersStack.Pop();
        }

        /// <summary>
        /// Выполняет разбор операторов и чисел и выполняет соответствующие математические вычисления.
        /// </summary>
        /// <returns>Результат математического вычисления.</returns>
        private double ParseToCalculate()
        {
            if (_operatorsStack.Count == 0 || _numbersStack.Count <= 1)
                throw CalculatorException.ThrowWrongFormatException;

            if (!double.TryParse(_numbersStack.Pop().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double rightNumber))
                throw CalculatorException.ThrowWrongFormatException;

            if (!double.TryParse(_numbersStack.Pop().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double leftNumber))
                throw CalculatorException.ThrowWrongFormatException;

            var operation = _operatorsStack.Pop();
            if (operation == '/' && rightNumber == 0)
                throw CalculatorException.ThrowDivideByZeroException;

            return Math.Round(MathCalculation(operation, leftNumber, rightNumber), 2);
        }

        /// <summary>
        /// Выполняет математическое вычисление на основе оператора и двух операндов.
        /// </summary>
        /// <param name="op">Оператор.</param>
        /// <param name="first">Первый операнд.</param>
        /// <param name="second">Второй операнд.</param>
        /// <returns>Результат математического вычисления.</returns>
        private static double MathCalculation(char op, double first, double second)
        {
            if (op == '+')
                return first + second;
            else if (op == '-')
                return first - second;
            else if (op == '*')
                return first * second;
            else if (op == '/')
                return first / second;
            else if (op == '^')
            {
                if (first < 0)
                    return - Math.Pow(first, second);
                return Math.Pow(first, second);
            }

            return 0;
        }
    }
}