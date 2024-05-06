namespace HangfireJob.ApplicationLogic
{
    public class SendMailService: ISendMailService
    {
        public async Task<List<string>> GetMails()
        {
            return new List<string>();
        }

        public async Task ProcessMail(int pendingMailId)
        {

        }
    }

    public interface ISendMailService
    {
        Task<List<string>> GetMails();
        Task ProcessMail(int pendingMailId);
    }
}
