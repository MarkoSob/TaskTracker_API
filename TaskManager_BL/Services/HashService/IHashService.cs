namespace TaskTracker_BL.Services.HashService
{
    public interface IHashService
    {
        public string GetHash(string password);
        public bool ValidateHash(string password, string hash);
    }
}
