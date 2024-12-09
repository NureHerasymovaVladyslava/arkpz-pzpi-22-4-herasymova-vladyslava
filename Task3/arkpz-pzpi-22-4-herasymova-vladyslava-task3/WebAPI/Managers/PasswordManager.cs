using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public static class PasswordManager
    {
        private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
        private const string Digits = "0123456789";
        private const string SpecialCharacters = "!@#$%^&*()-_=+[]{}|;:,.<>?";

        public static string GenerateTemporaryPassword()
        {
            var random = new Random();
            var allCharacters = Uppercase + Lowercase + Digits + SpecialCharacters;
            var password = new StringBuilder();

            password.Append(Uppercase[random.Next(Uppercase.Length)]);
            password.Append(Lowercase[random.Next(Lowercase.Length)]);
            password.Append(Digits[random.Next(Digits.Length)]);
            password.Append(SpecialCharacters[random.Next(SpecialCharacters.Length)]);

            for (int i = 4; i < 12; i++)
            {
                password.Append(allCharacters[random.Next(allCharacters.Length)]);
            }

            return Shuffle(password.ToString());
        }

        private static string Shuffle(string input)
        {
            var chars = input.ToCharArray();
            var random = new Random();

            for (int i = chars.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (chars[i], chars[j]) = (chars[j], chars[i]);
            }

            return new string(chars);
        }

        public static bool IsValidPassword(string password, out string errorMessage)
        {
            if (password.Length < 8 || password.Length > 16)
            {
                errorMessage = "Password must be between 8 and 16 characters long.";
                return false;
            }

            if (!password.Any(char.IsUpper))
            {
                errorMessage = "Password must contain at least one uppercase letter.";
                return false;
            }

            if (!password.Any(char.IsLower))
            {
                errorMessage = "Password must contain at least one lowercase letter.";
                return false;
            }

            if (!password.Any(char.IsDigit))
            {
                errorMessage = "Password must contain at least one digit.";
                return false;
            }

            if (!password.Any(c => SpecialCharacters.Contains(c)))
            {
                errorMessage = "Password must contain at least one special character.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
