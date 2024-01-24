using Application.Common.Interfaces;
using Domain.Entities;
using EntityFrameworkExtensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using EFCore.BulkExtensions;

namespace Infrastructure
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        #region Methods
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public async Task TransactionAsync(Func<Task> actionInTransaction, CancellationToken cancellationToken = default)
        {
            await this.ExecuteTransactionAsync(actionInTransaction, cancellationToken);
        }

        public async Task TransactionAsync(Func<Action, Task> actionInTransaction, CancellationToken cancellationToken = default)
        {
            await this.ExecuteTransactionAsync(actionInTransaction, cancellationToken);
        }

        public async Task<TResponse> TransactionAsync<TResponse>(Func<Task<TResponse>> actionInTransaction, CancellationToken cancellationToken = default)
        {
            return await this.ExecuteTransactionAsync(actionInTransaction, cancellationToken);
        }

        public async Task<TResponse> TransactionAsync<TResponse>(Func<Action, Task<TResponse>> actionInTransaction, CancellationToken cancellationToken = default)
        {
            return await this.ExecuteTransactionAsync(actionInTransaction, cancellationToken);
        }

        public bool HasActiveTransaction()
        {
            return this.Database.CurrentTransaction != null;
        }

        public void RollbackTransaction()
        {
            if (!HasActiveTransaction()) return;
            this.Database.CurrentTransaction!.Rollback();
        }

        public async Task BulkInsertAsync<T>(List<T> entities) where T : class
        {
            await DbContextBulkExtensions.BulkInsertAsync(this, entities, new BulkConfig() { SetOutputIdentity = true });
        }

        public async Task BulkUpdateAsync<T>(List<T> entities) where T : class
        {
            await DbContextBulkExtensions.BulkUpdateAsync(this, entities, new BulkConfig() { SetOutputIdentity = true });
        }

        public async Task<int> ExecuteSqlRawAsync(string query)
        {
            return await this.Database.ExecuteSqlRawAsync(query);
        }

        public (int? previousTimeout, int? currentTimeout) SetCommandTimeout(int? timeout)
        {
            var prevTimeout = this.Database.GetCommandTimeout();
            this.Database.SetCommandTimeout(timeout);
            return (prevTimeout, timeout);
        }
        #endregion

        #region Entities
        public DbSet<Job> Jobs { get; set; } = null!;
        public DbSet<JobType> JobTypes { get; set; } = null!;
        public DbSet<Execution> Executions { get; set; } = null!;
        public DbSet<ExecutionHistory> ExecutionHistories { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        #endregion
    }
}
