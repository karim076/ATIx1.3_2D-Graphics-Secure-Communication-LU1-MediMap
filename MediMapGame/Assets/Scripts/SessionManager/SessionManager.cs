using System;
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
        public string Token { get; private set; }

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
        public void SetJwtToken(string token)
        {
            this.Token = token;
        }
        public void ClearToken()
        {
            Token = string.Empty;
        }
    }
}
