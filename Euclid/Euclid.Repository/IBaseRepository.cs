using System.Linq.Expressions;

namespace Euclid.Repository
{
    /// <summary>
    /// Интерфейс базового репозитория.
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Получает сущность из контекста данных, удовлетворяющую условиям фильтрации, или возвращает null, если сущность не найдена.
        /// </summary>
        /// <param name="filter">Выражение фильтрации.</param>
        /// <returns>Сущность из контекста данных, удовлетворяющую условиям фильтрации, или null, если сущность не найдена.</returns>
        Task<TEntity?> GetEntityOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null, string? includeProperty = null);

        /// <summary>
        /// Добавляет новую сущность в контекст данных.
        /// </summary>
        /// <param name="entity">Добавляемая сущность.</param>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Обеовляет существующую сущность в контексте базы данных.
        /// </summary>
        /// <param name="entity">Обновляемая сущность.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Сохранить сущность в контексте базы данных.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Исключение если невозможно сохранить данные в контексте базы данных.</exception>
        Task SaveAsync();
    }
}
