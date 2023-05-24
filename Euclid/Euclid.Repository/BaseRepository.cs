using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Euclid.Repository
{
    /// <summary>
    /// Базовый репозиторий для работы с сущностями.
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _db;
        private readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// Конструктор базового репозитория.
        /// </summary>
        /// <param name="db">Контекст базы данных.</param>
        public BaseRepository(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();
        }

        /// <inheritdoc/>
        public async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);

        /// <inheritdoc/>
        public async Task<TEntity?> GetEntityOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null, string? includeProperty = null)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            if (includeProperty != null)
                foreach (var includeProp in includeProperty.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProp);

            if (filter != null)
                query = query.Where(filter);

            return await query.FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task SaveAsync()
        {
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка при сохранение данных.", ex);
            }
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }
    }
}
