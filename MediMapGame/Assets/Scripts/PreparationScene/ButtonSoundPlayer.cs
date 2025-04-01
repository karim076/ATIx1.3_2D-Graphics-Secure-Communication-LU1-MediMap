using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundPlayer : MonoBehaviour
{
    [Header("Sound Settings")]
    [Tooltip("The audio source that will play the sound")]
    public AudioSource audioSource;

    [Tooltip("The sound clip to play when button is clicked")]
    public AudioClip clickSound;

    [Header("Button Assignment")]
    [Tooltip("The button that will trigger the sound")]
    public Button targetButton;



    void Start()
    {
        // If no button is assigned, try to get the button on this GameObject
        if (targetButton == null)
        {
            targetButton = GetComponent<Button>();
        }

        // If we have a button, add the click listener
        if (targetButton != null)
        {
            targetButton.onClick.AddListener(PlayClickSound);
        }
        else
        {
            Debug.LogWarning("No button assigned or found on this GameObject.", this);
        }
    }

    public void PlayClickSound()
    {
        // Check if we have what we need to play sound
        if (audioSource == null)
        {
            Debug.LogWarning("No AudioSource assigned.", this);
            return;
        }

        if (clickSound == null)
        {
            Debug.LogWarning("No click sound assigned.", this);
            return;
        }

        // Play the sound
        audioSource.PlayOneShot(clickSound);
    }

}