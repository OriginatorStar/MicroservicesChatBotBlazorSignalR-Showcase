using Chat.BlazorChat.Data.Model;

namespace Chat.BlazorChat.Services
{
    public interface IRoomService
    {
        Task<ChatRoom> CreateRoomAsync(string name);
        Task<ChatRoom> GetRoomAsync(Guid id);
        Task<IEnumerable<ChatRoom>> GetRoomsAsync();
    }
}