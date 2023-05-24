using System.ComponentModel.DataAnnotations.Schema;

namespace Euclid.Data
{
    /// <summary>
    /// Сущность, представляющая связь между сущностями чата и результатами вычисления выражения.
    /// </summary>
    public class ChatExpressionResultEntity : BaseEntity
    {
        /// <summary>
        /// Идентификатор чата.
        /// </summary>
        [ForeignKey("ChatEntity")]
        public Guid ChatId { get; set; }

        /// <summary>
        /// Сущность чата.
        /// </summary>
        public ChatEntity Chat { get; set; } = null!;

        /// <summary>
        /// Идентификатор результата вычисления выражения.
        /// </summary>
        [ForeignKey("ExpressionResultEntity")]
        public Guid ExpressionResultId { get; set; }

        /// <summary>
        /// Сущность результата вычисления выражения.
        /// </summary>
        public ExpressionResultEntity ExpressionResult { get; set; } = null!;
    }
}
