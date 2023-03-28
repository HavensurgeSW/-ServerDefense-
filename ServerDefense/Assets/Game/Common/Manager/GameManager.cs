using UnityEngine;

using ServerDefense.Systems.Currencies;

public class GameManager : SceneController
{
    public static bool gameStatus = false;

    [SerializeField] private CommandManager commandManager = null;
    [SerializeField] private CurrenciesController currenciesController = null;
    [SerializeField] private UIManager uiManager = null;
    [SerializeField] private TerminalManager terminal = null;
    [SerializeField] private TowersController towersController = null;
    [SerializeField] private LevelManager levelManager = null;
    [SerializeField] private MapHandler mapHandler = null;
    [SerializeField] private PauseHandler pauseHandler = null;

    [Header("Gameplay Values")]
    [SerializeField] private float timerSeconds = 0;
    [SerializeField] private int startingPackets = 0;

    [Header("Scene settings")]
    [SerializeField] private bool tutorialScene;

    [Header("Debugging")]
    [SerializeField] private bool debugScore = false;
    [SerializeField] private int debugValue = 10000;

    private Camera mainCamera = null;

    protected override void Awake()
    {
        mainCamera = Camera.main;

        currenciesController.Init();
        currenciesController.SetCurrencyValue(CurrencyConstants.packetCurrency, debugScore ? debugValue : startingPackets);

        uiManager.Init(timerSeconds);
        uiManager.UpdatePacketPointsText(currenciesController.GetCurrencyValue(CurrencyConstants.packetCurrency));

        commandManager.Init(GenerateCommandManagerModel());
        commandManager.SetCallbacks((scene) => ChangeScene(scene, false));

        levelManager.Init(null);

        towersController.Init();
        mapHandler.Init();
        terminal.Init(InterpretTerminalText);

        pauseHandler.AddOnPausedCallback((status) => terminal.ToggleTerminalInteraction(!status));
        pauseHandler.Init(() => ChangeScene(SCENE.MAIN_MENU, false));
    }

    protected override void OnEnable()
    {
        LevelManager.OnAllWavesCompleted += WinGame;
        Server.OnDeath += LoseGame;
        Server.OnPacketEntry += UpdatePacketScore;

        uiManager.AddOnTimerEndCallback(BeginCurrentWave);
    }

    protected override void OnDisable()
    {
        LevelManager.OnAllWavesCompleted -= WinGame;
        Server.OnDeath -= LoseGame;
        Server.OnPacketEntry -= UpdatePacketScore;

        uiManager.RemoveOnTimerEndCallback(BeginCurrentWave);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseHandler.TogglePauseStatus(!pauseHandler.IS_PAUSED);
        }
    }

    private CommandManagerModel GenerateCommandManagerModel()
    {
        return new CommandManagerModel(commandManager, terminal, levelManager, mapHandler, towersController, currenciesController, uiManager, mainCamera);
    }

    private void BeginCurrentWave()
    {
        levelManager.BeginWave(levelManager.GetCurrentWaveIndex());
    }

    private void UpdatePacketScore(int value)
    {
        currenciesController.AddCurrencyValue(CurrencyConstants.packetCurrency, value);
        uiManager.UpdatePacketPointsText(currenciesController.GetCurrencyValue(CurrencyConstants.packetCurrency));
    }

    private void WinGame()
    {
        if (!tutorialScene)
        {
            gameStatus = true;
            ChangeScene(SCENE.END_SCENE, true);
        }
    }

    private void LoseGame()
    {
        if (!tutorialScene)
        {
            gameStatus = false;
            ChangeScene(SCENE.END_SCENE, true);
        }
    }

    private void InterpretTerminalText(string text)
    {
        text = text.ToLower();
        string[] arguments = text.Split(' ');
        string commandId = arguments[0];

        CommandSO command = commandManager.GetCommand(commandId);

        if (command == null)
        {
            terminal.AddInterpreterLines(commandManager.GetInvalidCommandResponse());
            return;
        }

        uiManager.ClearAllPopUps();
        commandManager.ProcessCommand(command, arguments);
    }
}