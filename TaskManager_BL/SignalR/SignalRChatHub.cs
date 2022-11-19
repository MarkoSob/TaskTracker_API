using Microsoft.AspNetCore.SignalR;
using TaskTracker.Core;
using TaskTracker_BL.SignalR.ConnectionStorage;
using TaskTracker_BL.SignalR.MessageStorage;
using Microsoft.AspNetCore.Authorization;
using TaskTracker_BL.Services;
using TaskTracker_BL.DTOs;

namespace TaskTracker_BL.SignalR
{
    [Authorize]
    public class SignalRChatHub : Hub<ISignalRClient>, ISignalRServer
    {
        private IConnectionStorage _connectionStorage;
        private IMessageStorage _messageStorage;
        private IAuthService _authService;

        public SignalRChatHub(IConnectionStorage connectionStorage, IMessageStorage messageStorage, IAuthService authService)
        {
            _connectionStorage = connectionStorage;
            _messageStorage = messageStorage;
            _authService = authService;
        }

        public async Task<string?> Login(string userLogin, string password)
        {
            CredentialsDto credentialsDto = new CredentialsDto()
            {
                Login = userLogin,
                Password = password
            };
            return await _authService.LoginAsync(credentialsDto);
        }

        public async Task SendMesasageToAll(string message)
        {
            if (await CheckIfUserNameExist())
            {
                var SenderUserInfo = _connectionStorage.GetPublicUserInfo(Context.ConnectionId);
                var messageSnaphot = new MessageSnapshot
                {
                    Message = message,
                    SentDate = DateTime.Now.ToString("HH:mm"),
                    IsPersonal = false,
                    SenderUserInfo = SenderUserInfo!

                };
                await Clients.Others.GetMessage(messageSnaphot);
                _messageStorage.Add(messageSnaphot);
            }
        }

        public async Task SendPersonalMessage(string nickname, string message)
        {
            var connectionId = _connectionStorage.GetConnectionId(nickname);
            var SenderUserInfo = _connectionStorage.GetPublicUserInfo(Context.ConnectionId);
            if (!string.IsNullOrEmpty(connectionId))
            {
                if (await CheckIfUserNameExist())
                {
                    var messageSnaphot = new MessageSnapshot()
                    {
                        ReceiverNickname = nickname,
                        Message = message,
                        SentDate = DateTime.Now.ToString("HH:mm"),
                        IsPersonal = true,
                        SenderUserInfo = SenderUserInfo!

                    };

                    await Clients.Client(connectionId).GetMessage(messageSnaphot);
                    _messageStorage.Add(messageSnaphot);
                }
            }
        }

        public async Task<bool> SetNicknameAndColor(string nickname, string color)
        {
            if (string.IsNullOrEmpty(nickname) || nickname.Length < 3)
                return false;
            return _connectionStorage.TryAddOrUpdateNickname(Context.ConnectionId,
                new PublicUserInfo
                {
                    MessageColor = color,
                    Nickname = nickname
                });
        }

        public async Task<bool> ChangeNickname(string nickname)
        {
            if (string.IsNullOrEmpty(nickname) || nickname.Length < 3)
                return false;
            var UserInfo = _connectionStorage.GetPublicUserInfo(Context.ConnectionId);
            UserInfo.Nickname = nickname;
            return _connectionStorage.TryAddOrUpdateNickname(Context.ConnectionId, UserInfo);
        }

        public async Task<IEnumerable<MessageSnapshot>> GetRecent(int count)
        {
            string? userNickname = _connectionStorage.GetUserNickname(Context.ConnectionId);
            return _messageStorage.GetRecent(userNickname, count);
        }

        public async Task<bool> SetMessageColor(string color)
        {
            return _connectionStorage.SetMessageColor(Context.ConnectionId, color);
        }

        private async Task<bool> CheckIfUserNameExist()
        {

            if (_connectionStorage.GetUserNickname(Context.ConnectionId) == null)
            {
                var messageSnapshot = new MessageSnapshot
                {
                    IsPersonal = true,
                    Message = "You must set nickname before sending messages",
                    SenderUserInfo = new PublicUserInfo
                    {
                        MessageColor = "Red",
                        Nickname = "Server"
                    }
                };
                await Clients.Caller.GetMessage(messageSnapshot);
                return false;
            }
            return true;
        }
    }
}
