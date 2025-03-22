using System.Text;
using System;
using UnityEngine;

public class JwtHelper
{
    public static bool IsTokenExpired(string token)
    {
        try
        {
            // Split the JWT into its parts (header, payload, signature)
            var parts = token.Split('.');
            if (parts.Length != 3)
            {
                Debug.LogWarning("Invalid JWT format.");
                return false; // Assume expired if the token is invalid
            }

            // Decode the payload (second part)
            var payload = parts[1];
            payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '='); // Add padding
            var payloadBytes = Convert.FromBase64String(payload);
            var payloadJson = Encoding.UTF8.GetString(payloadBytes);

            // Parse the payload JSON
            var payloadData = JsonUtility.FromJson<JwtPayload>(payloadJson);

            // Check if the token is expired
            var expiryTime = DateTimeOffset.FromUnixTimeSeconds(payloadData.exp).UtcDateTime;
            Debug.LogWarning("Token expiry time: " + expiryTime);
            return expiryTime < DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error decoding JWT: " + ex.Message);
            return true; // Assume expired if there's an error
        }
    }

    [Serializable]
    private class JwtPayload
    {
        public long exp; // Expiration time (Unix timestamp)
    }
}
