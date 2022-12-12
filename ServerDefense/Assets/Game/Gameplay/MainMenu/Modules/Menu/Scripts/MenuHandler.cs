using System;

using UnityEngine;
using UnityEngine.UI;

using static CommonUtils;

public class MenuHandler : MonoBehaviour
{
    [Header("Main Configuration")]
    [SerializeField] private GameObject holder = null;
    [SerializeField] private Button startGameButton = null;
   
    [SerializeField] private Button tutorialButton = null;
    [SerializeField] private Button creditsButton = null;
    [SerializeField] private Button exitButton = null;

    [Header("Level setup")]
    [SerializeField] private GameObject levelHolder = null;
    [SerializeField] private Button level0Button = null;
    [SerializeField] private Button level1Button = null;
    [SerializeField] private Button backButton = null;

    public void Init(Action onSwitchToCredits, Action<SCENE, bool> onChangeScene, Action onExitGame)
    {
        startGameButton.onClick.AddListener(() => {
            ToggleLevelStatus(true);
            ToggleMenuStatus(false);
        });
        backButton.onClick.AddListener(() => {
            ToggleMenuStatus(true);
            ToggleLevelStatus(false);
        });
        level0Button.onClick.AddListener(() => onChangeScene?.Invoke(SCENE.LEVEL_0, true));
        level1Button.onClick.AddListener(() => onChangeScene?.Invoke(SCENE.LEVEL_1, true));
        tutorialButton.onClick.AddListener(() => onChangeScene?.Invoke(SCENE.TUTORIAL, true));
        creditsButton.onClick.AddListener(() => onSwitchToCredits?.Invoke());

        exitButton.onClick.AddListener(() => onExitGame?.Invoke());
    }

    public void ToggleMenuStatus(bool status)
    {
        holder.SetActive(status);
    }

    public void ToggleLevelStatus(bool status)
    {
        levelHolder.SetActive(status);
    }
}
