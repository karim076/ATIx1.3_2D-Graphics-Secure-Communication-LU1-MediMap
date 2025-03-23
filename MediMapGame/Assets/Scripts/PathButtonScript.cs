using UnityEngine;
using UnityEngine.SceneManagement;

public class PathButtonScript : MonoBehaviour
{
    public int Id;
    public int Route;
    //public Transform position;

    public GameObject MovableAvatar;

    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.3f;

    void OnMouseDown()
    {
        MovableAvatar.GetComponent<MovableAvatarScript>().MoveAvatar(Id, Route, gameObject.transform);


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

    //public class RoadPathClass
    //{
        
    //}
}
