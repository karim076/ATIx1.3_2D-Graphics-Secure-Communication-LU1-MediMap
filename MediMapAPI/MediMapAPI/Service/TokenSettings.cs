namespace MediMapAPI.Models
{
    public class TokenSettings
    {
        public const string SettingsKey = "JWT";
        public required string Key { get; set; }
        public string Audience { get; set; } = "MediMapAPI"; // Add audience
        public string Issuer { get; set; } = "MediMapAPI"; // Add issuer
    }
}
