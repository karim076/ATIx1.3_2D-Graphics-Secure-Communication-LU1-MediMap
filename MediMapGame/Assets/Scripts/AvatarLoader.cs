using Assets.Scripts.SessionManager;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class AvatarLoader : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private SpriteRenderer _avatar;

    [SerializeField] private UnityEngine.UI.Image _avatarImage;

    void Start()
    {
        if (SessionManager.Instance.AvatarName != null)
        {
            if(_avatarImage != null)
            {
                _avatarImage.sprite = SessionManager.Instance.AvatarName;
            }
            else
            {
                _avatar.sprite = SessionManager.Instance.AvatarName;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
