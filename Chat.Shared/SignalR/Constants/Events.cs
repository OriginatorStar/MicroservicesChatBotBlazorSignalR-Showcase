namespace Chat.Shared.SignalR.Constants
{
    public static class Events
    {
        public const string RECEIVE = "ReceiveMessage";

        public const string SEND = "SendMessage";

        public const string JOINROOM = "JoinRoom";

        public const string LEAVEROOM = "LeaveRoom";

        public const string BOTRECEIVE = "BotReceive";
    }
}