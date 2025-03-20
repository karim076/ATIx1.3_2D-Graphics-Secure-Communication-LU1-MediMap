using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfileSceneScript : MonoBehaviour
{
    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("HomeScreenScene", LoadSceneMode.Single);
    }
    public void OnInfoSceneClicked()
    {
        SceneManager.LoadSceneAsync("InfoScene", LoadSceneMode.Single);
    }
    public void OnExitSceneClicked()
    {
        SceneManager.LoadSceneAsync("LoginScene", LoadSceneMode.Single);
    }
}
