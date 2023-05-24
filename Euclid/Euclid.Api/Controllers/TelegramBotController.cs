using Euclid.Service;
using Euclid.Service.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Euclid.Web.Controlles
{
    /// <summary>
    /// Контроллер для работы с Telegram-ботом.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class TelegramBotController : ControllerBase
    {
        private readonly ITelegramBotService _telegramBotService;
        private readonly ICalculatorService _calculatorService;
        private readonly ILogger<TelegramBotController> _logger;

        /// <summary>
        /// Конструктор класса TelegramBotController.
        /// </summary>
        /// <param name="telegramBotService">Сервис Telegram-бота.</param>
        /// <param name="calculatorService">Сервис калькулятора.</param>
        /// <param name="logger">Логгер.</param>
        public TelegramBotController(
            ITelegramBotService telegramBotService,
            ICalculatorService calculatorService,
            ILogger<TelegramBotController> logger)
        {
            _telegramBotService = telegramBotService;
            _calculatorService = calculatorService;
            _logger = logger;
        }

        /// <summary>
        /// Метод для обработки запросов типа POST для чата с ботом.
        /// </summary>
        /// <param name="update">Объект, содержащий информацию о сообщении.</param>
        [HttpPost]
        public async Task ChatAsync([FromBody] object update)
        {
            if (update is null) return;
            var updateDes = JsonConvert.DeserializeObject<Update>(update.ToString()!);
            var botResponseChat = await _telegramBotService.GetChatAsync(updateDes!);
            if (!botResponseChat.IsSuccess) return;
            var botResponseText = _telegramBotService.GetText(updateDes!);
            try
            {
                if (!botResponseText.IsSuccess) return;
                else if (botResponseText.Result!.Equals(MessageConstans.HelloMessage))
                {
                    await _telegramBotService.AddChatExpressionResultAsync(botResponseText.Result!, botResponseChat.Result!);
                    await _telegramBotService.SendResponseAsync(botResponseText.Result!, botResponseChat.Result!.ChatId);
                    return;
                }

                var expressionResult = _calculatorService.Calculation(botResponseText.Result!);
                await _telegramBotService.AddChatExpressionResultAsync(expressionResult, botResponseChat.Result!);
                await _telegramBotService.SendResponseAsync(expressionResult, botResponseChat.Result!.ChatId);
            }
            catch (Exception ex)
            {
                await _telegramBotService.AddChatExpressionResultAsync(ex.Message, botResponseChat.Result!);
                await _telegramBotService.SendResponseAsync(ex.Message, botResponseChat.Result!.ChatId);
            }
        }

        /// <summary>
        /// Метод для обработки запросов типа GET.
        /// </summary>
        /// <returns>Результат выполнения запроса.</returns>
        /// <response code="200">Статус 200 со список информации о боте.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var bot = _telegramBotService.GetBotClient();
            var botName = await bot.GetMyNameAsync();
            var botDescription = await bot.GetMyShortDescriptionAsync();
            var botWebHookInfo = await bot.GetWebhookInfoAsync();
            var info = new
            {
                botName.Name,
                botDescription.ShortDescription,
                botWebHookInfo.IpAddress,
                botWebHookInfo.Url,
                botWebHookInfo.HasCustomCertificate,    
                botWebHookInfo.LastSynchronizationErrorDate,
                botWebHookInfo.LastErrorDate
            };
            return Ok(info);
        }
    }
}
