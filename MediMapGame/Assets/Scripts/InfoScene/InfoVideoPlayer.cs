using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using Assets.Scripts.SessionManager;

public class InfoVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RawImage videoScreen; // Waar de video wordt weergegeven
    public bool autoplay = true; // Auto-play in- of uitschakelen
    private bool isPaused = false;

    // Dynamisch gegenereerde UI-componenten
    private Slider timeSlider; // Tijdlijn-slider voor vooruitspoelen
    private Text timeText; // Tekst die de huidige tijd van de video weergeeft

    [SerializeField] private Image _avatar;

    // RenderTexture voor het weergeven van de video op de RawImage
    public RenderTexture renderTexture;

    void Start()
    {
        if (SessionManager.Instance.AvatarName != null)
        {
            _avatar.GetComponent<Image>().sprite = SessionManager.Instance.AvatarName;
        }
        // Initialisatie van de video-player
        videoPlayer.loopPointReached += OnVideoEnd; // Event voor het einde van de video
        videoPlayer.playOnAwake = false; // Zet autoplay uit bij opstarten, tenzij expliciet aangevinkt

        // Zet video output naar RawImage voor weergave met de RenderTexture
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;
        videoScreen.texture = renderTexture;

        // Dynamisch de UI-elementen aanmaken (Slider en Text)
        CreateUI();

        if (autoplay)
        {
            PlayVideo(); // Start de video automatisch als autoplay is ingeschakeld
        }
    }

    void Update()
    {
        // Update tijdlijn-slider en de tijdsweergave
        if (videoPlayer.isPlaying && videoPlayer.length > 0)
        {
            float currentTime = (float)videoPlayer.time;
            timeSlider.value = (float)(videoPlayer.time / videoPlayer.length);
            timeText.text = FormatTime(currentTime);
        }
    }

    // Speel de video af
    public void PlayVideo()
    {
        if (!videoPlayer.isPlaying)
        {
            videoPlayer.Play();
            isPaused = false;
        }
    }

    // Pauzeer de video
    public void PauseVideo()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
            isPaused = true;
        }
    }

    // Stop de video
    public void StopVideo()
    {
        videoPlayer.Stop();
        isPaused = false;
    }

    // Skip forward door de video (bijvoorbeeld 10 seconden)
    public void SkipForward(float seconds)
    {
        videoPlayer.time += seconds;
    }

    // Rewind de video (bijvoorbeeld 10 seconden)
    public void Rewind(float seconds)
    {
        videoPlayer.time -= seconds;
    }

    // TimeSlider wijziging
    private void OnSliderValueChanged(float value)
    {
        if (videoPlayer.isPrepared)
        {
            videoPlayer.time = value * videoPlayer.length;
        }
    }

    // Format tijd als MM:SS
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Event wanneer de video is afgelopen
    private void OnVideoEnd(VideoPlayer vp)
    {
        // De video is afgelopen, eventueel weer afspelen of iets anders doen
        if (autoplay)
        {
            videoPlayer.time = 0; // Reset naar het begin
            videoPlayer.Play(); // Start opnieuw
        }
    }

    // Dynamisch de UI-elementen aanmaken (Slider en Text)
    private void CreateUI()
    {
        // Controleer of er een Canvas bestaat in de scène
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            // Maak een nieuw Canvas aan als er geen bestaat
            GameObject canvasObject = new GameObject("Canvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
        }

        // Maak een Slider (tijdlijn)
        GameObject sliderObject = new GameObject("TimeSlider");
        sliderObject.transform.SetParent(canvas.transform); // Zet de slider als child van het Canvas
        timeSlider = sliderObject.AddComponent<Slider>(); // Voeg een Slider-component toe

        // Slider instellingen
        RectTransform rtSlider = sliderObject.GetComponent<RectTransform>();
        rtSlider.sizeDelta = new Vector2(600, 30); // Zet de grootte van de slider
        rtSlider.anchoredPosition = new Vector2(0, -50); // Zet de positie op het scherm (aanpassen naar wens)

        // Voeg een listener toe voor wanneer de slider verandert
        timeSlider.onValueChanged.AddListener(OnSliderValueChanged);

        // Maak een Text element voor de tijd
        GameObject timeTextObject = new GameObject("TimeText");
        timeTextObject.transform.SetParent(canvas.transform); // Voeg deze Text als child toe aan het Canvas
        timeText = timeTextObject.AddComponent<Text>(); // Voeg een Text-component toe

        // Text instellingen
        RectTransform rtText = timeTextObject.GetComponent<RectTransform>();
        rtText.sizeDelta = new Vector2(200, 50); // Zet de grootte van de tekst
        rtText.anchoredPosition = new Vector2(0, -100); // Zet de positie van de tekst (aanpassen naar wens)

        timeText.fontSize = 24;
        timeText.color = Color.white; // Zet de tekstkleur (wit)

        // Zorg ervoor dat de Text een font heeft
        timeText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
    }
}