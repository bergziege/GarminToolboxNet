namespace GarminConnectExporter.Config
{
    public class MailConfiguration
    {
        public string SmtpServer { get; set; }
        public int SmtpServerPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
    }
}