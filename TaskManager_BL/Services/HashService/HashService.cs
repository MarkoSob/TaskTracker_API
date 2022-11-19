using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;

using TaskTracker_BL.Options;

namespace TaskTracker_BL.Services.HashService
{
    public class HashService : IHashService
    {
        private readonly HashOptions _hashOptions;

        public HashService(IOptions<HashOptions> hashOptions)
        {
            _hashOptions = hashOptions.Value;
        }

        public string GetHash(string password)
           => Convert.ToBase64String(KeyDerivation.Pbkdf2(
             password: password!,
             salt: Convert.FromBase64String(_hashOptions.Salt),
             prf: KeyDerivationPrf.HMACSHA256,
             iterationCount: _hashOptions.IterationCount,
             numBytesRequested: _hashOptions.NumBytesRequested));

        public bool ValidateHash(string password, string hash)
            => string.Compare(GetHash(password), hash) == 0;
    }
}
