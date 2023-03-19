using System.Collections.Generic;

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

    private int currentWave = 0;

    protected override void Awake()
    {
        currenciesController.Init();
        currenciesController.SetCurrencyValue(CurrencyConstants.packetCurrency, debugScore ? 10000 : startingPackets);

        uiManager.Init(timerSeconds);
        uiManager.UpdatePacketPointsText(currenciesController.GetCurrencyValue(CurrencyConstants.packetCurrency));

        commandManager.Init(terminal, levelManager, mapHandler, towersController, currenciesController, uiManager);
        commandManager.SetCallbacks(GetCurrentWaveIndex, uiManager.GeneratePopUp);

        levelManager.Init(IncreaseCurrentWaveValue);

        towersController.Init();
        mapHandler.Init();
        terminal.Init(NewInterpretTerminalText);

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

    private void BeginCurrentWave()
    {
        levelManager.BeginWave(currentWave);
    }

    private int GetCurrentWaveIndex()
    {
        return currentWave;
    }

    private void IncreaseCurrentWaveValue()
    {
        currentWave++;
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

        Command command = commandManager.GetCommand(commandId);

        if (command == null)
        {
            terminal.AddInterpreterLines(new List<string> { "Command not recognized. Type HELP for a list of commands" });
            return;
        }

        uiManager.ClearAllPopUps();
        commandManager.ProcessCommand(command, arguments);
    }

    private void NewInterpretTerminalText(string text)
    {
        text = text.ToLower();
        string[] arguments = text.Split(' ');
        string commandId = arguments[0];

        CommandSO command = commandManager.GetNewCommand(commandId);

        if (command == null)
        {
            terminal.AddInterpreterLines(new List<string> { "Command not recognized. Type HELP for a list of commands" });
            InterpretTerminalText(text);
            return;
        }

        uiManager.ClearAllPopUps();
        commandManager.NewProcessCommand(command, arguments);
    }
}