using System.Collections;

namespace TaskTracker_DAL.Entities
{
    public class RolesList : IEnumerable<string>
    {
        public const string Admin = nameof(Admin);
        public const string User = nameof(User);

        public IEnumerator<string> GetEnumerator()
        {
            yield return User;
            yield return Admin;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
