using Chat.DecoupledBot.Consumers;
using Chat.Shared.BrokerMessages;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Chat.DecoupledBot.Tests.Consumers
{
    public class StockQuoteCommandConsumerTests
    {
        [Fact]
        public async Task MakeAsyncHttpRequest_ReturnsCorrectResponse()
        {
            // Arrange
            var httpClient = new HttpClient(new TestHttpMessageHandler(async request =>
            {
                request.RequestUri.ToString().Should().StartWith("https://stooq.com/q/l/?s=");
                request.RequestUri.ToString().Should().Contain("&f=sd2t2ohlcv&h&e=csv");

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("Symbol,Date,Time,Open,High,Low,Close,Volume\nMSFT,2023-07-01,16:00:00,275.00,280.00,273.50,278.50,1000")
                };

                return await Task.FromResult(response);
            }));

            var service = new StockQuoteCommandConsumer(new Mock<ILogger<StockQuoteCommandConsumer>>().Object);

            var botCommandAndResponse = new BotCommandAndResponse
            {
                Command = "SomeCommand=MSFT",
                Response = "Initial Response" // Optional: Set an initial response if needed
            };

            // Act
            var result = await service.MakeAsyncHttpRequest(botCommandAndResponse);

            // Assert
            result.Should().NotBeNull();
            result.Response.Should().Be("Bot: MSFT quote is 278.50 per share");
        }

        // TestHttpMessageHandler for mocking HttpClient responses
        public class TestHttpMessageHandler : HttpMessageHandler
        {
            private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _handlerFunc;

            public TestHttpMessageHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> handlerFunc)
            {
                _handlerFunc = handlerFunc;
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
            {
                return await _handlerFunc(request);
            }
        }
    }
}
