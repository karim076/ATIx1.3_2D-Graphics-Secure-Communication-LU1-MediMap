using Assets.Scripts.SessionManager;
using MediMap.Scripts.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogBookManagerScript : MonoBehaviour
{

    void Start()
    {
        if (APIManager.Instance.isLogedIn)
        {
            SceneManager.LoadScene("HomeScreenScene", LoadSceneMode.Single);
            
        }

    }
}
