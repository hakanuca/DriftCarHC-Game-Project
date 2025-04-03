using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Settings UI")]
    [SerializeField] Button settingsButton;
    [SerializeField] Button settingsPanelBack;
    [SerializeField] GameObject settingsPanel;

    [SerializeField] Transform settingsButtonClosePosition;
    [SerializeField] Transform settingsButtonOpenPosition;

    [SerializeField] Transform settingsPanelClosePosition;
    [SerializeField] Transform settingsPanelOpenPosition;

    [Header("Coin UI")]
    [SerializeField] Button coinButton;
    [SerializeField] Button coinPanelBack;
    [SerializeField] GameObject coinPanel;

    [SerializeField] Transform coinButtonClosePosition;
    [SerializeField] Transform coinButtonOpenPosition;

    [SerializeField] Transform coinPanelClosePosition;
    [SerializeField] Transform coinPanelOpenPosition;

    private bool isSettingsOpen = false;
    private bool isCoinOpen = false;

    void Start()
    {
        settingsButton.onClick.AddListener(OpenSettings);
        settingsPanelBack.onClick.AddListener(CloseSettings);

        coinButton.onClick.AddListener(OpenCoin);
        coinPanelBack.onClick.AddListener(CloseCoin);
    }

    void OpenSettings()
    {
        if (isCoinOpen)
        {
            CloseCoinPanel();
        }
        
        OpenSettingsPanel();
    }

    void CloseSettings()
    {
       CloseSettingsPanel();
    }

    void OpenCoin()
    {
        if (isSettingsOpen)
        {
            CloseSettingsPanel();
        }

        OpenCoinPanel();                

    }

    void CloseCoin()
    {
        CloseCoinPanel();
    }

    void OpenSettingsPanel()
    {
        LeanTween.move(settingsButton.gameObject, settingsButtonClosePosition.position, 1f)
            .setEase(LeanTweenType.easeInOutBack);
        LeanTween.move(settingsPanel, settingsPanelOpenPosition.position, 1f)
            .setEase(LeanTweenType.easeInOutSine);

        isSettingsOpen = true;
    }

    void CloseSettingsPanel()
    {
        LeanTween.move(settingsButton.gameObject, settingsButtonOpenPosition.position, 1f)
            .setEase(LeanTweenType.easeInOutBack);
        LeanTween.move(settingsPanel, settingsPanelClosePosition.position, 1f)
            .setEase(LeanTweenType.easeInOutSine);

        isSettingsOpen = false;
    }

    void OpenCoinPanel()
    {
        LeanTween.move(coinButton.gameObject, coinButtonClosePosition.position, 1f)
            .setEase(LeanTweenType.easeInOutBack);
        LeanTween.move(coinPanel, coinPanelOpenPosition.position, 1f)
            .setEase(LeanTweenType.easeInOutSine);

        isCoinOpen = true;
    }

    void CloseCoinPanel()
    {
        LeanTween.move(coinButton.gameObject, coinButtonOpenPosition.position, 1f)
            .setEase(LeanTweenType.easeInOutBack);
        LeanTween.move(coinPanel, coinPanelClosePosition.position, 1f)
            .setEase(LeanTweenType.easeInOutSine);

        isCoinOpen = false;
    }
}
