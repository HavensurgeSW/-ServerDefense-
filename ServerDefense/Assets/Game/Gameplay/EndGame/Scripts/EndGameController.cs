using UnityEngine;
using UnityEngine.UI;

using ServerDefense.Common.Controller;

namespace ServerDefense.Gameplay.EndGame
{
    public class EndGameController : SceneController
    {
        [SerializeField] private GameStatusPanel gameStatusPanel = null;
        [SerializeField] private Button replayButton = null;
        [SerializeField] private Button mainMenuButton = null;

        protected override void Awake()
        {
            SavedDataHandler savedDataHandler = SavedDataHandler.Instance;

            gameStatusPanel.Init(savedDataHandler.GameStatus);

            replayButton.onClick.AddListener(() => ChangeScene(savedDataHandler.SelectedScene, true));
            mainMenuButton.onClick.AddListener(() => ChangeScene(SCENE.MAIN_MENU, true));
        }

        protected override void OnDisable()
        {
        }

        protected override void OnEnable()
        {
        }

        protected override void ChangeScene(SCENE scene, bool async)
        {
            if (scene == SCENE.NONE)
            {
                scene = SCENE.LEVEL_0;
            }

            base.ChangeScene(scene, async);
        }
    }
}