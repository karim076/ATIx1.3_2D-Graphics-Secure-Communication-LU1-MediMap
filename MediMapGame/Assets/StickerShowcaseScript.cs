using Assets.Scripts.SessionManager;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StickerShowcaseScript : MonoBehaviour
{
    public GameObject stickerShowcase;
    public GameObject[] stickerList;

    private Color unlockedColor = Color.white;
    private Color lockedColor = Color.gray;

    private void Start()
    {
        stickerList = GameObject.FindGameObjectsWithTag("ShowcaseStickers").OrderBy(sticker => sticker.name).ToArray();
        int userlocation = SessionManager.Instance.loggedUserPathLocation;
        Debug.Log(userlocation);
        Debug.Log(stickerList.Length);
        for (int i = 0; i <= stickerList.Length - 1; i++)
        {
            if (userlocation >= i || (i == 7 && userlocation == 7))
            {
                stickerList[i].GetComponent<Image>().color = unlockedColor;
            }
            else
            {
                stickerList[i].GetComponent<Image>().color = lockedColor;
            }

        }
    }
    public void OpenStickerShowcase()
    {
        stickerShowcase.SetActive(true);
    }
    public void CloseStickerShowcase()
    {
        stickerShowcase.SetActive(false);
    }
}
