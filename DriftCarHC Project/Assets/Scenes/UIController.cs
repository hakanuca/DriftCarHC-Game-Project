using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] Button settingsButton;
    [SerializeField] Button settingsPanelBack;
    [SerializeField] GameObject settingsPanel;

    [SerializeField] Transform settingsButtonClosePosition;
    [SerializeField] Transform settingsButtonOpenPosition;

    [SerializeField] Transform settingsPanelOpenPosition;
    [SerializeField] Transform settingsPanelClosePosition;

    void Start()
    {
        settingsButton.onClick.AddListener(CloseSettingsButton);
        settingsPanelBack.onClick.AddListener(OpenSettingsButton);
    }

    void Update()
    {

    }

    void CloseSettingsButton()
    {
        LeanTween.move(settingsButton.gameObject, settingsButtonClosePosition.position, 0.5f)
            .setEase(LeanTweenType.easeInOutBack);
        LeanTween.move(settingsPanel, settingsPanelClosePosition.position, 0.5f)
            .setEase(LeanTweenType.easeInOutElastic);
    }

    void OpenSettingsButton()
    {
        LeanTween.move(settingsButton.gameObject, settingsButtonOpenPosition.position, 0.5f)
            .setEase(LeanTweenType.easeInOutBounce);
        LeanTween.move(settingsPanel, settingsPanelOpenPosition.position, 0.5f)
            .setEase(LeanTweenType.easeInOutSine);
    }

}