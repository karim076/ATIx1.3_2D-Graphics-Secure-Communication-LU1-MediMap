using Assets.Scripts.SessionManager;
using MediMap.Scripts.Api;
using Newtonsoft.Json;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [Header("Camvas")]
    public GameObject Register;
    public GameObject Login;
    [Header("Login Input Fields")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    [Header("Register Input Fields")]
    public TMP_InputField usernameInputRegister;
    public TMP_InputField passwordInputRegister;
    public TMP_InputField emailInputRegister;

    public void OnRegisterClick()
    {
        // Go to Register Scene
        Register.SetActive(true);
        Login.SetActive(false);
    }
    public void LoginPage()
    {
        // Go to Login Scene
        Register.SetActive(false);
        Login.SetActive(true);
    }
    public void ReturnHome()
    {
        // Go to Home Scene
        SceneManager.LoadScene("HomeScreenScene", LoadSceneMode.Single);
    }
    public void OnLoginClick()
    {

        string username = usernameInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            //ErrorText("Username and password cannot be empty.");
            return;
        }

        StartCoroutine(Authenticate(username, password));


    }
    // Login authentication
    private IEnumerator Authenticate(string username, string password)
    {
        //loadingScreenCanvas.gameObject.SetActive(true);

        var user = new { Username = username, Password = password };

        yield return APIManager.Instance.SendRequest("Account/Login", "POST", user, response =>
        {
            //loadingScreenCanvas.gameObject.SetActive(false);
            RefreshTokenResponse responseData = JsonConvert.DeserializeObject<RefreshTokenResponse>(response);
            Debug.Log("Token: " + responseData.Token);
            APIManager.Instance.SaveTokens(responseData);
            Sprite sprite = Resources.Load<Sprite>("Art/Monster1 1_0");
            SessionManager.Instance.SetAvatarName(sprite);
            APIManager.Instance.userName = username;
            APIManager.Instance.isLogedIn = true;
            Debug.Log("Token succesfully created: " + APIManager.Instance.authTokens.Token);
            // Go to Home Scene
            SceneManager.LoadScene("HomeScreenScene", LoadSceneMode.Single);
        }, error =>
        {
            // Show error message
            // loadingScreenCanvas.gameObject.SetActive(false);
            // ErrorText("Login Failed: " + error); // Use the error message returned by APIManager
            Debug.LogError("Login Failed: " + error);
        });
    }

    public void OnRegisterSubmit()
    {
        string username = usernameInputRegister.text.Trim();
        string password = passwordInputRegister.text;
        string email = emailInputRegister.text;
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email))
        {
            //ErrorText("Username, password and email cannot be empty.");
            return;
        }
        StartCoroutine(RegisterAuthentication(username, password, email));
    }

    // Register user
    private IEnumerator RegisterAuthentication(string username, string password, string email)
    {
        // loadingScreenCanvas.gameObject.SetActive(true);

        var user = new CreateUserDto { Id = null,  Username = username, Email = email, Password = password }; // Role = "User" = future implementation

        yield return APIManager.Instance.SendRequest("Account/Create", "POST", user, response =>
        {
            // loadingScreenCanvas.gameObject.SetActive(false);
            LoginPage();
            //SuccessTextLog("Registration successful! You can now log in.");
        }, error =>
        {
            // loadingScreenCanvas.gameObject.SetActive(false);

            // Parse the error response from the API
            try
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorMessage>(error);
                if (!string.IsNullOrEmpty(errorResponse?.message))
                {
                    // ErrorText("Registration failed: " + errorResponse.message); // Use the API's error message
                }
                else
                {
                    // ErrorText("Registration failed: " + error); // Fallback to the generic error message
                }
            }
            catch
            {
                // If deserialization fails, use the generic error message
                // ErrorText("Registration failed: " + error);
            }
        });
    }

    public string ValidateCredentials(string username, string password, string email = "", bool isRegistering = false)
    {
        // Validatielogica hier
        if (isRegistering)
        {
            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return "Ongeldig e-mailformaat.";
        }

        if (string.IsNullOrWhiteSpace(username))
            return "Gebruikersnaam mag niet leeg zijn.";

        if (!Regex.IsMatch(username, @"^[a-zA-Z0-9]+$"))
            return "Gebruikersnaam mag alleen letters en cijfers bevatten.";

        if (password.Length < 10)
            return "Wachtwoord moet minimaal 10 tekens lang zijn.";

        if (!Regex.IsMatch(password, @"[a-z]")) return "Wachtwoord moet minimaal één kleine letter bevatten.";
        if (!Regex.IsMatch(password, @"[A-Z]")) return "Wachtwoord moet minimaal één hoofdletter bevatten.";
        if (!Regex.IsMatch(password, @"\d")) return "Wachtwoord moet minimaal één cijfer bevatten.";
        if (!Regex.IsMatch(password, @"[^a-zA-Z0-9]")) return "Wachtwoord moet minimaal één speciaal teken bevatten.";

        return null;
    }
    public class CreateUserDto
    {
        public int? Id { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
