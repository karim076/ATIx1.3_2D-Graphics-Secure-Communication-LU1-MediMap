using UnityEngine;

public class RefreshTokenResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }

    public RefreshTokenResponse(string token, string refreshToken)
    {
        Token = token;
        RefreshToken = refreshToken;
    }
}
