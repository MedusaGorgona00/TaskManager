using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{
    public class DatabaseMigrator
    {
        public static async Task SeedDatabaseAsync(IServiceProvider appServiceProvider)
        {
            await using var scope = appServiceProvider.CreateAsyncScope();
            var serviceProvider = scope.ServiceProvider;
            var logger = serviceProvider.GetRequiredService<ILogger<DatabaseMigrator>>();
            try
            {
                var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Migration error");
            }

            try
            {
                var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
                await SeedMockDataAsync(context);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "DB seed error");
            }
        }

        private static async Task SeedMockDataAsync(ApplicationDbContext context)
        {
            if (!(await context.JobTypes.AnyAsync()) 
                && !(await context.Jobs.AnyAsync()) 
                && !(await context.Notifications.AnyAsync()) 
                && !(await context.Executions.AnyAsync()) 
                && !(await context.ExecutionHistories.AnyAsync()))
            {
                var jobtype1 = new JobType { Name = "Integrating" };
                var jobtype2 = new JobType { Name = "Testing" };
                var jobtype3 = new JobType { Name = "Refactoring" };
                var jobTypes = new List<JobType> { jobtype1, jobtype2, jobtype3 };


                var job1 = new Job { JobType = jobtype3, Description = "The task is to understand the old code in project and refactor it" };
                var job2 = new Job { JobType = jobtype3, Description = "The task is to transfer the entire structure of entities from old ORM system to EntityFramework" };
                var job3 = new Job { JobType = jobtype2, Description = "You need to write unit tests" };
                var job4 = new Job { JobType = jobtype1, Description = "You need to implement a payment service to obtain the transaction status" };
                var jobs = new List<Job> { job1, job2, job3, job4 };

                var execution1 = new Execution()
                {
                    Job = job1,
                    CreatedDate = new DateTime(2024, 01, 01, 10, 10, 10),
                    StartedAt = new DateTime(2024, 01, 22, 10, 10, 10),
                    FinishedAt = new DateTime(2024, 01, 23, 10, 10, 10),
                };
                var execution2 = new Execution()
                {
                    Job = job2,
                    CreatedDate = new DateTime(2024, 01, 01, 10, 10, 10),
                    StartedAt = new DateTime(2024, 01, 05, 10, 10, 10),
                    FinishedAt = new DateTime(2024, 01, 10, 10, 10, 10),
                };
                var execution3 = new Execution()
                {
                    Job = job3,
                    CreatedDate = new DateTime(2024, 01, 10, 10, 50, 11),
                    StartedAt = new DateTime(2024, 01, 22, 10, 10, 10),
                    FinishedAt = null,
                };
                var executions = new List<Execution> { execution1, execution2, execution3 };


                var executionHistory1 = new ExecutionHistory()
                {
                    Message = "Started",
                    Execution = execution1,
                    CreatedDate = new DateTime(2024, 01, 05, 10, 10, 10),
                };
                var executionHistory2 = new ExecutionHistory()
                {
                    Message = "Stopped",
                    Execution = execution1,
                    CreatedDate = new DateTime(2024, 01, 08, 10, 10, 10),
                }; 
                var executionHistory3 = new ExecutionHistory()
                {
                    Message = "Completed",
                    Execution = execution1,
                    CreatedDate = new DateTime(2024, 01, 10, 10, 10, 10),
                };

                var executionHistories = new List<ExecutionHistory> { executionHistory1, executionHistory2, executionHistory3 };

                var notifications = new List<Notification>()
                {
                    new()
                    {
                        Execution = execution2,
                        Text = "Migration Completed with Warnings",
                        CreatedDate = new DateTime(2024, 01, 10, 10, 50, 50)
                    },
                    new()
                    {
                        Execution = execution1,
                        Text = "Project's refactoring completed successfully",
                        CreatedDate = new DateTime(2024, 01, 10, 15, 50, 50)
                    }
                };

                context.JobTypes.AddRange(jobTypes);
                context.Jobs.AddRange(jobs);
                context.Executions.AddRange(executions);
                context.ExecutionHistories.AddRange(executionHistories);
                context.Notifications.AddRange(notifications);

                await context.SaveChangesAsync();
            }
        }
    }
}
