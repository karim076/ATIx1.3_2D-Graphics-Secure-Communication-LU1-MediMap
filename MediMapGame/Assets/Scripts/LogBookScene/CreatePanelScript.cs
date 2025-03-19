using UnityEngine;

public class CreatePanelScene : MonoBehaviour
{
    public GameObject createPanel;
    public GameObject logPanel;

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
