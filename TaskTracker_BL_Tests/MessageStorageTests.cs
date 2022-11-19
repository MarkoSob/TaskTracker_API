using AutoFixture;
using FluentAssertions;
using TaskTracker.Core;
using TaskTracker_BL.SignalR.MessageStorage;

namespace TaskTracker_BL_Tests
{
    public class MessageStorageTests
    {
        private Fixture _fixture;

        public MessageStorageTests()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void ShouldReturnLastGlobalMessagesWithRequestedCount()
        {
            string _senderNickname = "1111";
            MessageStorage messageStorage = new MessageStorage();
            var expectedResult = new List<MessageSnapshot>();
            for (int i = 0; i < 10; i++)
            {
                var message = AddMessageSnaphot(senderNickname: _senderNickname);
                messageStorage.Add(message);
                expectedResult.Add(message);
            }

            expectedResult = expectedResult.Take(5).ToList();

            var messages = messageStorage.GetRecent(_senderNickname, 5);

            messages.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void ShouldReturnOnlyGlobal_WhenPrivateForOther()
        {
            string _senderNickname = "1111";
            MessageStorage messageStorage = new MessageStorage();
            var expectedResult = new List<MessageSnapshot>();
            for (int i = 0; i < 5; i++)
            {
                var message = AddMessageSnaphot(senderNickname: _senderNickname);
                messageStorage.Add(message);
                expectedResult.Add(message);
                messageStorage.Add(AddMessageSnaphot(isPersonal: true, receiverNickname: "2222"));
            }

            var messages = messageStorage.GetRecent(_senderNickname, 5);

            messages.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void ShouldReturnGlobalAndSenderPersonalMessages()
        {
            string _senderNickname = "1111";
            MessageStorage messageStorage = new MessageStorage();
            var expectedResult = new List<MessageSnapshot>();
            for (int i = 0; i < 5; i++)
            {
                var gloobalMessage = AddMessageSnaphot(senderNickname: _senderNickname);
                var personalMeesage = AddMessageSnaphot(isPersonal: true, senderNickname: _senderNickname);
                messageStorage.Add(gloobalMessage);
                expectedResult.Add(gloobalMessage);
                messageStorage.Add(personalMeesage);
                expectedResult.Add(personalMeesage);
            }

            var messages = messageStorage.GetRecent(_senderNickname, 10);

            messages.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void ShouldReturnGlobalAndReceiverPersonalMessages()
        {
            string _senderNickname = "1111";
            string _receiverNickname = "2222";
            MessageStorage messageStorage = new MessageStorage();
            var expectedResult = new List<MessageSnapshot>();
            for (int i = 0; i < 5; i++)
            {
                var gloobalMessage = AddMessageSnaphot(senderNickname: _senderNickname);
                var personalMeesage = AddMessageSnaphot(isPersonal: true, receiverNickname: _receiverNickname);
                messageStorage.Add(gloobalMessage);
                expectedResult.Add(gloobalMessage);
                messageStorage.Add(personalMeesage);
                expectedResult.Add(personalMeesage);
            }

            var messages = messageStorage.GetRecent(_receiverNickname, 10);

            messages.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void ShouldReturnGlobalAndAllRelatedPersonalMessagesExceptForOthers()
        {
            string _senderNickname = "1111";
            string _receiverNickname = "2222";
            MessageStorage messageStorage = new MessageStorage();
            var expectedResult = new List<MessageSnapshot>();
            for (int i = 0; i < 5; i++)
            {
                var gloobalMessage = AddMessageSnaphot(senderNickname: _senderNickname);
                var personalMeesage = AddMessageSnaphot(isPersonal: true, receiverNickname: _receiverNickname);
                messageStorage.Add(gloobalMessage);
                expectedResult.Add(gloobalMessage);
                messageStorage.Add(personalMeesage);
                expectedResult.Add(personalMeesage);
                messageStorage.Add(AddMessageSnaphot(isPersonal: true, receiverNickname: "3333"));
            }

            var messages = messageStorage.GetRecent(_receiverNickname, 10);

            messages.Should().BeEquivalentTo(expectedResult);
        }

        private MessageSnapshot AddMessageSnaphot(
            string? senderNickname = null,
            string? receiverNickname = null,
            bool isPersonal = false)
        {

            return new MessageSnapshot
            {
                ReceiverNickname = receiverNickname ?? _fixture.Create<string>(),
                Message = _fixture.Create<string>(),
                IsPersonal = isPersonal,
                SenderUserInfo = new PublicUserInfo
                {
                    Nickname = senderNickname
                } ?? _fixture.Create<PublicUserInfo>()
            };
        }
    }
}
