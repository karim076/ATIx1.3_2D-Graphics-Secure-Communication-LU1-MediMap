using Assets.Scripts.SessionManager;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class HomeScreenScript : MonoBehaviour
{
    public GameObject[] RoadTiles;

    //private GameObject roadTilePrefab;

    private LineRenderer lineRendererRoad1;
    private LineRenderer lineRendererRoad2;

    void Start()
    {
        lineRendererRoad1 = GameObject.Find("RoadLine1").GetComponent<LineRenderer>();
        lineRendererRoad2 = GameObject.Find("RoadLine2").GetComponent<LineRenderer>();

        //SessionManager.Instance.LoadHeader();
        //roadTilePrefab = Resources.Load<GameObject>("RoadButtonPrefab");
        RoadTiles = GameObject.FindGameObjectsWithTag("Pathway")
                              .OrderBy(obj => obj.GetComponent<PathButtonScript>().Route)
                              .ThenBy(obj => obj.GetComponent<PathButtonScript>().Id)
                              .ToArray(); // Convert the List<GameObject> back to an array

        //LoadPathWay();
    }

    void DrawLines()
    {
        GameObject SplitTile;
        for (int i = 0; i < RoadTiles.Length; i++)
        {
            int currentTileRouteId = RoadTiles[i].GetComponent<PathButtonScript>().Route;
            int currentTileId = RoadTiles[i].GetComponent<PathButtonScript>().Id;

            if (currentTileRouteId == 0)
            {
                Debug.Log("renderd 0");
                lineRendererRoad1.SetPosition(currentTileId, RoadTiles[i].transform.position);
                lineRendererRoad2.SetPosition(currentTileId, RoadTiles[i].transform.position);
            }
            if (currentTileRouteId == 1)
            {
                Debug.Log("renderd 1");

                lineRendererRoad1.SetPosition(currentTileId, RoadTiles[i].transform.position);
            }
            if (currentTileRouteId == 2)
            {
                Debug.Log("renderd 2");

                lineRendererRoad2.SetPosition(currentTileId, RoadTiles[i].transform.position);
            }
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        
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
        Debug.Log("cuont 1: " + lineRendererRoad1.positionCount);


        lineRendererRoad2.startWidth = 0.5f;
        lineRendererRoad2.endWidth = 0.5f;
        lineRendererRoad2.material = new Material(Shader.Find("Sprites/Default"));
        lineRendererRoad2.startColor = lineColor;
        lineRendererRoad2.endColor = lineColor;
        lineRendererRoad2.positionCount = RoadTiles.Count(obj =>
    obj.GetComponent<PathButtonScript>().Route == 0 ||
    obj.GetComponent<PathButtonScript>().Route == 2);
        Debug.Log("cuont 2: " + lineRendererRoad2.positionCount);


        DrawLines();
    }
}
