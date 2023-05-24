using System.ComponentModel.DataAnnotations;

namespace Euclid.Data
{
    /// <summary>
    /// Сущность чата в БД.
    /// </summary>
    public class ChatEntity : BaseEntity
    {
        /// <summary>
        /// ID чата.
        /// </summary>
        public long ChatId { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string? FirstName { get; set; } = null!;

        /// <summary>
        /// Фамилия пользователя.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Ник пользователя.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Свойство навигации с сущностью <see cref="ChatExpressionResultEntity"/>.
        /// </summary>
        public virtual ICollection<ChatExpressionResultEntity> ChatExpressionResults { get; set; } = new List<ChatExpressionResultEntity>();
    }
}