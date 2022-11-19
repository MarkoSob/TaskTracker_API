using TaskTracker.Core;

namespace TaskTracker_BL.SignalR.ConnectionStorage
{
    public interface IConnectionStorage
    {
        string? GetConnectionId(string nickname);
        string? GetUserNickname(string connectionId);
        PublicUserInfo? GetPublicUserInfo(string connectionId);
        void Remove(string connectionId);
        bool SetMessageColor(string connectionId, string color);
        bool TryAddOrUpdateNickname(string connectionId, PublicUserInfo publicUserInfo);
    }
}