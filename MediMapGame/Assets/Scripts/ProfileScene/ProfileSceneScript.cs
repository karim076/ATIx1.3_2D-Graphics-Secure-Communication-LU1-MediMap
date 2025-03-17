using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfileSceneScript : MonoBehaviour
{
    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("HomeScreenScene");
    }
    public void OnInfoSceneClicked()
    {
        SceneManager.LoadSceneAsync("InfoScene");
    }
    public void OnExitSceneClicked()
    {
        SceneManager.LoadSceneAsync("LoginScene");
    }
}
