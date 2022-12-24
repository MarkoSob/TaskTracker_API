using System.Text;

namespace TaskTracker_BL.Services.GeneratorService
{
    public class GeneratorService : IGeneratorService
    {
        public string GetRandomKey()
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!#$%&*^?".ToLower();
            int length = random.Next(10, 20);

            StringBuilder result = new StringBuilder(string.Empty, length);

            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }
    }
}
