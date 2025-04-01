using UnityEngine;

public class ObjectSwitcher : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject childInfo;
    public GameObject nurseInfo;

    public void SwitchToChild()
    {
        childInfo.SetActive(true);
    }
    public void SwitchToNurse()
    {
        nurseInfo.SetActive(true);
    }
    public void Return()
    {
        childInfo.SetActive(false);
        nurseInfo.SetActive(false);
    }
}
