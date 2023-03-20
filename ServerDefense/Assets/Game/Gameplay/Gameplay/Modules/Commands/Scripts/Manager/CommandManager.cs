using System;
using System.Collections.Generic;

using UnityEngine;

using ServerDefense.Systems.Currencies;

public class CommandManager : MonoBehaviour
{
    // TEMPORARY
    public event Action<string> OnChangeDirectory = null;
    public event Action<string> OnInstallTower = null;
    public event Action OnHelpArgument = null;
    public event Action<string> OnUpdateTower = null;
    //public event Action OnNetworkInit;
    //public event Action OnHelpCommand;


    [Header("Main Configuration")]
    [SerializeField] private List<CommandSO> newCommands = new List<CommandSO>();
    
    [Header("Helper commands Configuration")]
    [SerializeField] private HelpCommandSO helpCommand = null;

    private Camera mainCamera = null;

    private TerminalManager terminal = null;
    private MapHandler mapHandler = null;
    private LevelManager levelManager = null;
    private TowersController towersController = null;
    private CurrenciesController currencyController = null;
    private UIManager uiManager = null;

    public Action<SCENE> OnChangeScene = null;

    public void Init(TerminalManager terminal, LevelManager levelManager, MapHandler mapHandler, TowersController towersController, CurrenciesController currencyController, UIManager uiManager)
    {
        mainCamera = Camera.main;

        this.terminal = terminal;
        this.levelManager = levelManager;
        this.mapHandler = mapHandler;
        this.towersController = towersController;
        this.currencyController = currencyController;
        this.uiManager = uiManager;
    }

    public Camera GetMainCamera()
    {
        return mainCamera;
    }

    public TerminalManager GetTerminalManager()
    {
        return terminal;
    }

    public LevelManager GetLevelManager()
    {
        return levelManager;
    }

    public MapHandler GetMapHandler()
    {
        return mapHandler;
    }

    public TowersController GetTowersController()
    {
        return towersController;
    }

    public UIManager GetUIManager()
    {
        return uiManager;
    }

    public CurrenciesController GetCurrencyController()
    {
        return currencyController;
    }

    public void SetCallbacks(Action<SCENE> onChangeScene)
    {
        OnChangeScene = onChangeScene;
    }

    public void ChangeScene(SCENE scene)
    {
        OnChangeScene?.Invoke(scene);
    }

    public void ProcessCommand(CommandSO command, string[] fullArguments)
    {
        string[] commandArgs = new string[fullArguments.Length - 1];

        for (int i = 1; i <= commandArgs.Length; i++)
        {
            commandArgs[i - 1] = fullArguments[i];
        }

        if (CheckForHelpCommand(commandArgs))
        {
            ShowTerminalLines(command.HELP_RESPONSE);
            OnHelpArgument?.Invoke();
            return;
        }

        if (!CheckCommandArguments(commandArgs, command))
        {
            ShowTerminalLines(new List<string> { "Invalid argument amount" });
            return;
        }

        command.TriggerCommand(this, commandArgs, ShowTerminalLines, ShowTerminalLines);
    }

    public CommandSO GetCommand(string id)
    {
        foreach (CommandSO cmd in newCommands)
        {
            if (cmd.COMMAND_ID == id)
            {
                return cmd;
            }
        }

        return null;
    }

    public List<CommandSO> GetCommands()
    {
        return newCommands;
    }

    private bool CheckCommandArguments(string[] args, CommandSO command)
    {
        if (args != null)
        {
            if (args.Length == command.ARGUMENTS_COUNT)
            {
                return true;
            }
        }

        if (args == null && command.ARGUMENTS_COUNT == 0)
        {
            return true;
        }

        return false;
    }

    private bool CheckForHelpCommand(string[] args)
    {
        if (args != null && args.Length == 1)
        {
            for (int i = 0; i < helpCommand.HELP_KEYWORDS.Length; i++)
            {
                if (args[0] == helpCommand.HELP_KEYWORDS[i])
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void ShowTerminalLines(List<string> lines)
    {
        terminal.AddInterpreterLines(lines);
    }
}