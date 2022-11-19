using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Core
{
    public interface ISignalRServer
    {
        Task SendMesasageToAll(string message);

        Task SendPersonalMessage(string nickname, string message);

        Task<bool> SetNicknameAndColor(string nickname, string color);

        Task<bool> ChangeNickname(string nickname);

        Task<IEnumerable<MessageSnapshot>> GetRecent(int count);

        Task<bool> SetMessageColor(string color);
        Task<string?> Login(string userLogin, string password);
    }
}
