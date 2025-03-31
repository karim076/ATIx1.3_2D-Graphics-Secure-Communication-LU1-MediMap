using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_Text))]
public class TMPLinkClick : MonoBehaviour,
    IPointerClickHandler{
    public string url;
    private string originalText;
    private TMP_Text textComponent;

    void Start()
    {
        textComponent = GetComponent<TMP_Text>();
        originalText = textComponent.text;
        ApplyDefaultStyle();
    }

    void ApplyDefaultStyle()
    {
        textComponent.text = $"<color=white><u><color=#ffffff>{originalText}</color></u></color>";
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        Application.OpenURL(url);
    }

}