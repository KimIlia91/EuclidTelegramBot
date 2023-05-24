using Euclid.Data;
using Euclid.Repository;
using Euclid.Service.Helpers;
using Euclid.Service.Models;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Euclid.Service
{
    /// <summary>
    /// Сервис для работы с телеграмм ботом.
    /// </summary>
    public class TelegramBotService : ITelegramBotService
    {
        private readonly IConfiguration _configuration;
        private readonly IBaseRepository<ChatEntity> _chatRepository;
        private readonly IBaseRepository<ChatExpressionResultEntity> _expressionResultRepository;

        /// <summary>
        /// Конструктор класса TelegramBotService.
        /// </summary>
        /// <param name="configuration">Конфигурация приложения.</param>
        /// <param name="userRepository">Репозиторий пользователей.</param>
        /// <param name="expressionResultRepository">Репозиторий результатов выражений.</param>
        public TelegramBotService(
            IConfiguration configuration,
            IBaseRepository<ChatEntity> userRepository,
            IBaseRepository<ChatExpressionResultEntity> expressionResultRepository)
        {
            _chatRepository = userRepository;
            _expressionResultRepository = expressionResultRepository;
            _configuration = configuration;
        }

        /// <inheritdoc/>
        public TelegramBotClient GetBotClient() => new(_configuration["TelegramBot:Token"]!);

        /// <inheritdoc/>
        public async Task<BotResponse<ChatEntity>> GetChatAsync(Update update)
        {
            if (update.Message is null)
                return new BotResponse<ChatEntity> { IsSuccess = false };

            var chat = update!.Message!.Chat;
            var chatEntity = await FindChatAsync(chat);
            if (chatEntity is not null)
                return new BotResponse<ChatEntity> { IsSuccess = true, Result = chatEntity };

            return new BotResponse<ChatEntity>
            {
                IsSuccess = true,
                Result = await AddChatAsync(chat)
            };
        }

        /// <inheritdoc/>
        public BotResponse<string> GetText(Update update)
        {
            var text = update?.Message?.Text;
            if (text is null)
                return new BotResponse<string> {  IsSuccess = false };

            if (text.Equals("/start"))
                return new BotResponse<string> { Result = MessageConstans.HelloMessage, IsSuccess = true };

            return new BotResponse<string> { Result = text, IsSuccess = true };
        }

        /// <inheritdoc/>
        public async Task AddChatExpressionResultAsync(string expressionResult, ChatEntity chat)
        {
            var result = await _expressionResultRepository.GetEntityOrDefaultAsync(r => r.ExpressionResult.Result == expressionResult);
            if (result is null)
            {
                result = new ChatExpressionResultEntity
                {
                    ChatId = chat.Id,
                    ExpressionResult = new ExpressionResultEntity { Result = expressionResult }
                };
                _expressionResultRepository.Update(result);
                await _expressionResultRepository.SaveAsync();
                return;
            }

            result = new ChatExpressionResultEntity
            {
                ChatId = chat.Id,
                ExpressionResultId = result.ExpressionResultId
            };
            await _expressionResultRepository.AddAsync(result);
            await _expressionResultRepository.SaveAsync();
        }

        /// <inheritdoc/>
        public async Task SendResponseAsync(string expressionResult, long chatId)
        {
            var url = $"{_configuration["TelegramBot:Url"]}/TelegramBot";
            var telegramBotClient = new TelegramBotClient(_configuration["TelegramBot:Token"]!);
            await telegramBotClient.SetWebhookAsync(url);
            await telegramBotClient.SendTextMessageAsync(chatId, expressionResult);
        }

        /// <summary>
        /// Асинхронно находит сущность чата по указанному чату.
        /// </summary>
        /// <param name="chat">Чат для поиска.</param>
        /// <returns>Найденная сущность чата или null, если чат не найден.</returns>
        private async Task<ChatEntity?> FindChatAsync(Chat chat)
        {
            var chatExist = await _chatRepository.GetEntityOrDefaultAsync(u => u.ChatId == chat!.Id);
            if (chatExist is null)
                return null;

            return chatExist;
        }

        /// <summary>
        /// Асинхронно добавляет новый чат в базу данных.
        /// </summary>
        /// <param name="chat">Чат для добавления.</param>
        /// <returns>Добавленная сущность чата.</returns>
        private async Task<ChatEntity> AddChatAsync(Chat chat)
        {
            var newChat = new ChatEntity
            {
                FirstName = chat.FirstName,
                LastName = chat.LastName,
                UserName = chat.Username,
                ChatId = chat.Id,
            };
            await _chatRepository.AddAsync(newChat);
            await _chatRepository.SaveAsync();
            return newChat;
        }
    }
}
