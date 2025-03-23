using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MovableAvatarScript : MonoBehaviour
{
    public float _duration;
    private int pathPosition;

    private HomeScreenScript homeScreenScript;

    private void Start()
    {
        homeScreenScript = GameObject.Find("EventSystem").GetComponent<HomeScreenScript>();
        ResetAvatarPosition();
        SetPathPosition(0);
    }

    public void MoveAvatar(int newPosition, int route, Transform newLocation)
    {
        if (!CheckIfCanMoveToPostition(newPosition))
        {
            Debug.Log("not good");
            return;
        }
        SetPathPosition(newPosition);
        MoveAvatarToLocation(newLocation);
        Debug.Log("avatar moved");
    }

    public void SetPathPosition(int position)
    {
        pathPosition = position;
    }

    public bool CheckIfCanMoveToPostition(int moveToPostion)
    {
        Debug.Log("current postition:" + pathPosition + "movepotiton:" + moveToPostion);
        return (pathPosition == moveToPostion + 1 || pathPosition == moveToPostion - 1) ? true : false;
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
