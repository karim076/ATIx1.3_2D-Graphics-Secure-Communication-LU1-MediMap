using Assets.Scripts.Models;
using Assets.Scripts.SessionManager;
using MediMap.Scripts.Api;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class HomeScreenScript : MonoBehaviour
{
    public GameObject[] RoadTiles;
    private LineRenderer lineRendererRoad1;
    private LineRenderer lineRendererRoad2;
    private LineRenderer userProgressLineRenderer;

    private GameObject[] trajectTextList;

    private List<Traject> trajectList;

    private GameObject movableAvatar;


    public int userPathLocation;
    public int userRoute;

    void Start()
    {
        RoadTiles = GameObject.FindGameObjectsWithTag("Pathway")
                              .OrderBy(obj => obj.GetComponent<PathButtonScript>().Route)
                              .ThenBy(obj => obj.GetComponent<PathButtonScript>().Id)
                              .ToArray();
        Debug.Log("session position " + SessionManager.Instance.geustPathLocation);

        movableAvatar = GameObject.Find("MovableAvatar");
        movableAvatar.GetComponent<SpriteRenderer>().sortingOrder = 10;
        trajectTextList = GameObject.FindGameObjectsWithTag("TrajectText").OrderBy(obj => obj.name).ToArray();

        lineRendererRoad1 = GameObject.Find("RoadLine1").GetComponent<LineRenderer>();
        lineRendererRoad2 = GameObject.Find("RoadLine2").GetComponent<LineRenderer>();
        userProgressLineRenderer = GameObject.Find("UserLine").GetComponent<LineRenderer>();

        lineRendererRoad1.sortingOrder = 1;
        lineRendererRoad2.sortingOrder = 1;
        userProgressLineRenderer.sortingOrder = 2;
        
        if (APIManager.Instance.isLogedIn)
        {
            StartCoroutine(GetUserLocation());
        }
        else
        {
            userPathLocation = SessionManager.Instance.geustPathLocation;
            userRoute = 1;
            LoadUserData();
        }

        if (SessionManager.Instance.AvatarName == null)
        {
            Sprite sprite = Resources.Load<Sprite>("Art/Monster1");
            SessionManager.Instance.SetAvatarName(sprite);
        }

        LoadPathWay();
    }

    void DrawLines()
    {
        for (int i = 0; i < RoadTiles.Length; i++)
        {
            RoadTiles[i].GetComponent<SpriteRenderer>().sortingOrder = 3;
            int currentTileRouteId = RoadTiles[i].GetComponent<PathButtonScript>().Route;
            int currentTileId = RoadTiles[i].GetComponent<PathButtonScript>().Id;

            if (currentTileRouteId == 0)
            {
                //Debug.Log("renderd 0");
                lineRendererRoad1.SetPosition(currentTileId, RoadTiles[i].transform.position);
                lineRendererRoad2.SetPosition(currentTileId, RoadTiles[i].transform.position);
            }
            if (currentTileRouteId == 1)
            {
                //Debug.Log("renderd 1");

                lineRendererRoad1.SetPosition(currentTileId, RoadTiles[i].transform.position);
            }
            if (currentTileRouteId == 2)
            {
                //Debug.Log("renderd 2");

                lineRendererRoad2.SetPosition(currentTileId, RoadTiles[i].transform.position);
            }
            
        }
    }

    public void LoadPathWay()
    {
        if (RoadTiles.Length < 2)
        {
            Debug.LogError("Need at least two objects to draw lines!");
            return;
        }

        Color lineColor = new Color(233f / 255f, 216f / 255f, 133f / 255f); // RGB (233, 216, 133)

        lineRendererRoad1.startWidth = 0.5f;
        lineRendererRoad1.endWidth = 0.5f;
        lineRendererRoad1.material = new Material(Shader.Find("Sprites/Default"));
        lineRendererRoad1.startColor = lineColor;
        lineRendererRoad1.endColor = lineColor;
        lineRendererRoad1.positionCount = RoadTiles.Count(obj =>
        obj.GetComponent<PathButtonScript>().Route == 0 ||
        obj.GetComponent<PathButtonScript>().Route == 1);
        //Debug.Log("cuont 1: " + lineRendererRoad1.positionCount);

        lineRendererRoad2.startWidth = 0.5f;
        lineRendererRoad2.endWidth = 0.5f;
        lineRendererRoad2.material = new Material(Shader.Find("Sprites/Default"));
        lineRendererRoad2.startColor = lineColor;
        lineRendererRoad2.endColor = lineColor;
        lineRendererRoad2.positionCount = RoadTiles.Count(obj =>
        obj.GetComponent<PathButtonScript>().Route == 0 ||
        obj.GetComponent<PathButtonScript>().Route == 2);
        //Debug.Log("cuont 2: " + lineRendererRoad2.positionCount);

        DrawLines();
    }

    private void CreateUserProgressLine(GameObject[] userProgressionPathWays)
    {
        Color lineColorUserProgress = new Color(146f / 255f, 138f / 255f, 92f / 255f);

        userProgressLineRenderer.startWidth = 0.6f;
        userProgressLineRenderer.endWidth = 0.6f;
        userProgressLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        userProgressLineRenderer.startColor = lineColorUserProgress;
        userProgressLineRenderer.endColor = lineColorUserProgress;
        userProgressLineRenderer.positionCount = userProgressionPathWays.Length;

        for (int i = 0; i < userProgressionPathWays.Length; i++)
        {
            Debug.Log(userProgressionPathWays[i].GetComponent<PathButtonScript>().Id);
            //var gameObjectScript = ;
            userProgressLineRenderer.SetPosition(userProgressionPathWays[i].GetComponent<PathButtonScript>().Id, userProgressionPathWays[i].transform.position);
            userProgressionPathWays[i].GetComponent<SpriteRenderer>().color = lineColorUserProgress;

        }
    }

    public void LoadUserData()
    {
        GameObject foundTile;
        if (APIManager.Instance.isLogedIn)
        {
            foundTile = RoadTiles
            .FirstOrDefault(tile =>
            {
                PathButtonScript script = tile.GetComponent<PathButtonScript>();
                return script != null && script.Route == 0 && script.Id == userPathLocation;
            });

        }
        else
        {
            //Debug.Log("foundtile thing session " + SessionManager.Instance.geustPathLocation);
            //Debug.Log("ffrom variable " + userPathLocation);
            //Debug.Log(RoadTiles[userPathLocation]);
            foundTile = RoadTiles
            .FirstOrDefault(tile =>
            {
                PathButtonScript script = tile.GetComponent<PathButtonScript>();
                return script != null && script.Route == 0 && script.Id == SessionManager.Instance.geustPathLocation;
            });

        }


        if (foundTile != null)
        {
            Vector3 location = foundTile.transform.position;

            movableAvatar.GetComponent<MovableAvatarScript>().SetAvatarFromDataBase(location);
            movableAvatar.GetComponent<MovableAvatarScript>().SetLocation(foundTile.GetComponent<PathButtonScript>().Id, foundTile.GetComponent<PathButtonScript>().Route);

            Debug.Log("roadtiles length" + RoadTiles);
            GameObject[] userpathWayList = RoadTiles
            .Where(obj =>
            {
                PathButtonScript script = obj.GetComponent<PathButtonScript>();
                return script != null &&script.Route == 0 && script.Id <= userPathLocation;
            })
            .OrderBy(obj => obj.GetComponent<PathButtonScript>().Route)
            .ThenBy(obj => obj.GetComponent<PathButtonScript>().Id)
            .ToArray();
            Debug.Log("userlist size" + userpathWayList.Length);
            if (userpathWayList != null || userpathWayList.Length == 0)
            {
                CreateUserProgressLine(userpathWayList);
            }
        }
        else
        {
            Debug.Log("nog found");
            movableAvatar.GetComponent<MovableAvatarScript>().SetLocation(0, 0);
        }
    }

    private IEnumerator GetUserLocation()
    {
        Debug.Log("userid" + APIManager.Instance.userId);
        yield return APIManager.Instance.SendRequest("api/User/" + APIManager.Instance.userId, "GET", null, response =>
        {
            UserDto responceParsed = JsonConvert.DeserializeObject<UserDto>(response);
            //Debug.Log("response getlocation" + response);
            userPathLocation = responceParsed.PatientPathLocation;
            SessionManager.Instance.loggedUserPathLocation = userPathLocation;
            userRoute = responceParsed.TrajectId ?? 0;

            Debug.Log("setuoasf" + userRoute + " " + userPathLocation);

        }, error =>
        {
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
                // If deserialization fails, use the generic error message
                // ErrorText("Registration failed: " + error);
            }
        });
        LoadUserData();

    }


    private IEnumerator GetAllTraject()
    {
        yield return APIManager.Instance.SendRequest("Api/Traject/All", "GET", null, response =>
        {
            //Debug.Log("before parse" + response);
            List<Traject> responceParsed = JsonConvert.DeserializeObject<List<Traject>>(response);
            trajectList = responceParsed;

            for (int i = 0; i < trajectList.Count(); i++)
            {
                if (trajectList[i] != null || trajectTextList[i] != null)
                {
                    trajectTextList[i].GetComponent<TextMeshPro>().text = trajectList[i].Naam;
                }
                else
                {
                    trajectTextList[i].GetComponent<TextMeshPro>().text = "";
                }
            }

            //foreach (var traject in trajectList)
            //{
                
            //}
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
                // If deserialization fails, use the generic error message
                // ErrorText("Registration failed: " + error);
            }
        });
    }

    
}
