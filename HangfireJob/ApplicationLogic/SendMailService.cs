using HangfireJob.Model;

namespace HangfireJob.ApplicationLogic
{
    public class SendMailService: ISendMailService
    {
        public async Task<List<PendingMail>> GetMails()
        {
            return new List<PendingMail>();
        }

        public async Task ProcessMail(int pendingMailId)
        {

        }
    }

    public interface ISendMailService
    {
        Task<List<PendingMail>> GetMails();
        Task ProcessMail(int pendingMailId);
    }
}
