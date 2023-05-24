using Euclid.Data;
using Euclid.Service.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Euclid.Service
{
    /// <summary>
    /// Интерфейс для работы с телеграмм ботом.
    /// </summary>
    public interface ITelegramBotService
    {
        /// <summary>
        /// Получает клиента Telegram бота.
        /// </summary>
        /// <returns>Клиент Telegram бота.</returns>
        TelegramBotClient GetBotClient();

        /// <summary>
        /// Асинхронно получает информацию о чате на основе обновления (Update).
        /// </summary>
        /// <param name="update">Обновление (Update).</param>
        /// <returns>Ответ бота с информацией о чате.</returns>
        Task<BotResponse<ChatEntity>> GetChatAsync(Update update);

        /// <summary>
        /// Получает текстовое сообщение из обновления (Update).
        /// </summary>
        /// <param name="update">Обновление (Update).</param>
        /// <returns>Ответ бота с текстовым сообщением.</returns>
        BotResponse<string> GetText(Update update);

        /// <summary>
        /// Асинхронно добавляет результат вычисления выражения в связанный чат.
        /// </summary>
        /// <param name="expressionResult">Результат вычисления выражения.</param>
        /// <param name="chat">Чат, к которому добавляется результат.</param>
        Task AddChatExpressionResultAsync(string expressionResult, ChatEntity chat);

        /// <summary>
        /// Асинхронно отправляет ответное сообщение с результатом вычисления выражения в указанный чат.
        /// </summary>
        /// <param name="expressionResult">Результат вычисления выражения.</param>
        /// <param name="chatId">Идентификатор чата.</param>
        Task SendResponseAsync(string expressionResult, long chatId);
    }
}
