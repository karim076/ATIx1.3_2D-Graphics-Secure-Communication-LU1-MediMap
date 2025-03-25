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
    public void SaveAvatar()
    {

    }

    public void SaveProfileData(ProfileInformation profileInformation)
    {
        StartCoroutine(APIManager.Instance.SendRequest($"api/ProfileInformation/{1}", "PUT", profileInformation, (response) =>
        {
            Debug.Log(response);

            _profileSceneUI.SetProfileData(profileInformation.naam, profileInformation.geboorteDatum.ToShortDateString(), profileInformation.afspraakDatum.ToShortDateString(), profileInformation.naamDokter, profileInformation.behandelPlan);

        }, (error) =>
        {
            if (error != null)
            {
                var json = JsonConvert.DeserializeObject<ErrorMessage>(error);
                if (json != null) { Debug.LogError(json.message); }
            }
        }));
    }

    public void SetProfileData()
    {
        StartCoroutine(APIManager.Instance.SendRequest($"api/ProfileInformation/{1}", "GET", null, (response) =>
        {
            var profile = JsonConvert.DeserializeObject<ProfileInformation>(response);
            if(profile != null)
            {
                _profileSceneUI.SetProfileData(profile.naam, profile.geboorteDatum.ToShortDateString(), profile.afspraakDatum.ToShortDateString(), profile.naamDokter, profile.behandelPlan);
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
