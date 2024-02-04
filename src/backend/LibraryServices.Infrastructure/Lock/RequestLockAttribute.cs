using System.ComponentModel.DataAnnotations;

namespace LibraryServices.Infrastructure.Lock;

public class RequestLockAttribute : ValidationAttribute
{
    public uint Duration { get; set; }
    public string Message { get; set; }

    public RequestLockAttribute(uint duration, string message)
    {
        Duration = duration;
        Message = message;
    }
}