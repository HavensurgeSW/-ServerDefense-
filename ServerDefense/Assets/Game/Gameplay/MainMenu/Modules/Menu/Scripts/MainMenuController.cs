using UnityEngine;

public class MainMenuController : SceneController
{
    [Header("Main Configuration")]
    [SerializeField] private MenuHandler menuHandler = null;
    [SerializeField] private CreditsHandler creditsHandler = null;
    [SerializeField] private GameObject loadingPanel = null;

    protected override void Awake()
    {
        menuHandler.Init(SwitchToCredits, 
            (scene, isAsync) => 
            {
                loadingPanel.SetActive(true);
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
