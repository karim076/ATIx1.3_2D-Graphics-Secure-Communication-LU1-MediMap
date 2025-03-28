using Assets.Scripts.SessionManager;
using MediMap.Scripts.Api;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeaderComponentScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private Button _avatarButton;

    void Start()
    {
        if (SessionManager.Instance.AvatarName != null)
        {
            _avatarButton.GetComponent<Image>().sprite = SessionManager.Instance.AvatarName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadHomeScreen()
    {
        SceneManager.LoadScene("HomeScreenScene", LoadSceneMode.Single);
    }
    public void LoadProfile()
    {
        if (APIManager.Instance.isLogedIn)
        {
            SceneManager.LoadScene("ProfileScene", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("LoginScene", LoadSceneMode.Single);
        }
    }
    public void LoadLogBook()
    {
        SceneManager.LoadScene("LogBookScene", LoadSceneMode.Single);
    }
    public void Logout()
    {
        SessionManager.Instance.Clear();
        APIManager.Instance.Clear();
        SceneManager.LoadScene("LoginScene", LoadSceneMode.Single);
    }
}
