namespace Models.Model.Dto
{
    public class RefreshTokenResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public int? userId { get; set; }

        public RefreshTokenResponse(string token, string refreshToken, int? id)
        {
            Token = token;
            RefreshToken = refreshToken;
            userId = id;
        }
    }
}
