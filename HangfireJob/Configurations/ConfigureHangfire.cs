using Core.Configurations;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Hangfire.Storage;
using HangfireJob.Jobs;
using HangfireJob.Utils;
using StackExchange.Redis;

namespace HangfireJob.Configurations
{
    public static class ConfigureHangfire
    {
        public static void AddHangfireServices(this IServiceCollection services, RedisSettings settings)
        {
            var hangfireConnection = ConnectionMultiplexer.Connect($"{settings.Host}:{settings.Port}");

            services.AddHangfire(configuration =>
            {
                configuration.UseRedisStorage(hangfireConnection, new RedisStorageOptions { Prefix = "hangfire:notification:", UseTransactions = true })
                             .WithJobExpirationTimeout(TimeSpan.FromHours(1));
            });

            services.AddHangfireServer(conf =>
            {
                conf.CancellationCheckInterval = TimeSpan.FromMinutes(2);
            });
        }

        public static void UseHangfire(this IApplicationBuilder app, HangfireSettings setting)
        {
            app.UseHangfireDashboard("/jobs", new DashboardOptions
            {
                Authorization = new[] { new HanfireDashBoardAuth() },
                IgnoreAntiforgeryToken = true
            });

            //Notification Job
            RemoveRecurringJobs();
            RecurringJob.AddOrUpdate<INotificationJob>(
                "Notification",
                a => a.ExecuteRecurringJob(null, CancellationToken.None),
                setting.Cron);
        }

        private static int RemoveRecurringJobs()
        {
            var totalJobs = 0;
            using (var connection = JobStorage.Current.GetConnection())
            {
                totalJobs = connection.GetRecurringJobs().Count;
                foreach (var recurringJob in connection.GetRecurringJobs())
                {
                    RecurringJob.RemoveIfExists(recurringJob.Id);
                }
            }

            return totalJobs;
        }
    }
}
