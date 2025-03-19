using Unity.Mathematics;
using UnityEngine;

public class HomeScreenScript : MonoBehaviour
{
    //public GameObject Header;
    public static void LoadHeader()
    {
        GameObject Prefab = Resources.Load<GameObject>("HeaderPrefab");
        Transform canvasTransform = GameObject.Find("Canvas").transform;

        if (Prefab != null)
        {
            GameObject newButton = Instantiate(Prefab, canvasTransform);
            newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 465);
        }
        else
        {
            Debug.LogError("ButtonPrefab not found or Canvas is missing!");
        }
    }
    void Start()
    {
        LoadHeader();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
