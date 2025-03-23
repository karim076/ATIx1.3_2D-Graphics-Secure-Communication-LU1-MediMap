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
        

        public string AvatarName { get; private set; }


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
        public void SetAvatarName(string avatarName)
        {
            this.AvatarName = avatarName;
        }
    }
}
