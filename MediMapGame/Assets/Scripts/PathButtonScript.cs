using UnityEngine;
using UnityEngine.SceneManagement;

public class PathButtonScript : MonoBehaviour
{
    public GameObject MovableAvatar;

    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.3f;

    void OnMouseDown()
    {
        MovableAvatar.GetComponent<MovableAvatarScript>().MoveAvatarToLocation(gameObject.transform);
        

        if (Time.time - lastClickTime < doubleClickThreshold)
        {
            Debug.Log(gameObject.name + " double-clicked!");
            if (MovableAvatar.transform.position == gameObject.transform.position)
            {
                SceneManager.LoadScene("LogBookScene");
            }
        }
        lastClickTime = Time.time; // Update last click time
    }

    public void Start()
    {
        MovableAvatar = GameObject.Find("MovableAvatar");
    }
}
