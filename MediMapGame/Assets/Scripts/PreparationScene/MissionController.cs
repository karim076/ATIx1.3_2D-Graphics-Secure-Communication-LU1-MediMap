using Ricimi;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionController : MonoBehaviour
{
    public TMP_Text firstMission;
    public TMP_Text secondMission;
    public Button levelCompleteButton;
    public GameObject endCanvas;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        levelCompleteButton.enabled = false;
        endCanvas.gameObject.SetActive(false);
    }

    public void OnKidClick()
    {
        // Change the color of the first mission to green
        firstMission.color = Color.green;
        // Check if the level is complete
        OnLevelComplete();
    }
    public void OnNurseClick()
    {
        // Change the color of the second mission to green
        secondMission.color = Color.green;
        // Check if the level is complete
        OnLevelComplete();
    }
    public void OnLevelComplete()
    {
        if (firstMission.color == Color.green && secondMission.color == Color.green)
        {
            levelCompleteButton.enabled = true;
        }
    }
    public void OnLevelCompleteClick()
    {
        // Load the next scene
        endCanvas.gameObject.SetActive(true);
    }
    public void ReturnToHomePath()
    {
        // return to HomeScreenScene
        SceneManager.LoadScene("HomeScreenScene", LoadSceneMode.Single);
    }
}
