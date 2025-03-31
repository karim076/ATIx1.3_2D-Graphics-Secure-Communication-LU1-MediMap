using Assets.Scripts.Models;
using Assets.Scripts.Models.ViewModel;
using Assets.Scripts.SessionManager;
using MediMap.Scripts.Api;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private TMP_InputField _sureNameInputField; 
    [SerializeField] private TMP_InputField _doctorName;
    [SerializeField] private TMP_InputField _gardianName;
    [SerializeField] private TMP_InputField _gardianSurName;
    [SerializeField] private TMP_InputField _birthDay;
    [SerializeField] private TMP_InputField _appointmentDate;
    [SerializeField] private TMP_Dropdown _trajectDropdown;

    private List<Traject> allTrajects = new List<Traject>();




    private RegisterViewModel RegisterViewModel()
    {
        if(!DateTime.TryParse(_birthDay.text, out DateTime result))
        {
            Debug.LogError("Geboortedatum is niet correct.");
            return null;
        }
        if (!DateTime.TryParse(_appointmentDate.text, out DateTime result2))
        {
            Debug.LogError("Afspraakdatum is niet correct.");
            return null;
        }
        return new RegisterViewModel
        {
            CreateUserDto = new CreateUserDto
            {
                Id = null,
                Username = usernameInputRegister.text.Trim(),
                Email = emailInputRegister.text.Trim(),
                Password = passwordInputRegister.text
            },
            Arts = new Arts
            {
                Id = 0,
                Naam = _doctorName.text.Trim(),
                Specialisatie = "KunstGebit",
            },
            Patient = new Patient
            {
                Id = 0,
                VoorNaam = _nameInputField.text.Trim(),
                AchterNaam = _sureNameInputField.text.Trim(),
                AvatarNaam = "",
                Afspraakatum = result2,
                GeboorteDatum = result,
                PathLocation = 0,
                ArtsNaam = "",
                OuderVoogdNaam = "",
                TrajectNaam = ""
            },
            OuderVoogd = new OuderVoogd
            {
                Id = 0,
                VoorNaam = _gardianName.text.Trim(),
                AchterNaam = _gardianSurName.text.Trim()
            },
            trajectId = allTrajects[_trajectDropdown.value].Id
        };
    }

    public void AddTrajctToDropDown(List<Traject> trajects)
    {
        _trajectDropdown.options.Clear();
        foreach (var traject in trajects)
        {
            _trajectDropdown.options.Add(new TMP_Dropdown.OptionData(traject.Naam));
        }
    }

    private IEnumerator GetTrajects()
    {
        yield return APIManager.Instance.SendRequest("api/Traject/all", "GET", null, response =>
        {
            if (string.IsNullOrEmpty(response))
            {
                Debug.LogError("No data received from the API.");
                return;
            }

            allTrajects = JsonConvert.DeserializeObject<List<Traject>>(response);
            AddTrajctToDropDown(allTrajects);
        }, error =>
        {
            Debug.LogError("Error: " + error);
        });
    }

    [Header("Logging")]
    public TMP_Text errorText;
    public TMP_Text successText;

    [Header("Canvas")]
    public Canvas canvasError;
    public Canvas canvasSuccess;

    public void OnRegisterClick()
    {
        // Go to Register Scene
        StartCoroutine(GetTrajects());
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
            ErrorText("Gebruikersnaam and wachtwoord mag niet leeg zijn.");
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
            SessionManager.Instance.SetUserId(responseData.UserId);
            SessionManager.Instance.SetPatientId(responseData.PatientId);
            SessionManager.Instance.CheckAndLoadAvatar(responseData.PatientId);
            APIManager.Instance.userName = username;
            APIManager.Instance.isLogedIn = true;
            APIManager.Instance.userId = responseData.UserId;
            Debug.Log("Token succesfully created: " + APIManager.Instance.authTokens.Token);
            // Go to Home Scene
            SceneManager.LoadScene("HomeScreenScene", LoadSceneMode.Single);
        }, error =>
        {
            // Show error message
            // loadingScreenCanvas.gameObject.SetActive(false);
            ErrorText("Login Failed: " + error); // Use the error message returned by APIManager
        });
    }

    

    public void OnRegisterSubmit()
    {
        string username = usernameInputRegister.text.Trim();
        string password = passwordInputRegister.text;
        string email = emailInputRegister.text;
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email))
        {
            ErrorText("Gebruikersnaam, Wachtwoord en email mag niet leeg zijn.");
            return;
        }
        var registerViewModel = RegisterViewModel();
        StartCoroutine(RegisterAuthentication(registerViewModel));
    }

    // Register user
    private IEnumerator RegisterAuthentication(RegisterViewModel registerViewModel)
    {
        // loadingScreenCanvas.gameObject.SetActive(true);

        //var user = new CreateUserDto { Id = null,  Username = username, Email = email, Password = password }; // Role = "User" = future implementation
        
        yield return APIManager.Instance.SendRequest("Account/Create", "POST", registerViewModel, response =>
        {
            // loadingScreenCanvas.gameObject.SetActive(false);
            LoginPage();
            SuccessTextLog("Registration successful! You can now log in.");
        }, error =>
        {
            // loadingScreenCanvas.gameObject.SetActive(false);

            // Parse the error response from the API
            try
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorMessage>(error);
                if (!string.IsNullOrEmpty(errorResponse?.message))
                {
                    ErrorText("Registratie mislukt: " + errorResponse.message); // Use the API's error message
                }
                else
                {
                    ErrorText("Registratie mislukt: " + error); // Fallback to the generic error message
                }
            }
            catch
            {
                // If deserialization fails, use the generic error message
                ErrorText("Registratie mislukt: " + error);
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
    public void ErrorText(string message)
    {
        canvasSuccess.gameObject.SetActive(false);
        canvasError.gameObject.SetActive(true);
        errorText.text = message;
    }

    public void SuccessTextLog(string message)
    {
        canvasError.gameObject.SetActive(false);
        canvasSuccess.gameObject.SetActive(true);
        successText.text = message;
    }
}
