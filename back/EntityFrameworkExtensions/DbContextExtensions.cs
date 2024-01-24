using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkExtensions
{
    public static class DbContextExtensions
    {
        public static async Task ExecuteTransactionAsync(this DbContext context, Func<Task> actionInTransaction, CancellationToken cancellationToken = default)
        {
            await using var dbContextTransaction = await context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await actionInTransaction();
                await dbContextTransaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await dbContextTransaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public static async Task ExecuteTransactionAsync(this DbContext context, Func<Action, Task> actionInTransaction, CancellationToken cancellationToken = default)
        {
            await using var dbContextTransaction = await context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var shouldRollback = false;
                Action rollback = ()=> shouldRollback = true;
                await actionInTransaction(rollback);
                if(shouldRollback)
                    await dbContextTransaction.RollbackAsync(cancellationToken);
                else
                    await dbContextTransaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await dbContextTransaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public static async Task<TResponse> ExecuteTransactionAsync<TResponse>(this DbContext context, Func<Task<TResponse>> actionInTransaction, CancellationToken cancellationToken = default)
        {
            await using var dbContextTransaction = await context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var result = await actionInTransaction();
                await dbContextTransaction.CommitAsync(cancellationToken);

                return result;
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public static async Task<TResponse> ExecuteTransactionAsync<TResponse>(this DbContext context, Func<Action, Task<TResponse>> actionInTransaction, CancellationToken cancellationToken = default)
        {
            await using var dbContextTransaction = await context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var shouldRollback = false;
                Action rollback = () => shouldRollback = true;
                var result = await actionInTransaction(rollback);
                if (shouldRollback)
                    await dbContextTransaction.RollbackAsync(cancellationToken);
                else
                    await dbContextTransaction.CommitAsync(cancellationToken);

                return result;
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
