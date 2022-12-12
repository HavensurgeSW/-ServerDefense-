using System.Collections.Generic;

using UnityEngine;

public class GameManager : SceneController
{
    public static bool gameStatus = false;

    [SerializeField] private CommandManager commandManager = null;
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

    private int packetScore = 0;
    private int currentWave = 0;

    protected override void Awake()
    {
        packetScore = debugScore ? 10000 : startingPackets;
        
        uiManager.Init(timerSeconds);
        uiManager.UpdatePacketPointsText(packetScore);

        commandManager.Init(terminal, levelManager, mapHandler, towersController);
        commandManager.SetCallbacks(UpdatePacketScore, GetCurrentWaveIndex, GetPacketScore, uiManager.GeneratePopUp);

        levelManager.Init(IncreaseCurrentWaveValue);

        towersController.Init();
        mapHandler.Init();
        terminal.Init(InterpretTerminalText);
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
        if (Input.GetKey(KeyCode.Escape)) {
            if (pauseHandler.IS_PAUSED)
                pauseHandler.TogglePauseStatus(false);
            else
                pauseHandler.TogglePauseStatus(true); 
        }
    }

    private void BeginCurrentWave()
    {
        levelManager.BeginWave(currentWave);
    }

    private int GetPacketScore()
    {
        return packetScore;
    }

    private int GetCurrentWaveIndex()
    {
        return currentWave;
    }

    private void IncreaseCurrentWaveValue()
    {
        currentWave++;
    }

    private void UpdatePacketScore(int i)
    {

        packetScore += i;
        uiManager.UpdatePacketPointsText(packetScore);
    }

    private void WinGame()
    {
        if (!tutorialScene)
        {
           gameStatus = true;
            ChangeScene(CommonUtils.SCENE.END_SCENE, true);
        }
    }

    private void LoseGame()
    {
        if (!tutorialScene)
        {
            gameStatus = false;
            ChangeScene(CommonUtils.SCENE.END_SCENE, true);
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
}