
namespace Euclid.Data
{
    /// <summary>
    /// Сущность, представляющая результат вычисления выражения.
    /// </summary>
    public class ExpressionResultEntity : BaseEntity
    {   
        /// <summary>
        /// Результат вычисления выражения.
        /// </summary>
        public string Result { get; set; } = null!;

        /// <summary>
        /// Связи сущности результатов вычисления с сущностями чатов.
        /// </summary>
        public virtual ICollection<ChatExpressionResultEntity> ChatExpressionResults { get; set; } = new List<ChatExpressionResultEntity>();
    }
}
