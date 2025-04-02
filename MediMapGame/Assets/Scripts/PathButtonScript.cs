using Assets.Scripts.Models;
using Assets.Scripts.SessionManager;
using MediMap.Scripts.Api;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
public class PathButtonScript : MonoBehaviour
{
    public int Id;
    public int Route;
    //public Transform position;

    public GameObject MovableAvatar;
    public string NavigateTo;

    void OnMouseDown()
    {


        if (MovableAvatar.GetComponent<MovableAvatarScript>().pathPosition == Id && MovableAvatar.GetComponent<MovableAvatarScript>().routePosition == Route)
        {
            if (APIManager.Instance.isLogedIn)
            {
                StartCoroutine(UpdatePatientLocation());
            }
            SceneManager.LoadScene(NavigateTo);
        }
        else
        {
            
            MovableAvatar.GetComponent<MovableAvatarScript>().MoveAvatar(Id, Route, gameObject.transform);
        }
    }

    public void Start()
    {
        MovableAvatar = GameObject.Find("MovableAvatar");
        if(NavigateTo == "" || NavigateTo == null)
        {
            NavigateTo = "InfoScene";
        }
    }


    private IEnumerator UpdatePatientLocation()
    {
        int userId = SessionManager.Instance.UserId;
        UserDto updateUser = new UserDto
        {
            Id = userId,
            Username = APIManager.Instance.userName,
            PatientPathLocation = Id,
        };
        Debug.Log(updateUser.ToString());
        yield return APIManager.Instance.SendRequest("api/User/" + userId, "PUT", updateUser, response =>
        {
            Debug.Log(response);
        }, error =>
        {

            // Parse the error response from the API
            try
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorMessage>(error);
                if (!string.IsNullOrEmpty(errorResponse?.message))
                {
                    // ErrorText("Registration failed: " + errorResponse.message); // Use the API's error message
                }
                else
                {
                    // ErrorText("Registration failed: " + error); // Fallback to the generic error message
                }
            }
            catch
            {
            }
        });
    }
}
