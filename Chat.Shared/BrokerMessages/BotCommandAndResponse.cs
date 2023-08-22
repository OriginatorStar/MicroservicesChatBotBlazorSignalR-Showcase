namespace Chat.Shared.BrokerMessages
{
    public class BotCommandAndResponse : IBotCommandAndResponse
    {
        public BotCommandAndResponse()
        {
            Timestamp = DateTime.UtcNow;
        }
        public string Command { get; set; }
        public string? Response { get; set; }
        /// <summary>
        /// Created timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
