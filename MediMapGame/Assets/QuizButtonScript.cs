using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuizButtonScript : MonoBehaviour
{
    public int QuizButtonId;
    public int QuizButtonQuestionId;

    private GameObject EventSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => SendToEventSystem());
        Debug.Log("added");
        EventSystem = GameObject.Find("EventSystem");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendToEventSystem()
    {
        EventSystem.GetComponent<QuizQuestionManager>().QuizQuestionAwnserLogic(QuizButtonId);
        //FindFirstObjectByType<QuizQuestionManager>().QuizQuestionAwnserLogic(QuizButtonId);
    }
}
