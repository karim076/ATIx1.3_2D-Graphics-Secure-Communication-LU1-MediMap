using Assets.models;
using Assets.Scripts.Api.LogBookControllerConnection;
using Assets.Scripts.Models;
using MediMap.Scripts.Api;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class CreatePanelScene : MonoBehaviour
{
    [Header("Panels")]
    public GameObject createPanel;
    public GameObject logPanel;
    public GameObject errorPanel;
    public GameObject inputfieldErroPanel;

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

    public LogbookControllerConnection logbookControllerConnection;

    public void Start()
    {
        createPanel.SetActive(false);
        errorPanel.SetActive(false);
        inputfieldErroPanel.SetActive(false);
        logPanel.SetActive(true);
        scrollbar.SetActive(true);
        StartCoroutine(logbookControllerConnection.GetAllLogs());
    }

    public void OpenCreatePanel()
    {
        createPanel.SetActive(true);
        logPanel.SetActive(false);
        scrollbar.SetActive(false);
        errorPanel.SetActive(false);
        inputfieldErroPanel.SetActive(false);
    }


    public void CloseCreatePanel()
    {
        createPanel.SetActive(false);
        logPanel.SetActive(true);
        scrollbar.SetActive(true);
    }

    public void CreateNewLog()
    {
        if (!DateTime.TryParse(date.text, out DateTime datee))
        {
            Debug.Log("Voer een geldige datum in");
            errorPanel.SetActive(true);
            createPanel.SetActive(false);
            logPanel.SetActive(false);
            scrollbar.SetActive(false);
            inputfieldErroPanel.SetActive(false);
            return;
        }

        var newModel = new LogModelDTO
        {
            PatientId = 1,
            place = place.text,
            note = note.text,
            date = datee
        };

        logbookControllerConnection.SaveLogData(newModel);
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
            Button deleteButton = modelItem.transform.Find("DeleteButton").GetComponent<Button>();

            dateText.text = model.date?.ToString("dd/MM/yyyy");
            placeText.text = model.place;
            noteText.text = model.note;
            deleteButton.onClick.AddListener(() => logbookControllerConnection.DeleteLog(model));
        }
    }
}

