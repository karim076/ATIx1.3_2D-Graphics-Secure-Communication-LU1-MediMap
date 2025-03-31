using Assets.models;
using Assets.Scripts.Models;
using Assets.Scripts.ProfileScene.ProfileSceneUI;
using MediMap.Scripts.Api;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.SessionManager;
using JetBrains.Annotations;

namespace Assets.Scripts.Api.LogBookControllerConnection
{
    public class LogbookControllerConnection : MonoBehaviour
    {
        [Header("scripts")]
        public CreatePanelScene createPanelScene;

        public void SaveLogData(LogModelDTO logModel)
        {
            StartCoroutine(APIManager.Instance.SendRequest($"api/LogBook", "POST", logModel, (response) =>
            {
                StartCoroutine(GetAllLogsFromPatient());
                Debug.Log(response);
            }, (error) =>
            {
                if (error != null)
                {
                    Debug.Log("database error:  " + error);
                }

                if (error == "HTTP/1.1 400 Bad Request")
                {
                    createPanelScene.logPanel.SetActive(false);
                    createPanelScene.scrollbar.SetActive(false);
                    createPanelScene.createPanel.SetActive(false);
                    createPanelScene.inputfieldErroPanel.SetActive(true);
                }
            }));
        }

        public IEnumerator GetAllLogsFromPatient()
        {
            yield return APIManager.Instance.SendRequest($"api/LogBook/{SessionManager.SessionManager.Instance.PatientId}", "GET", null, response =>
            //yield return APIManager.Instance.SendRequest($"api/LogBook?id=1", "GET", null, response =>
            {
                List<LogModel> responseParsed = JsonConvert.DeserializeObject<List<LogModel>>(response);

                if (responseParsed == null)
                {
                    foreach (Transform child in createPanelScene.gridContent)
                    {
                        Destroy(child.gameObject);
                    }
                    Debug.Log("no logs found");
                }
                else
                {
                    createPanelScene.LoadLogs(responseParsed);
                }
            }, error =>
            {
                if (error != null)
                {
                    Debug.Log("database error:  " + error);
                }
            });
        }

        public void DeleteLog(LogModel model)
        {
            StartCoroutine(APIManager.Instance.SendRequest($"api/LogBook/{model.id}", "DELETE", model, (response) =>
            {
                StartCoroutine(GetAllLogsFromPatient());
                Debug.Log(response);
            }, (error) =>
            {
                if (error != null)
                {
                    Debug.Log("database error:  " + error);
                }
            }));
        }
    }
}

