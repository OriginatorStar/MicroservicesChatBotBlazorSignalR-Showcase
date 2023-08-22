using Chat.Shared.BrokerMessages;
using MassTransit;

namespace Chat.DecoupledBot.Consumers
{
    public class StockQuoteCommandConsumer : IConsumer<BotCommandAndResponse>
    {
        readonly ILogger<StockQuoteCommandConsumer> _logger;

        public StockQuoteCommandConsumer(ILogger<StockQuoteCommandConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BotCommandAndResponse> brokerMessage)
        {
            BotCommandAndResponse response = brokerMessage.Message;
            var sentCommand = brokerMessage.Message.Command;
            _logger.LogInformation("Received Command: {Command}", sentCommand);
            try
            {
                if (sentCommand.Split('=')[0] != "/stock")
                {
                    response.Response = "Unrecognized command.";
                    throw new Exception(response.Response);
                }

                response = await MakeAsyncHttpRequest(brokerMessage.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            finally
            {
                // Send the response back to the original sender
                await brokerMessage.RespondAsync<BotCommandAndResponse>(response);
            }
        }

        public async Task<BotCommandAndResponse> MakeAsyncHttpRequest(BotCommandAndResponse botCommandAndResponse)
        {
            using (var httpClient = new HttpClient())
            {
                var stockCode = botCommandAndResponse.Command.Split('=')[1];
                var response = await httpClient.GetAsync($"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv");
                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                var headers = await reader.ReadLineAsync();
                var columns = headers.Split(',');

                var closeIndex = Array.IndexOf(columns, "Close");

                var values = await reader.ReadLineAsync();
                var fields = values.Split(',');

                var closeValue = fields[closeIndex];

                var botResponse = $"{stockCode} quote is ${closeValue} per share";

                botCommandAndResponse.Response = botResponse;

                return botCommandAndResponse;
            }
        }
    }
}
