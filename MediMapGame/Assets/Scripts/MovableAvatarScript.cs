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
        SetPathPosition(1);

        //for (int i = pathPosition; i >= 3; i++)
        //{
        //    Debug.Log("I value: " + i);

        //    // Get the next position the avatar should move to
        //    Vector3 moveToLocation = homeScreenScript.RoadTiles[i].transform.position;

        //    // Animate the avatar to the new location
        //    AnimatePostion(moveToLocation, _duration);

        //    // Update the path position (move to the next point)
        //    pathPosition += 1;
        //    Debug.Log("path position" + pathPosition);
        //}
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
        //int multiplier = 0;

        //// Find the path object that matches the target location
        //GameObject pathGameObject = homeScreenScript.RoadTiles.Find(obj => obj.transform == location);
        //int pathObjectIndex = homeScreenScript.RoadTiles.IndexOf(pathGameObject);
        //Debug.Log("current place: " + pathObjectIndex);

        //// Determine the direction of movement (forward or backward)
        //multiplier = (pathPosition > pathObjectIndex) ? -1 : 1;

        //// Loop through all the intermediate points and move the avatar
        //for (int i = pathPosition; (multiplier == 1) ? (i <= pathObjectIndex) : (i >= pathObjectIndex); i += multiplier)
        //{
        //    Debug.Log("I value: " + i);

        //    // Get the next position the avatar should move to
        //    Vector3 moveToLocation = homeScreenScript.RoadTiles[i].transform.position;

        //    // Animate the avatar to the new location
            AnimatePostion(location.position, _duration);

        //    // Update the path position (move to the next point)
        //    pathPosition += multiplier;
        //    Debug.Log("path position" + pathPosition);
        //}
        //Debug.Log("DONNNEE");
    }


    public void AnimatePostion(Vector3 endPosition, float duration)
    {
        Debug.Log("animation");
        gameObject.transform.DOLocalMove(endPosition, duration);
    }
}
