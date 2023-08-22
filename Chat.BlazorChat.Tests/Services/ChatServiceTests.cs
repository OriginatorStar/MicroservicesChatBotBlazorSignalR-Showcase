using Bogus;
using Chat.BlazorChat.Data;
using Chat.BlazorChat.Data.Model;
using Chat.BlazorChat.Services;
using Chat.Shared.BrokerMessages;
using FluentAssertions;
using MassTransit;
using Moq.AutoMock;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Chat.BlazorChat.Tests.Services
{
    public class ChatServiceTests
    {
        private readonly ChatService _chatService;
        private readonly Faker _faker;

        public ChatServiceTests()
        {
            _chatService = new ChatService((new AutoMocker()).CreateInstance<ApplicationDbContext>(), (new AutoMocker()).CreateInstance<IRequestClient<BotCommandAndResponse>>());
            _faker = new Faker();
        }

        [Fact]
        public async Task SendMessage_ShouldAddMessageToRoom()
        {
            //Arrange
            var room = new ChatRoom { Id = Guid.NewGuid(), Name = _faker.Random.Word() };
            var user = new User { UserName = _faker.Internet.UserName() };
            var message = new Message { Content = _faker.Random.Words(), ChatRoomId = room.Id, UserId = user.Id };

            //Act
            await _chatService.SendMessageAsync(message);

            //Assert
            var messages = await _chatService.GetLast50MessagesAsync(room.Id);
            messages.Should().Contain(message);
        }

        [Fact]
        public async Task GetMessages_ShouldReturnLast50Messages()
        {
            //Arrange
            var room = new ChatRoom { Id = Guid.NewGuid(), Name = _faker.Random.Word() };
            var user = new User { UserName = _faker.Internet.UserName() };
            var messages = Enumerable.Range(1, 60).Select(_ => new Message { Content = _faker.Random.Words(), ChatRoomId = room.Id, UserId = user.Id });

            foreach (var message in messages)
            {
                await _chatService.SendMessageAsync(message);
            }

            //Act
            var lastMessages = await _chatService.GetLast50MessagesAsync(room.Id);

            //Assert
            lastMessages.Should().HaveCount(50);
        }

        [Fact]
        public async Task GetMessages_ShouldReturnMessagesOrderedByTimestamp()
        {
            //Arrange
            var room = new ChatRoom { Id = Guid.NewGuid(), Name = _faker.Random.Word() };
            var user = new User { UserName = _faker.Internet.UserName() };
            var messages = Enumerable.Range(1, 10).Select(_ => new Message { Content = _faker.Random.Words(), ChatRoomId = room.Id, UserId = user.Id });

            foreach (var message in messages)
            {
                await _chatService.SendMessageAsync(message);
            }

            //Act
            var orderedMessages = await _chatService.GetLast50MessagesAsync(room.Id);

            //Assert
            orderedMessages.Should().BeInDescendingOrder(m => m.Timestamp);
        }
    }
}
