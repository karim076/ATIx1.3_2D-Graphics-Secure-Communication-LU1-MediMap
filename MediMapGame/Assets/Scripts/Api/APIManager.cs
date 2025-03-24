
using Assets.Scripts.SessionManager;
using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms;

namespace MediMap.Scripts.Api
{
    public class APIManager : MonoBehaviour
    {
        private static APIManager _instance;

        private RefreshTokenResponse _authTokens;
        public RefreshTokenResponse authTokens
        {
            get
            {
                // Check if the token is expired
                if (_authTokens != null && JwtHelper.IsTokenExpired(_authTokens.Token))
                {
                    Debug.Log("Token is expired. Refreshing...");
                    RefreshToken();
                }
                return _authTokens;
            }
            set
            {
                _authTokens = value;
            }
        }
        public string userName { get; set; }
        public bool isLogedIn = false;
        public static APIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<APIManager>();
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject("APIManager");
                        _instance = singletonObject.AddComponent<APIManager>();
                    }
                }
                return _instance;
            }
        }

        private string apiBaseUrl = "https://localhost:44340/";

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        public void SaveTokens(RefreshTokenResponse response)
        {
            authTokens = response;
        }

        public void SetData(RefreshTokenResponse newAuthTokens)
        {
            authTokens = newAuthTokens;
        }

        public IEnumerator RefreshToken()
        {
            if (authTokens == null)
            {
                Debug.LogError("No auth tokens available to refresh.");
                yield break;
            }

            //Debug.LogWarning("Refreshing token...");
            var refreshTokenRequest = new RefreshTokenRequest
            {
                RefreshToken = authTokens.RefreshToken
            };

            string jsonData = JsonConvert.SerializeObject(refreshTokenRequest);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonData);

            using (UnityWebRequest request = new UnityWebRequest("https://localhost:7038/Account/RefreshToken", "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(jsonBytes);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    var response = JsonConvert.DeserializeObject<RefreshTokenResponse>(request.downloadHandler.text);
                    SaveTokens(response);
                    Debug.Log("Token refreshed successfully!");
                }
                else
                {
                    Debug.LogError("Failed to refresh token: " + request.error);
                    // Handle error (e.g., redirect to login screen)
                }
            }
        }
        public IEnumerator SendRequest(string endpoint, string method, object data = null, System.Action<string> onSuccess = null, System.Action<string> onError = null)
        {
            string url = apiBaseUrl + endpoint;
            Debug.Log("Sending request to: " + url);

            using (UnityWebRequest request = new UnityWebRequest(url, method))
            {
                // Set the request body if data is provided
                if (data != null)
                {
                    string jsonData = JsonConvert.SerializeObject(data);
                    byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonData);
                    request.uploadHandler = new UploadHandlerRaw(jsonBytes);
                }
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                // Add authorization header if a token exists
                if (SessionManager.Instance != null && !string.IsNullOrEmpty(Instance.authTokens?.Token))
                {
                    request.SetRequestHeader("Authorization", "Bearer " + Instance.authTokens.Token);
                }

                yield return request.SendWebRequest();

                // Log the response for debugging
                Debug.Log("Response Code: " + request.responseCode);

                if (request.result == UnityWebRequest.Result.Success)
                {
                    onSuccess?.Invoke(request.downloadHandler.text);
                }
                else
                {
                    string errorMessage = request.error;
                    if (!string.IsNullOrEmpty(request.downloadHandler.text))
                    {
                        try
                        {
                            var errorResponse = JsonConvert.DeserializeObject<ErrorMessage>(request.downloadHandler.text);
                            if (!string.IsNullOrEmpty(errorResponse?.message))
                            {
                                errorMessage = errorResponse.message;
                            }
                        }
                        catch
                        {
                            // If deserialization fails, keep the default error message
                        }
                    }

                    onError?.Invoke(errorMessage);
                }
            }
        }
    }
}