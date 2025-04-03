using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MemoryGameManager : MonoBehaviour
{
    public GameObject[] NameButtons; 
    public GameObject[] ImageButtons;

    public int NameButtonPressed;
    public int ImageButtonPressed;

    public GameObject WinScreen;
    public GameObject IncorrectScreen;
    void Start()
    {
        WinScreen = GameObject.Find("WinScreen");
        WinScreen.SetActive(false);
        //IncorrectScreen = GameObject.Find("IncorrectScreen");
        IncorrectScreen.SetActive(false);

        NameButtonPressed = 0;
        ImageButtonPressed = 0;
        NameButtons = GameObject.FindGameObjectsWithTag("MemoryButton")
            .Where(obj => obj.name.Contains("NameButton"))
            .ToArray();

        ImageButtons = GameObject.FindGameObjectsWithTag("MemoryButton")
            .Where(obj => obj.name.Contains("ImageButton"))
            .ToArray();
    }

    public void CheckIfCorrect()
    {
        Debug.Log(NameButtonPressed + " " + ImageButtonPressed);

        if (NameButtonPressed == ImageButtonPressed)
        {
            Debug.Log("is same");
            if(NameButtons.Any(button =>
                button.GetComponent<MemoryGameButton>().ButtonId == NameButtonPressed &&
                button.GetComponent<MemoryGameButton>().IsUsed == false))
            {
                Debug.Log("correct");

                CorrectAwnser();
                ButtonUnSelected(NameButtonPressed);
                ButtonUnSelected(ImageButtonPressed);
            }
            
            
        }
        else if(NameButtonPressed != ImageButtonPressed && NameButtonPressed != 0 && ImageButtonPressed != 0)
        {
            ButtonUnSelected(NameButtonPressed);
            ButtonUnSelected(ImageButtonPressed);
            IncorrectAwnser();
        }

    }

    public void CorrectAwnser()
    {
        MemoryGameButton memoryNameButton = NameButtons.FirstOrDefault(button => button.GetComponent<MemoryGameButton>().ButtonId == NameButtonPressed).GetComponent<MemoryGameButton>();
        MemoryGameButton memoryImageButton = ImageButtons.FirstOrDefault(button => button.GetComponent<MemoryGameButton>().ButtonId == NameButtonPressed).GetComponent<MemoryGameButton>();
        if (memoryNameButton != null && memoryImageButton != null)
        {
            memoryNameButton.IsUsed = true;
            memoryNameButton.ButtonDisabled();

            memoryImageButton.IsUsed = true;
            memoryImageButton.ButtonDisabled();
        }
        memoryImageButton = null;
        memoryNameButton = null;
        NameButtonPressed = 0;
        ImageButtonPressed = 0;

        if (NameButtons.All(button => button.GetComponent<MemoryGameButton>().IsUsed == true))
        {
            WinScreen.SetActive(true);
        }
    }

    public void IncorrectAwnser()
    {
        IncorrectScreen.SetActive(true);

        MemoryGameButton memoryNameButton = NameButtons.FirstOrDefault(button => button.GetComponent<MemoryGameButton>().ButtonId == NameButtonPressed).GetComponent<MemoryGameButton>();
        MemoryGameButton memoryImageButton = ImageButtons.FirstOrDefault(button => button.GetComponent<MemoryGameButton>().ButtonId == ImageButtonPressed).GetComponent<MemoryGameButton>();
        if (memoryNameButton != null && memoryImageButton != null)
        {
            memoryNameButton.IsUsed = false;
            memoryNameButton.ButtonUnSelected();

            memoryImageButton.IsUsed = false;
            memoryImageButton.ButtonUnSelected();
        }
        memoryImageButton = null;
        memoryNameButton = null;
        NameButtonPressed = 0;
        ImageButtonPressed = 0;
    }

    public void ReturnToHomePath()
    {
        SceneManager.LoadScene("HomeScreenScene", LoadSceneMode.Single);
    }

    public void CloseIncorrectScreen()
    {
        IncorrectScreen.SetActive(false);
    }

    public void CloseWinScreen()
    {
        WinScreen.SetActive(false);
    }

    public void ButtonUnSelected(int buttonId)
    {
        GameObject deselectButton = NameButtons.FirstOrDefault(button =>
        {
            MemoryGameButton script = button.GetComponent<MemoryGameButton>();
            return script.ButtonId == buttonId;
        });
        Color buttonColor = new Color(147f / 255f, 118f / 255f, 102f / 255f);
        if(deselectButton != null)
        {
            deselectButton.GetComponent<UnityEngine.UI.Image>().color = buttonColor;
        }
    }
}
