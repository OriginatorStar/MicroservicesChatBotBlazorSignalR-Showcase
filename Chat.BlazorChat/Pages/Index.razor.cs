using Chat.BlazorChat.Data.Model;
using Chat.BlazorChat.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Chat.BlazorChat.Pages
{
    public partial class Index
    {
        private IEnumerable<ChatRoom> rooms;
        private string? newRoomName;
        public string? NewRoomName { get => newRoomName?.Trim(); set => newRoomName = value; }

        [Inject]
        IRoomService roomService { get; set; }
        [Inject]
        NavigationManager navigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            rooms = await roomService.GetRoomsAsync();
        }

        private void OnRoomSelected(ChangeEventArgs e)
        {
            var roomId = e.Value.ToString();
            navigationManager.NavigateTo($"/chatroom/{roomId}");
        }

        private async Task CreateRoomAsync()
        {
            if (!string.IsNullOrEmpty(NewRoomName))
            {
                var newRoom = await roomService.CreateRoomAsync(NewRoomName);
                rooms = await roomService.GetRoomsAsync();
                navigationManager.NavigateTo($"/chatroom/{newRoom.Id}");
            }
        }

        async Task CheckForEnterKey(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await CreateRoomAsync();
            }
        }
    }
}
