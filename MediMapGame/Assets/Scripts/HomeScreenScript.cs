using Assets.Scripts.SessionManager;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class HomeScreenScript : MonoBehaviour
{
    public List<GameObject> RoadTiles;
    //private GameObject roadTilePrefab;

    private LineRenderer lineRenderer;

    void Start()
    {
        //SessionManager.Instance.LoadHeader();
        //roadTilePrefab = Resources.Load<GameObject>("RoadButtonPrefab");

        

        LoadPathWay();
    }

    void DrawLines()
    {
        for (int i = 0; i < RoadTiles.Count; i++)
        {
            if (RoadTiles[i] != null)
            {
                lineRenderer.SetPosition(i, RoadTiles[i].transform.position);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadPathWay()
    {
        if (RoadTiles.Count < 2)
        {
            Debug.LogError("Need at least two objects to draw lines!");
            return;
        }

        // Add LineRenderer if missing
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // LineRenderer settings
        lineRenderer.startWidth = 0.5f; // Thickness of the line
        lineRenderer.endWidth = 0.5f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Default material

        Color lineColor = new Color(233f / 255f, 216f / 255f, 133f / 255f); // RGB (233, 216, 133)
        // Set color
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;

        lineRenderer.positionCount = RoadTiles.Count;

        DrawLines();
        //foreach (GameObject roadTile in RoadTiles)
        //{
        //    if(roadTilePrefab != null)
        //    {
        //        Instantiate(roadTilePrefab, roadTile, quaternion.identity);
        //    }
        //}
    }
}
