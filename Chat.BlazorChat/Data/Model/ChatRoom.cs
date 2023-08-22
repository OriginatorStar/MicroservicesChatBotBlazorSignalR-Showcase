namespace Chat.BlazorChat.Data.Model
{
    public class ChatRoom
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Message> Messages { get; set; }

        public ChatRoom()
        {
            Messages = new List<Message>();
        }
    }
}
