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
    [SerializeField] private Button optionsButton = null;
    [SerializeField] private Button creditsButton = null;
    [SerializeField] private Button exitButton = null;

    [Header("Level setup")]
    [SerializeField] private GameObject levelHolder = null;
    [SerializeField] private Button level0Button = null;
    [SerializeField] private Button level1Button = null;
    [SerializeField] private Button backButton = null;

    [Header("Audio setup")]
    [SerializeField] private GameObject optionsHolder = null;
    [SerializeField] private Button optionsBackButton = null;
    [SerializeField] private GameObject sfx_source = null;

    public void Init(Action onSwitchToCredits, Action<SCENE, bool> onChangeScene, Action onExitGame)
    {
        startGameButton.onClick.AddListener(() => {
            ToggleLevelStatus(true);
            ToggleMenuStatus(false);
            startGameButton.onClick.AddListener(() => { AkSoundEngine.PostEvent("Play_UI_Button", gameObject); });
        });
        backButton.onClick.AddListener(() => {
            ToggleMenuStatus(true);
            ToggleLevelStatus(false);
            backButton.onClick.AddListener(() => { AkSoundEngine.PostEvent("Play_UI_Button", gameObject); });
        });

        optionsButton.onClick.AddListener(() => {
            ToggleOptionsStatus(true);
            ToggleMenuStatus(false);
        });

        optionsBackButton.onClick.AddListener(() => {
            ToggleMenuStatus(true);
            ToggleOptionsStatus(false);
            optionsBackButton.onClick.AddListener(() => { AkSoundEngine.PostEvent("Play_UI_Button", gameObject); });
        });

        level0Button.onClick.AddListener(() => onChangeScene?.Invoke(SCENE.LEVEL_0, true));
        level0Button.onClick.AddListener(() => { AkSoundEngine.PostEvent("Play_UI_Button", gameObject); });
        level1Button.onClick.AddListener(() => onChangeScene?.Invoke(SCENE.LEVEL_1, true));
        level1Button.onClick.AddListener(() => { AkSoundEngine.PostEvent("Play_UI_Button", gameObject); });
        tutorialButton.onClick.AddListener(() => onChangeScene?.Invoke(SCENE.TUTORIAL, true));
        tutorialButton.onClick.AddListener(() => { AkSoundEngine.PostEvent("Play_UI_Button", gameObject); });
        creditsButton.onClick.AddListener(() => onSwitchToCredits?.Invoke());
        creditsButton.onClick.AddListener(() => { AkSoundEngine.PostEvent("Play_UI_Button", gameObject); });

        exitButton.onClick.AddListener(() => onExitGame?.Invoke());
        
    }

    public void ToggleMenuStatus(bool status)
    {
        holder.SetActive(status);
        AkSoundEngine.PostEvent("Play_UI_Button", gameObject);
    }

    public void ToggleLevelStatus(bool status)
    {
        levelHolder.SetActive(status);   
    }

    public void ToggleOptionsStatus(bool status) { 
        optionsHolder.SetActive(status);
    }
}
