using Chat.BlazorChat.Data;
using Chat.BlazorChat.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Chat.BlazorChat.Services
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationDbContext _context;

        public RoomService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ChatRoom> CreateRoomAsync(string name)
        {
            var room = new ChatRoom { Name = name };
            _context.ChatRooms.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<IEnumerable<ChatRoom>> GetRoomsAsync()
        {
            return await _context.ChatRooms.ToListAsync();
        }

        public async Task<ChatRoom> GetRoomAsync(Guid id)
        {
            return await _context.ChatRooms.FindAsync(id);
        }
    }
}
