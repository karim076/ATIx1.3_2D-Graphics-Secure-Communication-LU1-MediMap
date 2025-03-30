using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoTimer : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // Sleep je VideoPlayer hiernaartoe
    public string nextScene = "HomeScreenScene";  // Naam van de volgende scene
    public float timerDuration = 3f;  // Hier zet je zelf het aantal seconden

    private float timer;

    void Start()
    {
        videoPlayer.Play();  // Start de video
    }

    void Update()
    {
        timer += Time.deltaTime;  // Tijd telt op

        // Als de timer afloopt, ga naar volgende scene
        if (timer >= timerDuration)
        {
            SceneManager.LoadScene(nextScene);
        }

        // Extra: Druk op een knop om over te slaan (optioneel)
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}