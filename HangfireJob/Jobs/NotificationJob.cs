using Hangfire.Server;
using Hangfire;
using HangfireJob.ApplicationLogic;

namespace HangfireJob.Jobs
{
    public class NotificationJob: INotificationJob
    {
        private readonly ISendMailService mailService;

        public NotificationJob(ISendMailService _mailService)
        {
            mailService = _mailService;
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        [DisableConcurrentExecution(10 * 60)]
        public async Task ExecuteRecurringJob(PerformContext context, CancellationToken token)
        {
            var parentJobId = context.BackgroundJob.Id;
            try
            {
                var pendingMails = await mailService.GetMails();

                pendingMails?.ForEach(pendingMail =>
                {
                    BackgroundJob.Enqueue(() => EnqueueBackgroundJob(parentJobId, pendingMail.Id));
                });
            }
            catch (Exception e)
            {
                
            }
        }

        public async Task EnqueueBackgroundJob(string parentJobId, int pendingMailId)
        {
            try
            {
                    await mailService.ProcessMail(pendingMailId);
            }
            catch (Exception ex)
            {
                
            }
        }
    }

    public interface INotificationJob
    {
        Task ExecuteRecurringJob(PerformContext context, CancellationToken token);
    }
}
