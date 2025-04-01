using NUnit.Framework;
using System;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizQuestionManager : MonoBehaviour
{
    private int correctAwnerQuestion1;
    private int correctAwnerQuestion2;

    private int currentQuestion;
    public GameObject[] questionUis;
    public GameObject[] quizButtons;

    public GameObject finishedQuizUi;
    //public List<GameObject> questionUis;
    //public GameObject question1Ui;
    //public GameObject question2Ui;

    public void Start()
    {
        currentQuestion = 1;
        correctAwnerQuestion1 = 1;
        correctAwnerQuestion2 = 7;

        foreach (var item in questionUis)
        {
            item.SetActive(true);
        }

        quizButtons = GameObject.FindGameObjectsWithTag("QuizButton");
        Debug.Log(quizButtons.Length);

        foreach (var item in questionUis)
        {
            item.SetActive(false);
        }
        questionUis[0].SetActive(true);

    }

    public void CorrectAwnserLogic()
    {
        questionUis[currentQuestion - 1].SetActive(false);
        
        if (currentQuestion - 1 >= 0 && currentQuestion < questionUis.Length)
        {
            questionUis[currentQuestion].SetActive(true);
            currentQuestion++;
        }
        else
        {
            finishedQuizUi.SetActive(true);
        }
        Debug.Log("correct");


    }
    public void InCorrectAwnserLogic(int buttonId)
    {
        GameObject test = quizButtons.Select(go => go.GetComponent<QuizButtonScript>())
            .FirstOrDefault(script => script != null && script.QuizButtonId == buttonId)?.gameObject;
        if (!test)
        {
            Debug.Log("not found");
        }
        test.SetActive(false); 
        Debug.Log("disabled");

    }
    public void QuizQuestionAwnserLogic(int buttonId)
    {
        Debug.Log("is in button logix" + buttonId);
        switch (currentQuestion)
        {
            case 1:
                if (buttonId == correctAwnerQuestion1)
                {
                    CorrectAwnserLogic();
                }
                else
                {
                    InCorrectAwnserLogic(buttonId);
                }
                break;
            case 2:
                if (buttonId == correctAwnerQuestion2)
                {
                    CorrectAwnserLogic();
                }
                else
                {
                    InCorrectAwnserLogic(buttonId);
                }


                break;
        }

    }

    public void NavigatToHome()
    {
        SceneManager.LoadScene("HomeScreenScene");
    }
}
