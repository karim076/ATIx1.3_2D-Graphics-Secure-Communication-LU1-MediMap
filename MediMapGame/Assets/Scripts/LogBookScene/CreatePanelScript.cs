using Assets.Scripts.SessionManager;
using UnityEngine;
using UnityEngine.UI;

public class CreatePanelScene : MonoBehaviour
{
    

    public GameObject createPanel;
    public GameObject logPanel;

    [SerializeField] private Image _avatar;

    private void Start()
    {
        if (SessionManager.Instance.AvatarName != null)
        {
            _avatar.GetComponent<Image>().sprite = SessionManager.Instance.AvatarName;
        }
    }

    public void OpenCreatePanel()
    {
        createPanel.SetActive(true);
        logPanel.SetActive(false);
    }

    public void CloseCreatePanel()
    {
        createPanel.SetActive(false);
        logPanel.SetActive(true);
    }
}
