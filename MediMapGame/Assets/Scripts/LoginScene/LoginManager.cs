using Assets.Scripts.SessionManager;
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

    public void OnLoginClick()
    {
        SessionManager.Instance.isLogedIn = true;
        // Go to Home Scene

        SceneManager.LoadScene("HomeScreenScene", LoadSceneMode.Single);
        
    }
    public void OnRegisterClick()
    {
        // Go to Register Scene
        Register.SetActive(true);
        Login.SetActive(false);
    }
    public void ReturnToLoginPage()
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
}
