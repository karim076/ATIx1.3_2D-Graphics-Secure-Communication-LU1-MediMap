using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using Assets.Scripts.Models;

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

        [SerializeField] private GameObject _editProfilePanel;
        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private TMP_InputField _birthDayInputField;

        [SerializeField] private ProfileSceneScript ProfileSceneScript;

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
                SessionManager.SessionManager.Instance.SetAvatarName(image.name);
                Debug.Log("Avatar name: " + image.name);
                button.GetComponent<Image>().sprite = image.sprite;
                ProfileSceneScript.SaveAvatar();
            }
            else
            {
                Debug.LogError("Geen image gevonden");
            }
        }

        public void EditProfile()
        {
            _editProfilePanel.SetActive(true);
            _nameInputField.text = _userNameText.text;
            _birthDayInputField.text = _userBirthDayText.text;
        }
        public void CloseProfilePanel()
        {
            _editProfilePanel.SetActive(false);
            _nameInputField.text = "";
            _birthDayInputField.text = "";
        }
        public void SaveProfile()
        {
            ProfileInformation profileInformation = new ProfileInformation
            {
                naam = _nameInputField.text,
                geboorteDatum = DateTime.Parse(_birthDayInputField.text),
                afspraakDatum = DateTime.Parse(_userAppointmentText.text),
                naamDokter = _doctorNameText.text,
                behandelPlan = _treatmentPlanText.text
            };
            try
            {
                ProfileSceneScript.SaveProfileData(profileInformation);
                CloseProfilePanel();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error bij het opslaan van gegevens: {e.Message}");
            }
        }
    }
}
