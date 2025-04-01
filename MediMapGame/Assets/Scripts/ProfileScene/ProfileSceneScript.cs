using Assets.Scripts.Models;
using Assets.Scripts.ProfileScene.ProfileSceneUI;
using Assets.Scripts.SessionManager;
using MediMap.Scripts.Api;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfileSceneScript : MonoBehaviour
{

    [SerializeField] private ProfileSceneUI _profileSceneUI;

    void Start()
    {
        SetProfileData();
    }
    public void SaveAvatar(AvatarName avatarNaam)
    {
        StartCoroutine(APIManager.Instance.SendRequest($"api/Patient/avatar/{SessionManager.Instance.PatientId}", "PUT", avatarNaam, (response) =>
        {
            Debug.Log(response);
            var json = JsonConvert.DeserializeObject<AvatarName>(response);

            if (json != null)
            {
                Sprite sprite = Resources.Load<Sprite>($"Art/{json.avatar}");
                SessionManager.Instance.SetAvatarName(sprite);
            }
        }, (error) =>
        {
            if (error != null)
            {
                Debug.Log(error);
            }
        }));
    }

    public void SaveProfileData(Patient patient)
    {
        StartCoroutine(APIManager.Instance.SendRequest($"api/Patient/{SessionManager.Instance.PatientId}", "PUT", patient, (response) =>
        {
            Debug.Log(response);
            var profile = JsonConvert.DeserializeObject<Patient>(response);

            _profileSceneUI.SetProfileData(profile.VoorNaam, profile.AchterNaam, profile.GeboorteDatum.ToShortDateString(), profile.AfspraakDatum.ToShortDateString(), profile.ArtsNaam, profile.TrajectNaam);

        }, (error) =>
        {
            if (error != null)
            {
                Debug.Log(error);
            }
        }));
    }

    public void SetProfileData()
    {
        StartCoroutine(APIManager.Instance.SendRequest($"api/Patient/{SessionManager.Instance.PatientId}", "GET", null, (response) =>
        {
            var profile = JsonConvert.DeserializeObject<Patient>(response);
            if(profile != null)
            {
                _profileSceneUI.SetProfileData(profile.VoorNaam, profile.AchterNaam, profile.GeboorteDatum.ToShortDateString(), profile.AfspraakDatum.ToShortDateString(), profile.ArtsNaam, profile.TrajectNaam);
            }
        }));
    }
    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("HomeScreenScene", LoadSceneMode.Single);
    }
    public void OnInfoSceneClicked()
    {
        SceneManager.LoadSceneAsync("InfoScene", LoadSceneMode.Single);
    }
    public void OnExitSceneClicked()
    {
        SceneManager.LoadSceneAsync("LoginScene", LoadSceneMode.Single);
    }
}
