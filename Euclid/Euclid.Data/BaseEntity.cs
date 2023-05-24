namespace Euclid.Data
{
    /// <summary>
    /// Базовый класс для сущностей.
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// Идентификатор сущности.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Дата создания сущности.
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
