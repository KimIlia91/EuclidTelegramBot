using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euclid.Service.Models
{
    /// <summary>
    /// Ответ об операции от Телеграмм бота.
    /// </summary>
    public class BotResponse<TEntity>
    {
        /// <summary>
        /// Прошла операция успешно или нет.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Результат операции.
        /// </summary>
        public TEntity? Result { get; set; }

        /// <summary>
        /// Ошибки возникщие во время опеции.
        /// </summary>
        public string? Errors { get; set; }
    }
}
