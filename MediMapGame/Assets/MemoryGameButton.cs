using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGameButton : MonoBehaviour
{
    public int ButtonId;
    public bool IsUsed;

    private GameObject EventSystem;
    private MemoryGameManager memoryManagerScript;

    public void Start()
    {
        EventSystem = GameObject.Find("EventSystem");
        IsUsed = false;
    }

    public void ButtonsPressed()
    {
        Debug.Log("pressed");
        memoryManagerScript = EventSystem.GetComponent<MemoryGameManager>();

        if (gameObject.name.Contains("Image"))
        {
            if (memoryManagerScript.ImageButtonPressed != 0)
            {
                int oldButtonId = memoryManagerScript.ImageButtonPressed;
                if (memoryManagerScript.ImageButtonPressed != ButtonId)
                {
                    EventSystem.GetComponent<MemoryGameManager>().ImageButtonPressed = ButtonId;

                    ButtonSelected();
                }
                else
                {
                    EventSystem.GetComponent<MemoryGameManager>().ImageButtonPressed = 0;

                    ButtonUnSelected();
                }
                //GameObject deselectButton = EventSystem.GetComponent<MemoryGameManager>().ImageButtons.FirstOrDefault(button =>
                //{
                //    MemoryGameButton script = button.GetComponent<MemoryGameButton>();
                //    return script.ButtonId == oldButtonId;
                //});
                memoryManagerScript.ButtonUnSelected(oldButtonId);

            }
            else
            {
                EventSystem.GetComponent<MemoryGameManager>().ImageButtonPressed = ButtonId;

                ButtonSelected();
            }
        }

        if (gameObject.name.Contains("Name"))
        {

            if (memoryManagerScript.NameButtonPressed != 0)
            {
                int oldButtonId = memoryManagerScript.NameButtonPressed;

                if (memoryManagerScript.NameButtonPressed != ButtonId)
                {

                    EventSystem.GetComponent<MemoryGameManager>().NameButtonPressed = ButtonId;

                    ButtonSelected();
                }
                else
                {
                    EventSystem.GetComponent<MemoryGameManager>().NameButtonPressed = 0;

                    ButtonUnSelected();
                }
                //GameObject deselectButton = EventSystem.GetComponent<MemoryGameManager>().NameButtons.FirstOrDefault(button =>
                //{
                //    MemoryGameButton script = button.GetComponent<MemoryGameButton>();
                //    return script.ButtonId == oldButtonId;
                //});
                //ButtonUnSelected(oldButtonId);
                memoryManagerScript.ButtonUnSelected(oldButtonId);

            }
            else
            {
                EventSystem.GetComponent<MemoryGameManager>().NameButtonPressed = ButtonId;

                ButtonSelected();
            }
        }



        EventSystem.GetComponent<MemoryGameManager>().CheckIfCorrect();
    }
    public void ButtonDisabled()
    {
        gameObject.SetActive(false);
    }
    
    public void ButtonUnSelected()
    {
        //147 118 102
        Color buttonColor = new Color(147f / 255f, 118f / 255f, 102f / 255f);
        gameObject.GetComponent<Image>().color = buttonColor;
    }

    public void ButtonSelected()
    {
        

        gameObject.GetComponent<Image>().color = Color.gray;
    }
}
