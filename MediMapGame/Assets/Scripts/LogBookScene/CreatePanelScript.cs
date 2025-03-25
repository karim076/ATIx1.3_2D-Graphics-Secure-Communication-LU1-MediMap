using Assets.models;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CreatePanelScene : MonoBehaviour
{
    [Header("Panels")]
    public GameObject createPanel;
    public GameObject logPanel;

    [Header("Prefabs")]
    public GameObject prefab;
    public Transform gridContent;

    [Header("Input Fields")]
    public TMP_InputField date;
    public TMP_InputField place;
    public TMP_InputField note;

    [Header("Canvas")]
    public GameObject canvas;

    [Header("Objects")]
    public GameObject scrollbar;

    public List<LogModel> models = new List<LogModel>();

    public void OpenCreatePanel()
    {
        createPanel.SetActive(true);
        logPanel.SetActive(false);
        scrollbar.SetActive(false);
    }


    public void CloseCreatePanel()
    {
        createPanel.SetActive(false);
        logPanel.SetActive(true);
        scrollbar.SetActive(true);
    }

    public void CreateNewLog()
    {
        var newModel = new LogModel
        {
            Date = date.text,
            Place = place.text,
            Note = note.text
        };

        models.Add(newModel);
        LoadLogs(models);
    }
    public void LoadLogs(List<LogModel> models)
    {
        foreach (Transform child in gridContent)
        {
            Destroy(child.gameObject);
        }

        foreach (LogModel model in models)
        {
            GameObject modelItem = Instantiate(prefab, gridContent);
            modelItem.transform.SetParent(gridContent, false);

            TMP_Text dateText = modelItem.transform.Find("Date").GetComponent<TMP_Text>();
            TMP_Text placeText = modelItem.transform.Find("Place").GetComponent<TMP_Text>();
            TMP_Text noteText = modelItem.transform.Find("Note").GetComponent<TMP_Text>();

            dateText.text = model.Date;
            placeText.text = model.Place;
            noteText.text = model.Note;
        }
    }
}
