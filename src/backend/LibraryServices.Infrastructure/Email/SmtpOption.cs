namespace LibraryServices.Infrastructure.Email
{
    public class SmtpOption
    {
        public string? Server { get; set; }

        public int Port { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? Sender { get; set; }

        public string? SenderAddress { get; set; }

        public bool UseSSL { get; set; }
    }
}
