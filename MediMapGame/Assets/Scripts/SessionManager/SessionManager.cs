using Assets.Scripts.Models;
using MediMap.Scripts.Api;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.Scripts.SessionManager
{
    public class SessionManager : MonoBehaviour
    {
        public static SessionManager Instance { get; private set; }
        public Sprite AvatarName { get; private set; }
        public int UserId { get; private set; }
        public int PatientId { get; private set; }

        public int geustPathLocation { get; set; }
        public int loggedUserPathLocation { get; set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public void SetAvatarName(Sprite avatarName)
        {
            this.AvatarName = avatarName;
        }
        public void SetUserId(int userId)
        {
            UserId = userId;
        }
        public void SetPatientId(int patientId)
        {
            PatientId = patientId;
        }

        public void CheckAndLoadAvatar(int patientId)
        {
            StartCoroutine(LoadAvatar(patientId));
        }
        public void Clear()
        {
            PatientId = 0;
            UserId = 0;
            AvatarName = null;
        }

        private IEnumerator LoadAvatar(int patientId)
        {
            yield return APIManager.Instance.SendRequest($"api/Patient/avatar/{patientId}", "GET", null, (response) =>
            {
                var avatarName = JsonConvert.DeserializeObject<AvatarName>(response);
                if (avatarName != null)
                {
                    Sprite sprite = Resources.Load<Sprite>($"Art/{avatarName.avatar}");
                    SetAvatarName(sprite);
                }
            }, (error) =>
            {
                if (error != null)
                {
                    Debug.Log(error);
                }
            });
        }

    }
}
