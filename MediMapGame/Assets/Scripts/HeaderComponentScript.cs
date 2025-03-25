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
        SceneManager.LoadScene("HomeScreenScene");
    }
    public void LoadProfile()
    {
        if (APIManager.Instance.isLogedIn)
        {
            SceneManager.LoadScene("ProfileScene");
        }
        else
        {
            SceneManager.LoadScene("LoginScene");
        }
    }
    public void LoadLogBook()
    {
        SceneManager.LoadScene("LogBookScene");
    }
}
