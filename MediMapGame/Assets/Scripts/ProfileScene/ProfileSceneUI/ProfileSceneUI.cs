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
        [SerializeField] private Image _lastName;

        [SerializeField] private GameObject _avatarPanel1;
        [SerializeField] private GameObject _avatarPanel2;

        [SerializeField] private Button _avatarbutton;

        [SerializeField] private GameObject _editProfilePanel;
        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private TMP_InputField _birthDayInputField;

        [SerializeField] private ProfileSceneScript ProfileSceneScript;

        private TMP_Text _userNameText;
        private TMP_Text _userBirthDayText;
        private TMP_Text _userAppointmentText;
        private TMP_Text _doctorNameText;
        private TMP_Text _treatmentPlanText;
        private TMP_Text _lastNameText;

        private List<string> images = new List<string>();

        private void Start()
        {
            InitializeTextComponent();

            CollectAvatarNames();

            MakeAvatarsClickble();
        }
        public void SetProfileData(string userName, string lastName, string userBirthDay, string userAppointment, string doctorName, string treatmentPlan)
        {
            _userNameText.text = userName;
            _userBirthDayText.text = userBirthDay;
            _userAppointmentText.text = userAppointment;
            _doctorNameText.text = doctorName;
            _treatmentPlanText.text = treatmentPlan;
            _lastNameText.text = lastName;
        }

        private void InitializeTextComponent()
        {
            _userNameText = _userName.GetComponentInChildren<TMP_Text>();
            _userBirthDayText = _userBirthDay.GetComponentInChildren<TMP_Text>();
            _userAppointmentText = _userAppointment.GetComponentInChildren<TMP_Text>();
            _doctorNameText = _doctorName.GetComponentInChildren<TMP_Text>();
            _treatmentPlanText = _treatmentPlan.GetComponentInChildren<TMP_Text>();
            _lastNameText = _lastName.GetComponentInChildren<TMP_Text>();

            if (_userNameText == null || _userBirthDayText == null || _userAppointmentText == null || _doctorNameText == null || _treatmentPlanText == null || _lastNameText == null)
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
                SessionManager.SessionManager.Instance.SetAvatarName(image.sprite);
                Debug.Log("Avatar name: " + image.name);
                _avatarbutton.GetComponent<Image>().sprite = SessionManager.SessionManager.Instance.AvatarName;

                AvatarName avatarName = new AvatarName
                {
                    avatar = image.name
                };

                ProfileSceneScript.SaveAvatar(avatarName);
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

            if (string.IsNullOrEmpty(_nameInputField.text))
            {
                Debug.Log("Naam is verplicht!");
                return;
            }

            if (!DateTime.TryParse(_birthDayInputField.text, out DateTime geboorteDatum))
            {
                Debug.Log("Voer een geldige datum in");
                return;
            }
            Patient patient = new()
            {
                Id = 0,
                VoorNaam = _nameInputField.text,
                AchterNaam = _lastNameText.text,
                AvatarNaam = string.Empty,
                ArtsNaam = _doctorNameText.text ?? string.Empty,
                TrajectNaam = _treatmentPlanText.text ?? string.Empty,
                OuderVoogdNaam = string.Empty,
                GeboorteDatum = geboorteDatum,
                Afspraakatum = DateTime.Parse(_userAppointmentText.text)
            };
            try
            {
                ProfileSceneScript.SaveProfileData(patient);
                CloseProfilePanel();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error bij het opslaan van gegevens: {e.Message}");
            }
        }
    }
}
