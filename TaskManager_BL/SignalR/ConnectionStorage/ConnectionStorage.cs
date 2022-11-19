using System.Collections.Concurrent;
using TaskTracker.Core;

namespace TaskTracker_BL.SignalR.ConnectionStorage
{
    public class ConnectionStorage : IConnectionStorage
    {
        private ConcurrentDictionary<string, PublicUserInfo> _connections = new ConcurrentDictionary<string, PublicUserInfo>();

        public string? GetUserNickname(string connectionId)
        {
            return _connections.TryGetValue(connectionId, out PublicUserInfo? userInfo) ? userInfo.Nickname : null;
        }
        public PublicUserInfo? GetPublicUserInfo(string connectionId)
        {
            return _connections.TryGetValue(connectionId, out PublicUserInfo? userInfo) ? userInfo : null;
        }

        public string? GetConnectionId(string nickname)
        {
            return _connections.FirstOrDefault(x => x.Value.Nickname == nickname).Key;
        }

        public bool TryAddOrUpdateNickname(string connectionId, PublicUserInfo publicUserInfo)
        {
            if (!_connections.TryAdd(connectionId, publicUserInfo))
            {
                var value = _connections[connectionId];
                return _connections.TryUpdate(connectionId, new PublicUserInfo
                {
                    Nickname = publicUserInfo.Nickname,
                    MessageColor = value.MessageColor
                },
                value);
            }

            return true;
        }

        public void Remove(string connectionId)
        {
            _connections.TryRemove(connectionId, out _);
        }

        public bool SetMessageColor(string connectionId, string color)
        {
            if (_connections.TryGetValue(connectionId, out PublicUserInfo? userInfo))
            {
                userInfo.MessageColor = color;
                return true;
            }

            return false;
        }
    }
}
