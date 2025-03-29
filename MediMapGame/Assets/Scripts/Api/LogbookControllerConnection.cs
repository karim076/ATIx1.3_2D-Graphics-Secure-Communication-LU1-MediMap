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
                StartCoroutine(GetAllLogs());
                Debug.Log(response);
            }, (error) =>
            {
                if (error != null)
                {
                    Debug.Log("database error:  " + error);
                }

                if(error == "HTTP/1.1 400 Bad Request")
                {
                   createPanelScene.inputfieldErroPanel.SetActive(true);
                   createPanelScene.errorPanel.SetActive(false);
                   createPanelScene.createPanel.SetActive(false);
                   createPanelScene.logPanel.SetActive(false);
                   createPanelScene.scrollbar.SetActive(false);
                }
            }));
        }

        public IEnumerator GetAllLogs()
        {
            yield return APIManager.Instance.SendRequest("api/LogBook/", "GET", null, response =>
            {
                List<LogModel> responseParsed = JsonConvert.DeserializeObject<List<LogModel>>(response);
                
                for (int i = 0; i < responseParsed.Count(); i++)
                {
                    if (responseParsed[i] != null)
                    {
                        createPanelScene.LoadLogs(responseParsed);
                    }
                    else
                    {
                        Debug.Log("no logs found");
                    }
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
                StartCoroutine(GetAllLogs());
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

