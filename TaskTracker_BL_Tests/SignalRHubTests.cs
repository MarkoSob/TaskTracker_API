using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Moq;
using TaskTracker_BL.SignalR.ConnectionStorage;
using TaskTracker.Core;
using TaskTracker_BL.SignalR.MessageStorage;
using TaskTracker_BL.Services;
using TaskTracker_BL.SignalR;

namespace TaskTracker_BL_Tests
{
    public class SignalRHubTests
    {
        private Fixture _fixture;
        private Mock<IHubCallerClients<ISignalRClient>> _clientsMock;
        private Mock<IMessageStorage> _messageStorage;
        private Mock<IConnectionStorage> _connectionStorage;
        Mock<HubCallerContext> _contextMock;
        Mock<IAuthService> _mockAuthService;
        private SignalRChatHub _hub;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _messageStorage = new Mock<IMessageStorage>();
            _connectionStorage = new Mock<IConnectionStorage>();
            _contextMock = new Mock<HubCallerContext>();
            _mockAuthService = new Mock<IAuthService>();
            _clientsMock = new Mock<IHubCallerClients<ISignalRClient>>();
            _hub = new SignalRChatHub(
                _connectionStorage.Object,
                _messageStorage.Object,
                _mockAuthService.Object);
            _hub.Clients = _clientsMock.Object;
            _hub.Context = _contextMock.Object;
        }

        [Test]
        public void SendMessageToAll_WhenCalled_ShoudAddMesageToStorage()
        {
            string message = _fixture.Create<string>();
            string connectionId = _fixture.Create<string>();
            string userNickName = _fixture.Create<string>();

            _contextMock.Setup(x => x.ConnectionId).Returns(connectionId).Verifiable();
            _connectionStorage.Setup(x => x.GetUserNickname(It.Is<string>(x => x == connectionId))).Returns(userNickName).Verifiable();

            _connectionStorage.Setup(x => x.GetPublicUserInfo(connectionId))
                .Returns(new PublicUserInfo { Nickname = userNickName })
                .Verifiable();

            _clientsMock.Setup(x => x.Others.GetMessage(It.Is<MessageSnapshot>(x =>
              x.IsPersonal == false &&
              x.Message == message &&
              x.SenderUserInfo.Nickname == userNickName &&
              x.ReceiverNickname == null)))
                .Verifiable();

            _messageStorage.Setup(x => x.Add(It.Is<MessageSnapshot>(x =>
            x.SenderUserInfo.Nickname == userNickName &&
            x.Message == message &&
            x.IsPersonal == false && x.ReceiverNickname == null)))
                .Verifiable();

            _hub.SendMesasageToAll(message);

            _connectionStorage.VerifyAll();
            _messageStorage.Verify();
            _clientsMock.Verify();
            _contextMock.Verify();
        }

        [Test]
        public void SendPersonalMessage_WhenCalled_ShoudAddMesageToStorage()
        {
            string message = _fixture.Create<string>();
            string connectionId = _fixture.Create<string>();
            string senderNickName = _fixture.Create<string>();
            string receiverNickName = _fixture.Create<string>();

            _contextMock.Setup(x => x.ConnectionId).Returns(connectionId).Verifiable();

            _connectionStorage.Setup(x => x.GetConnectionId(It.Is<string>(x => x == receiverNickName))).Returns(connectionId).Verifiable();
            _connectionStorage.Setup(x => x.GetUserNickname(It.Is<string>(x => x == connectionId))).Returns(senderNickName).Verifiable();

            _connectionStorage.Setup(x => x.GetPublicUserInfo(connectionId))
                .Returns(new PublicUserInfo { Nickname = senderNickName })
                .Verifiable();

            _clientsMock.Setup(x => x.Client(connectionId).GetMessage(It.Is<MessageSnapshot>(x =>
              x.IsPersonal == true &&
              x.Message == message &&
              x.SenderUserInfo.Nickname == senderNickName &&
              x.ReceiverNickname == receiverNickName)))
                .Verifiable();

            _messageStorage.Setup(x => x.Add(It.Is<MessageSnapshot>(x =>
            x.SenderUserInfo.Nickname == senderNickName &&
            x.Message == message &&
            x.IsPersonal == true &&
            x.ReceiverNickname == receiverNickName)))
                .Verifiable();

            _hub.SendPersonalMessage(receiverNickName, message);

            _connectionStorage.VerifyAll();
            _messageStorage.Verify();
            _clientsMock.Verify();
            _contextMock.Verify();
        }

        [Test]
        public void SendMessageToAll_IfNickNameNotSet_ShoudAddSpecificMesageToStorage()
        {
            string message = "You must set nickname before sending messages";
            string connectionId = _fixture.Create<string>();
            string senderNickName = "Server";

            _contextMock.Setup(x => x.ConnectionId).Returns(connectionId).Verifiable();
            _connectionStorage.Setup(x => x.GetUserNickname(It.Is<string>(x => x == connectionId))).Verifiable();

            _clientsMock.Setup(x => x.Caller.GetMessage(It.Is<MessageSnapshot>(x =>
              x.IsPersonal == true &&
              x.Message == message &&
              x.SenderUserInfo.Nickname == senderNickName
             )))
                .Verifiable();

            _hub.SendMesasageToAll(message);

            _connectionStorage.VerifyAll();
            _clientsMock.Verify();
            _contextMock.Verify();
        }

        [Test]
        public void SendPersonalMessage_IfNickNameNotSet_ShoudAddSpecificMesageToStorage()
        {
            string message = "You must set nickname before sending messages";
            string connectionId = _fixture.Create<string>();
            string senderNickName = "Server";
            string receiverNickName = _fixture.Create<string>();

            _contextMock.Setup(x => x.ConnectionId).Returns(connectionId).Verifiable();

            _connectionStorage.Setup(x => x.GetConnectionId(It.Is<string>(x => x == receiverNickName))).Returns(connectionId).Verifiable();
            _connectionStorage.Setup(x => x.GetUserNickname(It.Is<string>(x => x == connectionId))).Verifiable();

            _clientsMock.Setup(x => x.Caller.GetMessage(It.Is<MessageSnapshot>(x =>
              x.IsPersonal == true &&
              x.Message == message &&
              x.SenderUserInfo.Nickname == senderNickName
             )))
                .Verifiable();

            _hub.SendPersonalMessage(receiverNickName, message);

            _connectionStorage.VerifyAll();
            _clientsMock.Verify();
            _contextMock.Verify();
        }

        [Test]
        public async Task SetNicknameAndColor_WhenCalled_ShouldReturnTrue()
        {
            string connectionId = _fixture.Create<string>();
            string color = _fixture.Create<string>();
            string userNickName = _fixture.Create<string>();

            _contextMock.Setup(x => x.ConnectionId).Returns(connectionId).Verifiable();
            _connectionStorage.Setup(x => x.TryAddOrUpdateNickname(It.Is<string>(x => x == connectionId),
                It.Is<PublicUserInfo>(x =>
                x.Nickname == userNickName &&
                x.MessageColor == color)))
                .Returns(true)
                .Verifiable();

            bool result = await _hub.SetNicknameAndColor(userNickName, color);

            result.Should().BeTrue();
            _connectionStorage.VerifyAll();
            _contextMock.Verify();
        }

        [Test]
        public async Task SetNicknameAndColor_IfNicknameLessThen3Symbols_ShouldReturnFalse()
        {
            string color = _fixture.Create<string>();
            string userNickName = "qq";

            bool result = await _hub.SetNicknameAndColor(userNickName, color);

            result.Should().BeFalse();
            _connectionStorage.VerifyAll();
            _contextMock.Verify();
        }

        [Test]
        public async Task ChangeNickname_WhenCalled_ShouldReturnTrue()
        {
            string connectionId = _fixture.Create<string>();
            string userNickName = _fixture.Create<string>();

            _contextMock.Setup(x => x.ConnectionId).Returns(connectionId).Verifiable();

            _connectionStorage.Setup(x =>
            x.GetPublicUserInfo(It.Is<string>(x => x == connectionId)))
                .Returns(new PublicUserInfo { Nickname = userNickName })
                .Verifiable();

            _connectionStorage.Setup(x => x.TryAddOrUpdateNickname(It.Is<string>(x =>
            x == connectionId),
                It.Is<PublicUserInfo>(x =>
                x.Nickname == userNickName)))
                .Returns(true)
                .Verifiable();

            bool result = await _hub.ChangeNickname(userNickName);

            result.Should().BeTrue();
            _connectionStorage.VerifyAll();
            _contextMock.Verify();
        }

        [Test]
        public async Task ChangeNickname_IfNicknameLessThen3Symbols_ShouldReturnFalse()
        {
            string connectionId = _fixture.Create<string>();
            string userNickName = "qq";

            bool result = await _hub.ChangeNickname(userNickName);

            result.Should().BeFalse();
            _connectionStorage.VerifyAll();
            _contextMock.Verify();
        }
    }
}
