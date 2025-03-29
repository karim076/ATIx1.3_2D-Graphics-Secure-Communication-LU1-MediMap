namespace Models.Model.Dto
{
    public class RefreshTokenResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public int UserId { get; set; }
        public int? PatientId { get; set; }

        public RefreshTokenResponse(string token, string refreshToken, int userId, int? patientId)
        {
            Token = token;
            RefreshToken = refreshToken;
            UserId = userId;
            PatientId = patientId;
        }
    }
}
