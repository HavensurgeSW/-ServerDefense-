using UnityEngine;

public class MainMenuController : SceneController
{
    [Header("Main Configuration")]
    [SerializeField] private MenuHandler menuHandler = null;
    [SerializeField] private CreditsHandler creditsHandler = null;

    protected override void Awake()
    {
        menuHandler.Init(SwitchToCredits, ChangeScene, QuitApplication);
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
        menuHandler.ToggleStatus(true);
        creditsHandler.ToggleStatus(false);
    }

    private void SwitchToCredits()
    {
        menuHandler.ToggleStatus(false);
        creditsHandler.ToggleStatus(true);
    }
}
