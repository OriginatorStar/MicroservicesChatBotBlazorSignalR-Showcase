using Bogus;
using Chat.BlazorChat.Data;
using Chat.BlazorChat.Services;
using FluentAssertions;
using Moq.AutoMock;
using System.Threading.Tasks;
using Xunit;

namespace Chat.BlazorChat.Tests.Services
{
    public class RoomServiceTests
    {
        private readonly RoomService _roomService;
        private readonly Faker _faker;

        public RoomServiceTests()
        {
            _roomService = new RoomService((new AutoMocker()).CreateInstance<ApplicationDbContext>());
            _faker = new Faker();
        }

        [Fact]
        public async Task Should_Create_New_Room()
        {
            //Arrange
            var roomName = _faker.Random.Word();

            //Act
            var room = await _roomService.CreateRoomAsync(roomName);

            //Assert
            room.Should().NotBeNull();
            room.Name.Should().Be(roomName);
        }

        [Fact]
        public async Task Should_Get_All_Rooms()
        {
            //Arrange
            var roomName1 = _faker.Random.Word();
            var roomName2 = _faker.Random.Word();
            await _roomService.CreateRoomAsync(roomName1);
            await _roomService.CreateRoomAsync(roomName2);

            //Act
            var rooms = await _roomService.GetRoomsAsync();

            //Assert
            rooms.Should().NotBeNull();
            rooms.Should().HaveCount(2);
            rooms.Should().Contain(r => r.Name == roomName1);
            rooms.Should().Contain(r => r.Name == roomName2);
        }
    }
}
