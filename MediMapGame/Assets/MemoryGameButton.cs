using UnityEngine;

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
                if(memoryManagerScript.NameButtonPressed != ButtonId)
                {
                    EventSystem.GetComponent<MemoryGameManager>().NameButtonPressed = ButtonId;

                    ButtonSelected();
                }
                else
                {
                    EventSystem.GetComponent<MemoryGameManager>().NameButtonPressed = 0;

                    ButtonUnSelected();
                }
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
    public void ButtonUnSelected(int buttonId)
    {

    }
    public void ButtonUnSelected()
    {
    }

    public void ButtonSelected()
    {
    }
}
