using Unity.Mathematics;
using UnityEngine;

public class HomeScreenScript : MonoBehaviour
{
    public GameObject Header;
    public void LoadHeader()
    {
        //GameObject headerPrefab = GameObject.Find("HeaderPrefab");
        //Vector3 location = new Vector3(0, 465, 0);
        Transform canvasTransform = GameObject.Find("Canvas").transform;
        //location, quaternion.identity
        GameObject newObject = Instantiate(Header, canvasTransform);

        RectTransform buttonRect = newObject.GetComponent<RectTransform>();
        buttonRect.anchoredPosition = new Vector2(0, 465); // Centered



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
