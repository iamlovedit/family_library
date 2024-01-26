namespace LibraryServices.Infrastructure.Email
{
    public class EmailMessage
    {
        public string? Receiver { get; set; }

        public string? RecevieAddress { get; set; }

        public string? Subject { get; set; }

        public string? Content { get; set; }

        public EmailMessage(string recevier, string recevieAddress, string subject, string content)
        {
            Receiver = recevier;
            RecevieAddress = recevieAddress;
            Subject = subject;
            Content = content;
        }
    }
}
