using Unity.VisualScripting;
using UnityEngine;

public class RouteConnector : MonoBehaviour
{
    public GameObject homeScreen;
    public GameObject carerMomentOne;
    public GameObject carerMomentTwo;
    public GameObject carerMomentThree;
    public GameObject carerMomentFour;
    void ResetAllCareMoments()
    {
        homeScreen.SetActive(false);
        carerMomentOne.SetActive(false);
        carerMomentTwo.SetActive(false);
        carerMomentThree.SetActive(false);
        carerMomentFour.SetActive(false);
    }

    public void SetCarerMomentOne()
    {
        ResetAllCareMoments();
        carerMomentOne.SetActive(true);
    }
    public void SetCarerMomentTwo()
    {
        ResetAllCareMoments();
        carerMomentTwo.SetActive(true);
    }
    public void SetCarerMomentThree()
    {
        ResetAllCareMoments();
        carerMomentThree.SetActive(true);
    }
    public void SetCarerMomentFour()
    {
        ResetAllCareMoments();
        carerMomentFour.SetActive(true);
    }
}
