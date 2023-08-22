using System.ComponentModel.DataAnnotations;

namespace Chat.Shared.BrokerMessages
{
    public interface IBotCommandAndResponse
    {
        [Required]
        string Command { get; set; }
        string? Response { get; set; }
        DateTime Timestamp { get; set; }
    }
}