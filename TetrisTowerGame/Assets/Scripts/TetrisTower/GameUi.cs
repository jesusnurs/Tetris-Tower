using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameUi : MonoBehaviour
{
    [SerializeField] private TetrisObjectController tetrisObjectController;
    [SerializeField] private StateMachine stateMachine;

    [Space]
    [SerializeField] private RewardedAds rewardedAds;
    [SerializeField] private InterstitialAds interstitialAds;

    [Space]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button rotateTetrisButton;
    [SerializeField] private TextMeshProUGUI currentDistanceText;
    [SerializeField] private TextMeshProUGUI bestDistanceText;

    [Header("GameOverUI")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button doubleCoinsButton;
    [SerializeField] private Button giveUpButton;
    
    [Space] 
    [SerializeField] private Image logoImage;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameUiPanel;
    
    [Space]
    [SerializeField] private RectTransform settingsPanel;
    [SerializeField] private Vector2 settingsFromToPos;
    [SerializeField] private float animationDuration = 0.5f;

    private int bestDistance;
    private int currentDistance;
    private bool isSettingsVisible;
    private Image playButtonImage;

    private void Awake()
    {
        stateMachine.OnGameStarted += OnStartGame;
        stateMachine.OnGameEnded += OnGameOver;
        stateMachine.OnMainMenuOpened += OnMainMenu;

        rewardedAds.OnRewardedAdsShowed += Continue;

        settingsButton.onClick.AddListener(ToggleMenu);
        playButton.onClick.AddListener(stateMachine.StartGame);
        rotateTetrisButton.onClick.AddListener(tetrisObjectController.RotateTetrisObject);
        
        continueButton.onClick.AddListener(WatchAdToContinue);
        doubleCoinsButton.onClick.AddListener(WatchAdToDoubleCoins);
        giveUpButton.onClick.AddListener(GiveUp);
        
        playButtonImage = playButton.GetComponent<Image>();
        
        bestDistance = PlayerPrefs.GetInt("BestDistance", 0);
        bestDistanceText.text = bestDistance + "m";
        currentDistanceText.text = 0 + "m";
    }
    
    private void ToggleMenu()
    {
        RotateGear();
        
        float endValue = isSettingsVisible ? settingsFromToPos.x : settingsFromToPos.y;
        isSettingsVisible = !isSettingsVisible;

        if (isSettingsVisible)
        {
            stateMachine.PauseGame();
            //mainCamera.cullingMask = ~(1 << LayerMask.NameToLayer("TetrisObjects"));
        }
        else
        {
            stateMachine.ResumeGame();
            //mainCamera.cullingMask |= 1 << LayerMask.NameToLayer("TetrisObjects");
        }
        
        settingsPanel.DOAnchorPosX(endValue, animationDuration).SetEase(Ease.InOutQuad);
    }
    
    private void RotateGear()
    {
        settingsButton.transform.DORotate(new Vector3(0, 0, isSettingsVisible ? -360 : 360), animationDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutQuad);
    }

    private void OnStartGame()
    {
        gameOverUI.SetActive(false);
        gameUiPanel.SetActive(true);
        
        rotateTetrisButton.gameObject.SetActive(true);
        settingsButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
        
        currentDistance = 0;
        currentDistanceText.text = currentDistance + "m";
        
        logoImage.DOFade(0,0.5f).From(1).SetEase(Ease.InOutQuad).OnComplete( () => logoImage.gameObject.SetActive(false));
    }

    private void OnMainMenu()
    {
        gameOverUI.SetActive(false);
        gameUiPanel.SetActive(false);
        
        rotateTetrisButton.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(false);

        var color = logoImage.color;
        color.a = 0;
        logoImage.color = color;
        playButtonImage.color = color;
        logoImage.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        logoImage.DOFade(1,1f).SetEase(Ease.InOutQuad);
        playButtonImage.DOFade(1,1f).SetEase(Ease.InOutQuad);
    }

    private void OnGameOver()
    {
        interstitialAds.ShowInterstitialAd();
        gameOverUI.SetActive(true);
    }

    private void GiveUp()
    {
        stateMachine.OpenMainMenu();
    }

    private void WatchAdToContinue()
    {
        rewardedAds.ShowRewardedAd();
    }

    private void Continue()
    {
        gameOverUI.SetActive(false);
        stateMachine.RestartGame();
    }
    
    private void WatchAdToDoubleCoins()
    {
        //TODO Fara magic things
    }
    
    public void UpdateDistanceText(int distance)
    {
        if(currentDistance >= distance)
            return;
        
        currentDistance = distance;
        currentDistanceText.text = currentDistance + "m";

        if (bestDistance < currentDistance)
        {
            PlayerPrefs.SetInt("BestDistance", currentDistance);
            bestDistance = currentDistance;
            bestDistanceText.text = bestDistance + "m";
        }
    }
}