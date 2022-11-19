using Microsoft.AspNetCore.SignalR;
using TaskTracker_BL.SignalR.ConnectionStorage;

namespace TaskTracker_BL.SignalR
{
    public class AppUser : IUserIdProvider
    {
        private readonly IConnectionStorage _connectionsStorage;

        public AppUser(IConnectionStorage connectionsStorage)
        {
            _connectionsStorage = connectionsStorage;
        }

        public string? GetUserId(HubConnectionContext connection)
        {
            return _connectionsStorage.GetUserNickname(connection.ConnectionId)
                ?? connection.ConnectionId;
        }
    }
}
