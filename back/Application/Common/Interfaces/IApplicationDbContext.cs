using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext : IDisposable, IAsyncDisposable
    {
        #region Methods
        Task BulkInsertAsync<T>(List<T> entity) where T : class;
        Task BulkUpdateAsync<T>(List<T> entity) where T : class;
        /// <summary>
        ///     Asynchronous transaction saving
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Getting a DbSet Instance
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        ///     Asynchronous transaction execution
        /// </summary>
        /// <param name="actionInTransaction">Action within a transaction</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task TransactionAsync(Func<Task> actionInTransaction, CancellationToken cancellationToken = default);
        Task TransactionAsync(Func<Action, Task> actionInTransaction, CancellationToken cancellationToken);

        /// <summary>
        ///     Asynchronous execution of a transaction with a return value
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="actionInTransaction">Action within a transaction</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TResponse> TransactionAsync<TResponse>(Func<Task<TResponse>> actionInTransaction, CancellationToken cancellationToken = default);

        Task<TResponse> TransactionAsync<TResponse>(Func<Action, Task<TResponse>> actionInTransaction, CancellationToken cancellationToken = default);

        bool HasActiveTransaction();
        void RollbackTransaction();

        EntityEntry<T> Attach<T>(T entity) where T : class;
        EntityEntry<T> Entry<T>(T entity) where T : class;

        Task<int> ExecuteSqlRawAsync(string query);
        (int? previousTimeout, int? currentTimeout) SetCommandTimeout(int? timeout);
        #endregion

        #region Entities
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<Execution> Executions { get; set; }
        public DbSet<ExecutionHistory> ExecutionHistories { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        #endregion
    }
}
