namespace LibraryServices.Infrastructure
{
    public class MessageData<T>
    {
        public int StatusCode { get; set; } = 200;

        public bool Succeed { get; set; }

        public string? Message { get; }

        public T? Response { get; }

        public MessageData(bool succeed, string message, int statusCode = 200)
        {
            Succeed = succeed;
            Message = message;
            StatusCode = statusCode;
        }

        public MessageData(bool succeed, string message, T response, int statusCode = 200) : this(succeed, message, statusCode)
        {
            Response = response;
        }
    }
}
