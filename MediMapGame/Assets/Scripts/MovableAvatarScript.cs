using UnityEngine;

public class MovableAvatarScript : MonoBehaviour
{
    public void MoveAvatarToLocation(Transform location)
    {
        transform.position = location.position;
    }
}
