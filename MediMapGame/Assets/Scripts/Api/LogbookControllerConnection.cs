using Assets.models;
using Assets.Scripts.ProfileScene.ProfileSceneUI;
using MediMap.Scripts.Api;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Api.LogBookControllerConnection
{
    public class LogbookControllerConnection : MonoBehaviour
    {
        public void SaveLogData(LogModel logModel)
        {
            StartCoroutine(APIManager.Instance.SendRequest($"api/LogBook", "POST", logModel, (response) =>
            {
                Debug.Log(response);
            }, (error) =>
            {
                if (error != null)
                {
                    Debug.Log("database error:  " + error);

                    //var json = JsonConvert.DeserializeObject<ErrorMessage>(error);
                    //if (json != null) { Debug.LogError(json.message); }
                }
            }));
        }
    }
}

