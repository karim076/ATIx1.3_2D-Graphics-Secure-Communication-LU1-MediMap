using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

namespace Assets.Scripts.ProfileScene.ProfileSceneUI
{
    public class ProfileSceneUI : MonoBehaviour
    {
        [SerializeField] private Image _userName;
        [SerializeField] private Image _userBirthDay;
        [SerializeField] private Image _userAppointment;
        [SerializeField] private Image _doctorName;
        [SerializeField] private Image _treatmentPlan;

        [SerializeField] private GameObject _avatarPanel1;
        [SerializeField] private GameObject _avatarPanel2;

        private TMP_Text _userNameText;
        private TMP_Text _userBirthDayText;
        private TMP_Text _userAppointmentText;
        private TMP_Text _doctorNameText;
        private TMP_Text _treatmentPlanText;

        private List<string> images = new List<string>();

        private void Start()
        {
            InitializeTextComponent();

            CollectAvatarNames();

            MakeAvatarsClickble();

            SetProfileData("John Doe", "01-01-2000", "Dentist", "Dr. Smith", "Treatment plan");
        }
        public void SetProfileData(string userName, string userBirthDay, string userAppointment, string doctorName, string treatmentPlan)
        {
            _userNameText.text = userName;
            _userBirthDayText.text = userBirthDay;
            _userAppointmentText.text = userAppointment;
            _doctorNameText.text = doctorName;
            _treatmentPlanText.text = treatmentPlan;
        }

        private void InitializeTextComponent()
        {
            _userNameText = _userName.GetComponentInChildren<TMP_Text>();
            _userBirthDayText = _userBirthDay.GetComponentInChildren<TMP_Text>();
            _userAppointmentText = _userAppointment.GetComponentInChildren<TMP_Text>();
            _doctorNameText = _doctorName.GetComponentInChildren<TMP_Text>();
            _treatmentPlanText = _treatmentPlan.GetComponentInChildren<TMP_Text>();

            if (_userNameText == null || _userBirthDayText == null || _userAppointmentText == null || _doctorNameText == null || _treatmentPlanText == null)
            {
                Debug.LogError("Geen Text gevonden");
            }
        }
        private void CollectAvatarNames()
        {
            if(_avatarPanel1 != null)
            {
                Image[] image = _avatarPanel1.GetComponentsInChildren<Image>();
                if (image != null)
                {
                    foreach (var item in image.Skip(1))
                    {
                        images.Add(item.gameObject.name);
                    }
                }
            }
            if (_avatarPanel2 != null)
            {
                Image[] image = _avatarPanel2.GetComponentsInChildren<Image>();
                if (image != null)
                {
                    foreach (var item in image.Skip(1))
                    {
                        images.Add(item.gameObject.name);
                    }
                }
            }
        }
        private void MakeAvatarsClickble()
        {
            if(_avatarPanel1 != null)
            {
                Button[] buttons = _avatarPanel1.GetComponentsInChildren<Button>();
                if (buttons != null)
                {
                    foreach (var button in buttons)
                    {
                        button.onClick.AddListener(() => SelectAvatar(button));
                    }
                }
            }
            if (_avatarPanel2 != null)
            {
                Button[] buttons = _avatarPanel2.GetComponentsInChildren<Button>();
                if (buttons != null)
                {
                    foreach (var button in buttons)
                    {
                        button.onClick.AddListener(() => SelectAvatar(button));
                    }
                }
            }
        }

        private void SelectAvatar(Button button)
        {
            Image image = button.GetComponent<Image>();
            if(image != null && image.sprite != null)
            {
                SessionManager.SessionManager.Instance.SetAvatarName(image.sprite.name);
                Debug.Log("Avatar name: " + image.sprite.name);
            }
            else
            {
                Debug.LogError("Geen image gevonden");
            }
        }
    }
}
