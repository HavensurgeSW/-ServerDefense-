using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public event Action<string> OnChangeDirectory;
    public event Action<string> OnInstallTower;
    public event Action OnHelpArgument;
    //public event Action OnNetworkInit;
    public event Action<string> OnUpdateTower;
    //public event Action OnHelpCommand;

    [SerializeField] private CommandManager commandManager = null;
    [SerializeField] private UIManager uiManager = null;
    [SerializeField] private TerminalManager terminal = null;
    [SerializeField] private TowersController towersController = null;
    [SerializeField] private LevelManager levelManager = null;
    [SerializeField] private MapHandler mapHandler = null;

    [Header("Gameplay Values")]
    [SerializeField] private float timerSeconds = 0;
    [SerializeField] private int startingPackets = 0;

    [Header("Debugging")]
    [SerializeField] private bool debugScore = false;

    private int packetScore = 0;
    private int currentWave = 0;

    private void Awake()
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

    private void OnEnable()
    {
        Server.OnDeath += LoseGame;
        Server.OnPacketEntry += UpdatePacketScore;

        uiManager.AddOnTimerEndCallback(BeginCurrentWave);
    }

    private void OnDisable()
    {
        Server.OnDeath -= LoseGame;
        Server.OnPacketEntry -= UpdatePacketScore;

        uiManager.RemoveOnTimerEndCallback(BeginCurrentWave);
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

    private void LoseGame()
    {
        SceneManager.LoadScene(1);
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