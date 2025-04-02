using Assets.Scripts.SessionManager;
using Unity.VisualScripting;
using UnityEngine;

public class RouteSwitcher : MonoBehaviour
{
    public GameObject userRouteChoice;
    public GameObject routeA;
    public GameObject routeB;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // check if user is logged in
        userRouteChoice.SetActive(false);
        routeA.SetActive(false);
        routeB.SetActive(false);
        try
        {
            if (SessionManager.Instance.UserId == null)
            {
                userRouteChoice.SetActive(true);
                Debug.Log("User not logged in");
                return;
            }
        }
        catch (System.Exception e)
        {
            userRouteChoice.SetActive(true);
            Debug.Log("Go further as guest");
            return;
        }
    }
    public void UserRouteA() 
    {
        userRouteChoice.SetActive(false);
        routeB.SetActive(false);

        routeA.SetActive(true);
    }
    public void UserRouteB()
    {
        userRouteChoice.SetActive(false);
        routeA.SetActive(false);

        routeB.SetActive(true);
    }
}
