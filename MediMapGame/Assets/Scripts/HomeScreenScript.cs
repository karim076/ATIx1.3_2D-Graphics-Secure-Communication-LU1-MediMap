using Assets.Scripts.Models;
using Assets.Scripts.SessionManager;
using MediMap.Scripts.Api;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
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

    private GameObject[] trajectTextList;

    private List<Traject> trajectList;

    private GameObject movableAvatar;

    void Start()
    {
        movableAvatar = GameObject.Find("MovableAvatar");
        trajectTextList = GameObject.FindGameObjectsWithTag("TrajectText").OrderBy(obj => obj.name).ToArray();

        StartCoroutine(GetAllTraject());
        
        lineRendererRoad1 = GameObject.Find("RoadLine1").GetComponent<LineRenderer>();
        lineRendererRoad2 = GameObject.Find("RoadLine2").GetComponent<LineRenderer>();

        //SessionManager.Instance.LoadHeader();
        //roadTilePrefab = Resources.Load<GameObject>("RoadButtonPrefab");
        RoadTiles = GameObject.FindGameObjectsWithTag("Pathway")
                              .OrderBy(obj => obj.GetComponent<PathButtonScript>().Route)
                              .ThenBy(obj => obj.GetComponent<PathButtonScript>().Id)
                              .ToArray(); // Convert the List<GameObject> back to an array
        LoadPathWay();
        LoadUserData();

    }

    void DrawLines()
    {
        for (int i = 0; i < RoadTiles.Length; i++)
        {
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

    public void LoadUserData()
    {
        if (APIManager.Instance.isLogedIn)
        {
            int userRouteFromDatabase = 2;
            int userPlaceFromDatabase = 7;
            Debug.Log("user is logged in");
            GameObject foundTile = RoadTiles
            .FirstOrDefault(tile =>
            {
                PathButtonScript script = tile.GetComponent<PathButtonScript>();
                return script != null && script.Route == userRouteFromDatabase - 1 && script.Id == userPlaceFromDatabase;
            });
            if(foundTile != null)
            {
                Vector3 location = foundTile.transform.position;

                movableAvatar.GetComponent<MovableAvatarScript>().SetAvatarFromDataBase(location);
                movableAvatar.GetComponent<MovableAvatarScript>().SetLocation(foundTile.GetComponent<PathButtonScript>().Id, foundTile.GetComponent<PathButtonScript>().Route);
            }
            else
            {
                movableAvatar.GetComponent<MovableAvatarScript>().SetLocation(0,0);
            }
        }
        else
        {
            movableAvatar.GetComponent<MovableAvatarScript>().SetLocation(0,0);

        }
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
