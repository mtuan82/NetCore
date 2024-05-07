namespace HangfireJob.Model
{
    public class PendingMail
    {
        public int Id { get; set; }

        public string? MailType { get; set; }

        public string? EmailAddress { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? FailCouter { get; set; }

        public string? ExtraValues { get; set; }
    }
}
