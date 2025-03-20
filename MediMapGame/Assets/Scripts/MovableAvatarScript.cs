using DG.Tweening;
using UnityEngine;

public class MovableAvatarScript : MonoBehaviour
{
    public float _duration;
    public void MoveAvatarToLocation(Transform location)
    {
        AnimatePostion(location.position, _duration);
        //transform.position = location.position;
    }

    public void AnimatePostion(Vector3 endPosition, float duration)
    {
        gameObject.transform.DOLocalMove(endPosition, duration);
    }
}
