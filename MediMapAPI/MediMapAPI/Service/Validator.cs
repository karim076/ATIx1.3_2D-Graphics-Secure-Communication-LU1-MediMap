using System.Text.RegularExpressions;

namespace MediMapAPI.Service
{
    public class Validator
    {
        public static string ValidateUserCredentials(string username, string password, string email)
        {
            if (string.IsNullOrWhiteSpace(username))
                return "Gebruikersnaam mag niet leeg zijn.";

            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9]+$"))
                return "Gebruikersnaam mag alleen letters en cijfers bevatten.";

            if (string.IsNullOrWhiteSpace(password))
                return "Wachtwoord mag niet leeg zijn.";

            if (password.Length < 10)
                return "Wachtwoord moet minimaal 10 tekens lang zijn.";

            if (!Regex.IsMatch(password, @"[a-z]")) return "Wachtwoord moet minimaal een kleine letter bevatten.";
            if (!Regex.IsMatch(password, @"[A-Z]")) return "Wachtwoord moet minimaal een hoofdletter bevatten.";
            if (!Regex.IsMatch(password, @"\d")) return "Wachtwoord moet minimaal een cijfer bevatten.";
            if (!Regex.IsMatch(password, @"[^a-zA-Z0-9]")) return "Wachtwoord moet minimaal een speciaal teken bevatten.";

            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return "Ongeldig e-mailformaat.";

            return null; // Geen fouten
        }

        // Helper method to validate credentials
        public static string ValidateCredentials(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                return "Gebruikersnaam mag niet leeg zijn.";

            if (string.IsNullOrWhiteSpace(password))
                return "Wachtwoord mag niet leeg zijn.";

            return null; // Geen fouten
        }
    }
}