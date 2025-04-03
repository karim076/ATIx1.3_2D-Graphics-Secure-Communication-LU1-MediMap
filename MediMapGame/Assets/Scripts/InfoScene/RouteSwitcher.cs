using Assets.Scripts.Models;
using Assets.Scripts.SessionManager;
using MediMap.Scripts.Api;
using Newtonsoft.Json;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RouteSwitcher : MonoBehaviour
{
    public GameObject userRouteChoice;
    public GameObject routeA;
    public GameObject routeB;

    private int userRoute;

    void Start()
    {
        
        userRouteChoice.SetActive(false);
        routeA.SetActive(false);
        routeB.SetActive(false);
        StartCoroutine(GetUserTraject());
        
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

    private IEnumerator GetUserTraject()
    {
        // 1. Null-check voor APIManager.Instance
        if (APIManager.Instance == null)
        {
            Debug.LogError("APIManager.Instance is null");
            userRouteChoice.SetActive(true);
            yield break;
        }

        // 2. Null-check voor userId
        if (APIManager.Instance.userId == null)
        {
            Debug.LogError("userId is null or empty");
            userRouteChoice.SetActive(true);
            yield break;
        }

        if(APIManager.Instance.isLogedIn == true)
        {
            yield return APIManager.Instance.SendRequest($"api/User/{APIManager.Instance.userId}", "GET", null, response =>
            {
                try
                {
                    // 3. Veilige deserialisatie met null-checks
                    UserDto responceParsed = JsonConvert.DeserializeObject<UserDto>(response);

                    if (responceParsed == null)
                    {
                        Debug.LogError("Failed to parse API response");
                        userRouteChoice.SetActive(true);
                        return;
                    }

                    // 4. Default waarde voor TrajectId als het null is
                    userRoute = responceParsed.TrajectId ?? 0;

                    // 5. Veilige toegang tot geneste properties
                    if (responceParsed.TrajectId != null)
                    {
                        Debug.Log($"userRoute: {responceParsed.TrajectId ?? null}");
                    }
                    else
                    {
                        Debug.Log("Patient data is null");
                    }

                    // 6. Route selectie met null-checks
                    if (!routeA || !routeB || !userRouteChoice)
                    {
                        Debug.LogError("Route GameObjects are not assigned in inspector");
                        return;
                    }

                    if (userRoute == 1)
                    {
                        routeA.SetActive(true);
                        routeB.SetActive(false);
                        userRouteChoice.SetActive(false);
                    }
                    else if (userRoute == 2)
                    {
                        routeB.SetActive(true);
                        routeA.SetActive(false);
                        userRouteChoice.SetActive(false);
                    }
                    else
                    {
                        userRouteChoice.SetActive(true);
                        routeA.SetActive(false);
                        routeB.SetActive(false);
                    }
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"Error processing response: {e.Message}");
                        userRouteChoice.SetActive(true);
                    }
                        },
                    error =>
                    {
                        Debug.LogError($"API request failed: {error}");
                        userRouteChoice.SetActive(true);
                    });
        }
        else
        {
            userRouteChoice.SetActive(true);
            yield break;
        }

        
    }
}
