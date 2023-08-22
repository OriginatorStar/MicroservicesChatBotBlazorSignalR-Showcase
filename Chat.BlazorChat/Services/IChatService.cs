using Chat.BlazorChat.Data.Model;
using Chat.Shared.BrokerMessages;

namespace Chat.BlazorChat.Services
{
    public interface IChatService
    {
        Task<List<Message>> GetLast50MessagesAsync(Guid roomId);
        Task<Message> GetMessageAsync(Guid messageId);
        Task SendMessageAsync(Message message);
    }
}