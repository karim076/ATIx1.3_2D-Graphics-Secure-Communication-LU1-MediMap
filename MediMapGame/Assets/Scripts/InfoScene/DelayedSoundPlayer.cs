using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.SessionManager;

public class DelayedSoundPlayer : MonoBehaviour
{
    public AudioClip soundToPlay;
    public float delayInSeconds = 2f;





    private AudioSource audioSource;

    void Start()
    {

        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        StartCoroutine(PlaySoundAfterDelay());
    }

    IEnumerator PlaySoundAfterDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);
        if (soundToPlay != null)
        {
            audioSource.PlayOneShot(soundToPlay);
        }
    }
}