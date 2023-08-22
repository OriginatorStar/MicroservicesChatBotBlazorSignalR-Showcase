using Chat.BlazorChat.Data.Model;
using Chat.BlazorChat.Services;
using Chat.Shared.BrokerMessages;
using Chat.Shared.Helpers;
using Chat.Shared.SignalR;
using MassTransit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Chat.BlazorChat.Pages
{
    public partial class Room : IAsyncDisposable
    {
        private string roomName;
        private string? message;
        public string? Message { get => message?.Trim(); set => message = value; }
        private FixedSizedConcurrentQueue<Message> messages;
        [Parameter]
        public Guid RoomId { get; set; }
        [Inject]
        IRoomService roomService { get; set; }
        [Inject]
        AuthenticationStateProvider authenticationStateProvider { get; set; }
        [Inject]
        IRequestClient<BotCommandAndResponse> BrokerCommander { get; set; }
        [Inject]
        ChatService chatService { get; set; }

        public ChatClient chatClient { get; set; }

        public User user { get; set; }

        protected override async Task OnInitializedAsync()
        {
            chatClient = new ChatClient(RoomId, "https://localhost:7135");
            // add an event handler for incoming messages
            chatClient.MessageReceived += MessageReceived;
            await chatClient.StartAsync();

            roomName = (await roomService.GetRoomAsync(RoomId)).Name;

            var authenticationState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var userIdClaim = authenticationState.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                user = new User { Id = userIdClaim.Value, UserName = authenticationState.User?.Identity?.Name };
            }

            messages = new FixedSizedConcurrentQueue<Message>(await chatService.GetLast50MessagesAsync(RoomId), 50);
        }

        void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var newMsg = new Message
            {
                ChatRoomId = RoomId,
                Username = e.Username,
                Content = e.Message 
            };
            messages.Enqueue(newMsg);

            InvokeAsync(StateHasChanged);
        }

        async Task SendMessage()
        {
            if (!string.IsNullOrEmpty(Message))
            {
                var sendingMessage = Message;
                Message = string.Empty;

                var newMessage = new Message { Content = sendingMessage, UserId = user.Id, Username = user.UserName, ChatRoomId = RoomId };

                if (sendingMessage.StartsWith("/stock"))
                {
                    await BrokerCommander.GetResponse<BotCommandAndResponse>(new { Command = sendingMessage })
                                                    .ContinueWith(task => chatClient.BotReceive(task.Result.Message.Response));
                    messages.Enqueue(newMessage);
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    if (chatClient.HubConnection is not null)
                    {
                        await chatClient.SendAsync(sendingMessage, user.UserName);
                        await chatService.SendMessageAsync(newMessage);
                        messages.Enqueue(newMessage);
                    }
                }
            }
        }

        async Task CheckForEnterKey(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await SendMessage();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (chatClient.HubConnection is not null)
            {
                await chatClient.StopAsync();
            }
        }
    }
}
