using Assets.Scripts.SessionManager;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MovableAvatarScript : MonoBehaviour
{
    public float _duration;
    public int pathPosition;
    public int routePosition;

    private HomeScreenScript homeScreenScript;

    private void Start()
    {
        homeScreenScript = GameObject.Find("EventSystem").GetComponent<HomeScreenScript>();
        //ResetAvatarPosition();
        //SetLocation(0, 0);

        if (SessionManager.Instance.AvatarName != null)
        {
            GameObject.Find("MovableAvatar").GetComponent<SpriteRenderer>().sprite = SessionManager.Instance.AvatarName;
        }
    }

    public void MoveAvatar(int newPosition, int route, Transform newLocation)
    {
        if (!CheckIfCanMoveToPostition(newPosition, route))
        {
            Debug.Log("not good");
            return;
        }
        SetLocation(newPosition, route);
        MoveAvatarToLocation(newLocation);
        Debug.Log("avatar moved");
    }

    public void SetLocation(int position, int route)
    {
        pathPosition = position;
        routePosition = route;
    }

    public bool CheckIfCanMoveToPostition(int moveToPostion, int moveToRoute)
    {
        Debug.Log("current postition:" + pathPosition + "movepotiton:" + moveToPostion);
        if (pathPosition == moveToPostion + 1 || pathPosition == moveToPostion - 1)
        {
            if (routePosition == moveToRoute || (routePosition == 0 && moveToRoute != 0) || (routePosition != 0 && moveToRoute == 0))
            {
                return true;
            }
        }
        else
        {
            return false;
        }
        return false;

    }

    public void SetAvatarFromDataBase(Vector3 location)
    {
        transform.position = location;

    }
    public void ResetAvatarPosition()
    {
        transform.position = homeScreenScript.RoadTiles[0].transform.position;
    }
    public void MoveAvatarToLocation(Transform location)
    {
        AnimatePostion(location.position, _duration);
    }


    public void AnimatePostion(Vector3 endPosition, float duration)
    {
        Debug.Log("animation");
        gameObject.transform.DOLocalMove(endPosition, duration);
    }
}
