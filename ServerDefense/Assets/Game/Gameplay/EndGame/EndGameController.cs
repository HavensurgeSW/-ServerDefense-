using UnityEngine;
using UnityEngine.UI;

public class EndGameController : SceneController
{
    [SerializeField] private GameStatusPanel gameStatusPanel = null;
    [SerializeField] private Button replayButton = null;
    [SerializeField] private Button mainMenuButton = null;

    protected override void Awake()
    {
        gameStatusPanel.Init(GameManager.gameStatus);

        //replayButton.onClick.AddListener(() => ChangeScene(CommonUtils.SCENE.TOWER_DEFENSE, true));
        mainMenuButton.onClick.AddListener(() => ChangeScene(CommonUtils.SCENE.MAIN_MENU, true));
    }

    protected override void OnDisable()
    {
    }

    protected override void OnEnable()
    {
    }
}
