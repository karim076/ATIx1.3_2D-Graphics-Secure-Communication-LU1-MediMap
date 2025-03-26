using UnityEngine;

public class RefreshTokenResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public int UserId { get; set; }

    public RefreshTokenResponse(string token, string refreshToken, int userId)
    {
        Token = token;
        RefreshToken = refreshToken;
        UserId = userId;
    }
}
