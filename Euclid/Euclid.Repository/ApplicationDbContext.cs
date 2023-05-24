using Euclid.Data;
using Microsoft.EntityFrameworkCore;

namespace Euclid.Repository
{
    /// <summary>
    /// Контекст базы данных приложения.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Коллекция сущностей "Чаты".
        /// </summary>
        public DbSet<ChatEntity> Chats { get; set; }

        /// <summary>
        /// Коллекция сущностей "Результаты выражений".
        /// </summary>
        public DbSet<ExpressionResultEntity> ExpressionResults { get; set; }

        /// <summary>
        /// Коллекция сущностей "Связь Чат-Результат выражения".
        /// </summary>
        public DbSet<ChatExpressionResultEntity> ChatExpressionResults { get; set; }

        /// <summary>
        /// Конструктор контекста базы данных приложения.
        /// </summary>
        /// <param name="options">Параметры контекста базы данных.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}