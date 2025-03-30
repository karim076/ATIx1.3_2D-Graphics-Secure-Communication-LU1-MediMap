using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Assets.Scripts.SessionManager;

public class EnhancedVideoPlayer : MonoBehaviour
{
    [Header("Video Settings")]
    public VideoPlayer videoPlayer;
    public RawImage videoScreen;
    
    public bool autoplay = true; // Auto-play in- of uitschakelen
    [SerializeField] private Image _avatar;

    // RenderTexture voor het weergeven van de video op de RawImage
    public RenderTexture renderTexture;

    [Header("UI Elements")]
    public Text timeText;
    public Button playButton;
    public Button stopButton;
    public Image overlayImage;
    public RectTransform overlayTransform;

    [Header("Animation Settings")]
    [SerializeField] private float buttonHoverScale = 1.05f;
    [SerializeField] private float buttonHoverDuration = 0.3f;
    [SerializeField] private float buttonClickDuration = 0.4f;
    [Space]
    [SerializeField] private float overlayFadeInDuration = 0.6f;
    [SerializeField] private float overlayFadeOutDuration = 0.8f;
    [SerializeField] private float overlayScaleDuration = 0.7f;
    [SerializeField] private Ease overlayEase = Ease.OutQuint;
    [SerializeField] private float overlayEndDelay = 0.2f;

    private Vector3 originalButtonScale;
    private bool isVideoPrepared = false;

    void Start()
    {
        // 1. Absolutely prevent any autoplay
        videoPlayer.playOnAwake = false;
        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.isLooping = false;

        /*if (SessionManager.Instance.AvatarName != null)
        {
            _avatar.GetComponent<Image>().sprite = SessionManager.Instance.AvatarName;
        }*/
        // Initialisatie van de video-player
        videoPlayer.loopPointReached += OnVideoEnd; // Event voor het einde van de video
        videoPlayer.playOnAwake = false; // Zet autoplay uit bij opstarten, tenzij expliciet aangevinkt


        // 2. Setup video output
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;
        videoScreen.texture = renderTexture;

        // 3. Initialize UI
        originalButtonScale = playButton.transform.localScale;
        InitializeButtons();
        /*InitializeOverlay();*/

        // 4. Prepare video (without playing)
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepared;
    }

    void InitializeButtons()
    {
        playButton.onClick.RemoveAllListeners();
        stopButton.onClick.RemoveAllListeners();

        playButton.onClick.AddListener(PlayVideo);
        stopButton.onClick.AddListener(StopVideo);

        playButton.interactable = true;
        stopButton.interactable = false;

        SetupButtonAnimation(playButton);
        SetupButtonAnimation(stopButton);
    }

    void InitializeOverlay()
    {
        if (overlayImage != null)
        {
            overlayImage.color = new Color(0, 0, 0, 0.5f);
            overlayImage.gameObject.SetActive(true);
            overlayTransform.localScale = Vector3.one;
            overlayImage.raycastTarget = false;
        }
    }

    void OnVideoPrepared(VideoPlayer source)
    {
        isVideoPrepared = true;
        videoPlayer.frame = 0; // Ensure we're at first frame
        videoPlayer.Pause(); // Double guarantee it's paused
    }

    public void PlayVideo()
    {
        if (!isVideoPrepared || videoPlayer.isPlaying) return;

        videoPlayer.Play();
        HideOverlay();
        playButton.interactable = false;
        stopButton.interactable = true;
    }

    public void StopVideo()
    {
        if (!videoPlayer.isPlaying) return;

        videoPlayer.Pause();
        ShowOverlay();
        playButton.interactable = true;
        stopButton.interactable = false;
    }

    void ShowOverlay()
    {
        if (overlayImage == null) return;

        overlayImage.gameObject.SetActive(true);
        overlayImage.DOKill();
        overlayTransform.DOKill();

        Sequence showSequence = DOTween.Sequence();
        showSequence.Join(overlayImage.DOFade(0.5f, overlayFadeInDuration));
        showSequence.Join(overlayTransform.DOScale(Vector3.one, overlayScaleDuration)
            .From(Vector3.zero)
            .SetEase(overlayEase));
        showSequence.Play();
    }

    void HideOverlay()
    {
        if (overlayImage == null || !overlayImage.gameObject.activeSelf) return;

        overlayImage.DOKill();
        overlayTransform.DOKill();

        Sequence hideSequence = DOTween.Sequence();
        hideSequence.Join(overlayImage.DOFade(0, overlayFadeOutDuration));
        hideSequence.Join(overlayTransform.DOScale(Vector3.zero, overlayScaleDuration)
            .SetEase(overlayEase));
        hideSequence.AppendInterval(overlayEndDelay);
        hideSequence.OnComplete(() => overlayImage.gameObject.SetActive(false));
        hideSequence.Play();
    }

    void SetupButtonAnimation(Button button)
    {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

        // Hover enter
        var pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener((e) => {
            button.transform.DOScale(originalButtonScale * buttonHoverScale, buttonHoverDuration)
                .SetEase(Ease.OutBack);
        });
        trigger.triggers.Add(pointerEnter);

        // Hover exit
        var pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((e) => {
            button.transform.DOScale(originalButtonScale, buttonHoverDuration)
                .SetEase(Ease.OutBack);
        });
        trigger.triggers.Add(pointerExit);

        // Click animation
        button.onClick.AddListener(() => {
            button.transform.DOPunchScale(Vector3.one * 0.1f, buttonClickDuration);
        });
    }

    void Update()
    {
        if (videoPlayer.isPlaying && timeText != null)
        {
            timeText.text = $"{FormatTime((float)videoPlayer.time)} / {FormatTime((float)videoPlayer.length)}";
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        StopVideo();
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return $"{minutes:00}:{seconds:00}";
    }

}