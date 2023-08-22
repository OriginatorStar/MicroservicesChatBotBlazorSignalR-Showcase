using Chat.Shared.SignalR.Constants;
using Microsoft.AspNetCore.SignalR;

namespace Chat.SignalRHub
{
    public class ChatHub : Hub
    {
        public async Task JoinRoom(string room)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
        }

        public async Task LeaveRoom(string room)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);
        }

        public async Task SendMessage(string room, string message, string username)
        {
            await Clients.OthersInGroup(room).SendAsync(Events.RECEIVE, message, username);
        }

        public async Task BotReceive(string room, string message)
        {
            await Clients.Group(room).SendAsync(Events.RECEIVE, message, "Bot");
        }
    }
}
