using UnityEngine;

using ServerDefense.Common.Controller;
using ServerDefense.Common.Currencies;

using ServerDefense.Gameplay.Gameplay.Modules.Commands;
using ServerDefense.Gameplay.Gameplay.Modules.Enemies;
using ServerDefense.Gameplay.Gameplay.Modules.Towers;
using ServerDefense.Gameplay.Gameplay.Modules.Currencies;
using ServerDefense.Gameplay.Gameplay.Modules.Map;
using ServerDefense.Gameplay.Gameplay.Modules.Terminal;
using ServerDefense.Gameplay.Gameplay.Modules.Pause;
using ServerDefense.Gameplay.Gameplay.Modules.Waves;
using ServerDefense.Gameplay.Gameplay.Modules.Packets;
using ServerDefense.Gameplay.Gameplay.Modules.Servers;

namespace ServerDefense.Gameplay.Gameplay.Controller
{
    public class GameManager : SceneController
    {
        [Header("Game Manager Configuration")]
        [SerializeField] private CommandManager commandManager = null;
        [SerializeField] private TerminalManager terminal = null;
        [SerializeField] private TowersController towersController = null;
        [SerializeField] private MapHandler mapHandler = null;
        [SerializeField] private PauseHandler pauseHandler = null;
        [SerializeField] private EnemyHandler enemyHandler = null;
        [SerializeField] private WavesController wavesController = null;
        [SerializeField] private PacketsHandler packetsHandler = null;
        [SerializeField] private WaypointManager waypointManager = null;

        [Header("Entities Configuration")]
        [SerializeField] private Server server = null;

        [Header("Currencies Configuration")]
        [SerializeField] private GameCurrenciesController currenciesController = null;
        [SerializeField] private CurrencySO packetCurrency = null;
        [SerializeField] private int startingPackets = 0;

        [Header("Scene settings")]
        [SerializeField] private bool tutorialScene = false;

        [Header("Debugging")]
        [SerializeField] private bool debugScore = false;
        [SerializeField] private int debugValue = 10000;

        protected override void Awake()
        {
            currenciesController.Init();
            currenciesController.SetCurrencyValue(packetCurrency, debugScore ? debugValue : startingPackets);

            server.Init();

            commandManager.Init(GenerateCommandManagerModel());
            commandManager.SetCallbacks((scene) => ChangeScene(scene, false), mapHandler.ClearAllPopUps);

            enemyHandler.Init(waypointManager.WAYPOINTS);
            wavesController.Init(enemyHandler.GenerateEnemy, packetsHandler.StartPacketsWave, WinGame);

            packetsHandler.Init(waypointManager.WAYPOINTS);
            packetsHandler.UpdatePacketPointsText(currenciesController.GetCurrencyValue(packetCurrency));

            towersController.Init();
            mapHandler.Init();
            terminal.Init(commandManager.ProcessCommand);

            pauseHandler.AddOnPausedCallback((status) => terminal.ToggleTerminalInteraction(!status));
            pauseHandler.Init(() => ChangeScene(SCENE.MAIN_MENU, false));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseHandler.TogglePauseStatus(!pauseHandler.IS_PAUSED);
            }
        }

        protected override void OnEnable()
        {
            Server.OnDeath += LoseGame;
            Server.OnPacketEntry += UpdatePacketScore;
        }

        protected override void OnDisable()
        {
            Server.OnDeath -= LoseGame;
            Server.OnPacketEntry -= UpdatePacketScore;
        }

        private CommandManagerModel GenerateCommandManagerModel()
        {
            return new CommandManagerModel(commandManager, terminal, wavesController, mapHandler, towersController, currenciesController);
        }

        private void UpdatePacketScore(int value)
        {
            int newValue = currenciesController.AddCurrencyValue(packetCurrency, value);
            packetsHandler.UpdatePacketPointsText(newValue);
        }

        private void WinGame()
        {
            TriggerEndGame(true);
        }

        private void LoseGame()
        {
            TriggerEndGame(false);
        }

        private void TriggerEndGame(bool status)
        {
            if (!tutorialScene)
            {
                SavedDataHandler.Instance.SetGameWonStatus(status);
                ChangeScene(SCENE.END_SCENE, true);
            }
        }
    }
}