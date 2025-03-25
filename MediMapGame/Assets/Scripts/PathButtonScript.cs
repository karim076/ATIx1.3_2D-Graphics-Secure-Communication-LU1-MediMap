using UnityEngine;
using UnityEngine.SceneManagement;

public class PathButtonScript : MonoBehaviour
{
    public int Id;
    public int Route;
    //public Transform position;

    public GameObject MovableAvatar;

    void OnMouseDown()
    {


        if (MovableAvatar.GetComponent<MovableAvatarScript>().pathPosition == Id && MovableAvatar.GetComponent<MovableAvatarScript>().routePosition == Route)
        {
            SceneManager.LoadScene("InfoScene");
        }
        else
        {
            MovableAvatar.GetComponent<MovableAvatarScript>().MoveAvatar(Id, Route, gameObject.transform);
        }
    }

    public void Start()
    {
        MovableAvatar = GameObject.Find("MovableAvatar");
    }

}
