using Chat.Shared.SignalR.Constants;
using Microsoft.AspNetCore.SignalR.Client;

namespace Chat.Shared.SignalR
{
    public class ChatClient : IAsyncDisposable
    {
        public const string HubEndpoint = "/chathub";

        private readonly string _hubUrl;
        public HubConnection HubConnection;

        public ChatClient(Guid room, string siteUrl)
        {
            if (string.IsNullOrWhiteSpace(siteUrl))
                throw new ArgumentNullException(nameof(siteUrl));
            _room = room;
            _hubUrl = siteUrl.TrimEnd('/') + HubEndpoint;
        }

        private readonly Guid _room;

        private bool _started = false;

        public async Task StartAsync()
        {
            if (!_started)
            {
                HubConnection = new HubConnectionBuilder()
                    .WithUrl(_hubUrl)
                    .Build();

                HubConnection.On<string, string>(Events.RECEIVE, (message, username) =>
                {
                    HandleReceiveMessage(message, username);
                });

                await HubConnection.StartAsync();

                await JoinRoomAsync(_room.ToString());

                _started = true;
            }
        }

        public event MessageReceivedEventHandler MessageReceived;

        private void HandleReceiveMessage(string message, string username)
        {
            MessageReceived?.Invoke(this, new MessageReceivedEventArgs(message, username));
        }

        private async Task JoinRoomAsync(string room)
        {
            await HubConnection.SendAsync(Events.JOINROOM, room);
        }

        private async Task LeaveRoomAsync(string room)
        {
            await HubConnection.SendAsync(Events.LEAVEROOM, room);
        }

        public async Task SendAsync(string message, string username)
        {
            if (!_started)
                throw new InvalidOperationException("ChatClient not started");
            await HubConnection.SendAsync(Events.SEND, _room, message, username);
        }

        public async Task BotReceive(string message)
        {
            if (!_started)
                throw new InvalidOperationException("ChatClient not started");
            await HubConnection.SendAsync(Events.BOTRECEIVE, _room, message);
        }

        public async Task StopAsync()
        {
            if (_started)
            {
                await LeaveRoomAsync(_room.ToString());
                await HubConnection.StopAsync();
                await HubConnection.DisposeAsync();
                HubConnection = null;
                _started = false;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await StopAsync();
        }
    }

    public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);

    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(string message, string username)
        {
            Message = message;
            Username = username;
        }
        public string Message { get; set; }
        public string Username { get; set; }
    }
}
