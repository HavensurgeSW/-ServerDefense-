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

    [Header("Debugging")]
    [SerializeField] private bool debugScore = false;
    [SerializeField] private int startingPackets;

    private int packetScore = 0;
    private int currentWave = 0;

    private void Awake()
    {
        packetScore = debugScore ? 10000 : startingPackets;
        uiManager.UpdatePacketPointsText(packetScore);

        Server.OnDeath += LoseGame;
        Server.OnPacketEntry += UpdatePacketScore;

        commandManager.Init(uiManager, terminal, levelManager, mapHandler, towersController);
        commandManager.SetCallbacks(UpdatePacketScore, GetCurrentWaveIndex, GetPacketScore);

        levelManager.Init(IncreaseCurrentWaveValue);
        uiManager.Init(timerSeconds);

        uiManager.AddOnTimerEndCallback(() => levelManager.BeginWave(currentWave));

        towersController.Init();
        mapHandler.Init();
        terminal.Init(InterpretTerminalText);
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

    private void OnDisable()
    {
        Server.OnDeath -= LoseGame;
        Server.OnPacketEntry -= UpdatePacketScore;
    }

    private void LoseGame()
    {
        SceneManager.LoadScene(1);
    }

    private void InterpretTerminalText(string text)
    {
        text = text.ToLower();
        string[] arguments = text.Split(' ');
        bool searchHit = false;

        foreach (Command cmd in commandManager.COMMANDS)
        {
            if (cmd.INFO.ID == arguments[0])
            {
                searchHit = true;
                commandManager.ProcessCommand(cmd, arguments);
                break;
            }
        }

        if (!searchHit)
        {
            terminal.AddInterpreterLines(new List<string> { "Command not recognized. Type HELP for a list of commands" });
        }
    }
}