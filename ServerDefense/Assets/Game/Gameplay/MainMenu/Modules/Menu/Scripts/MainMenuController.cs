using UnityEngine;
using static CommonUtils;

public class MainMenuController : SceneController
{
    public static SCENE selectedLevel = default;

    [Header("Main Configuration")]
    [SerializeField] private MenuHandler menuHandler = null;
    [SerializeField] private CreditsHandler creditsHandler = null;
    [SerializeField] private GameObject sfx_source = null;
    [SerializeField] private GameObject loadingPanel = null;

    protected override void Awake()
    {
        menuHandler.Init(SwitchToCredits, 
            (scene, isAsync) => 
            {
                AkSoundEngine.PostEvent("Stop_AX_Menu", sfx_source);
                loadingPanel.SetActive(true);
                selectedLevel = scene;
                ChangeScene(scene, isAsync);
            }, 
            QuitApplication);

        creditsHandler.Init(SwitchToMenu);

        SwitchToMenu();
    }

    protected override void OnDisable()
    {
    }

    protected override void OnEnable()
    {
    }

    private void SwitchToMenu()
    {
        menuHandler.ToggleMenuStatus(true);
        creditsHandler.ToggleStatus(false);
    }
    private void SwitchToCredits()
    {
        menuHandler.ToggleMenuStatus(false);
        creditsHandler.ToggleStatus(true);
    }
}
