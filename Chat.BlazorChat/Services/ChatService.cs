using Chat.BlazorChat.Data;
using Chat.BlazorChat.Data.Model;
using Chat.Shared.BrokerMessages;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Chat.BlazorChat.Services
{
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _context;
        IRequestClient<BotCommandAndResponse> _brokerCommander;

        public ChatService(ApplicationDbContext context, IRequestClient<BotCommandAndResponse> brokerCommander)
        {
            _context = context;
            _brokerCommander = brokerCommander;
        }

        public async Task<List<Message>> GetLast50MessagesAsync(Guid roomId)
        {
            return await _context.ChatMessages
                                    .Where(m => m.ChatRoom.Id == roomId)
                                    .OrderByDescending(m => m.Timestamp)
                                    .Take(50)
                                    .OrderBy(m => m.Timestamp)
                                    .ToListAsync();
        }

        public async Task<Message> GetMessageAsync(Guid messageId)
        {
            return await _context.ChatMessages
                                    .Include(r => r.ChatRoom)
                                    .FirstAsync(c => c.Id == messageId);
        }

        public async Task SendMessageAsync(Message message)
        {
            var addedMessage = _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}
