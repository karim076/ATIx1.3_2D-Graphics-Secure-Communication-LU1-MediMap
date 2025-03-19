using UnityEngine;
using UnityEngine.SceneManagement;

public class HeaderComponentScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

        SceneManager.LoadScene("LoginScene");
    }
    public void LoadLogBook()
    {

    }
}
