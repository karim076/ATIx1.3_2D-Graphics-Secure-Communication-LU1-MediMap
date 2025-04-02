using Ricimi;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionEnd : MonoBehaviour
{
    public GameObject endCanvas;
    public string sceneName;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        endCanvas.gameObject.SetActive(false);
    }
    public void OnLevelCompleteClick()
    {
        // Load the next scene
        endCanvas.gameObject.SetActive(true);
    }
    public void ReturnToHomePath()
    {
        // return to HomeScreenScene
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
