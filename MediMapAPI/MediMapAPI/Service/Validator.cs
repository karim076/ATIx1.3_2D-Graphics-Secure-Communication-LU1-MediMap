using System.Text.RegularExpressions;

namespace MediMapAPI.Service
{
    public class Validator
    {
        public static string ValidateUserCredentials(string username, string password, string email)
        {
            if (string.IsNullOrWhiteSpace(username))
                return "Username cannot be empty.";

            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9]+$"))
                return "Username must only contain letters and numbers.";

            if (string.IsNullOrWhiteSpace(password))
                return "Password cannot be empty.";

            if (password.Length < 10)
                return "Password must be at least 10 characters long.";

            if (!Regex.IsMatch(password, @"[a-z]")) return "Password must contain at least one lowercase letter.";
            if (!Regex.IsMatch(password, @"[A-Z]")) return "Password must contain at least one uppercase letter.";
            if (!Regex.IsMatch(password, @"\d")) return "Password must contain at least one number.";
            if (!Regex.IsMatch(password, @"[^a-zA-Z0-9]")) return "Password must contain at least one special character.";

            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return "Invalid email format.";

            return null; // No errors
        }
        // Helper method to validate credentials
        public static string ValidateCredentials(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                return "Username cannot be empty.";

            if (string.IsNullOrWhiteSpace(password))
                return "Password cannot be empty.";

            return null; // No errors
        }
    }
}
