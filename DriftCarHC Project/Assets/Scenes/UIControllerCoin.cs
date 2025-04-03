using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIControllerCoin : MonoBehaviour
{
    [SerializeField] Button coinButton;
    [SerializeField] Button coinPanelBack;
    [SerializeField] GameObject coinPanel;

    [SerializeField] Transform coinButtonClosePosition;
    [SerializeField] Transform coinButtonOpenPosition;

    [SerializeField] Transform coinPanelOpenPosition;
    [SerializeField] Transform coinPanelClosePosition;

    void Start()
    {
        coinButton.onClick.AddListener(CloseSettingsButton);
        coinPanelBack.onClick.AddListener(OpenSettingsButton);
    }

    void Update()
    {

    }

    void CloseSettingsButton()
    {
        LeanTween.move(coinButton.gameObject, coinButtonClosePosition.position, 1f)
            .setEase(LeanTweenType.easeInOutBack);
        LeanTween.move(coinPanel, coinPanelClosePosition.position, 1f)
            .setEase(LeanTweenType.easeInOutSine);
    }

    void OpenSettingsButton()
    {
        LeanTween.move(coinButton.gameObject, coinButtonOpenPosition.position, 1f)
            .setEase(LeanTweenType.easeInOutBack);
        LeanTween.move(coinPanel, coinPanelOpenPosition.position, 1f)
            .setEase(LeanTweenType.easeInOutSine);
    }

}