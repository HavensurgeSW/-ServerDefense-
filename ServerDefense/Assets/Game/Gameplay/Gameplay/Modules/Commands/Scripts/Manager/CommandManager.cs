using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private List<Command> commands = new List<Command>();
    [SerializeField] private List<CommandSO> newCommands = new List<CommandSO>();
    
    [Header("Helper commands Configuration")]
    [SerializeField] private HelpCommandInfo helpInfo = null;

    private Camera mainCamera = null;

    private TerminalManager terminal = null;
    private MapHandler mapHandler = null;
    private LevelManager levelManager = null;
    private TowersController towersController = null;
    private CurrenciesController currencyController = null;
    private UIManager uiManager = null;

    public Action<string, Vector3> OnGeneratePopup = null;
    public Func<int> OnGetCurrentWaveIndex = null;

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

    public void SetCallbacks(Func<int> onGetCurrentWaveIndex, Action<string, Vector3> onGeneratePopup)
    {
        OnGetCurrentWaveIndex = onGetCurrentWaveIndex;
        OnGeneratePopup = onGeneratePopup;
    }

    public void NewProcessCommand(CommandSO command, string[] fullArguments)
    {
        string[] commandArgs = new string[fullArguments.Length - 1];

        for (int i = 1; i <= commandArgs.Length; i++)
        {
            commandArgs[i - 1] = fullArguments[i];
        }

        if (CheckHelpCommand(commandArgs))
        {
            ShowTerminalLines(command.HELP_RESPONSE);
            OnHelpArgument?.Invoke();
            return;
        }

        if (!CheckNewCommandArguments(commandArgs, command))
        {
            ShowTerminalLines(new List<string> { "Invalid argument amount" });
            return;
        }

        command.TriggerCommand(this, commandArgs, ShowTerminalLines, ShowTerminalLines);
    }

    public void ProcessCommand(Command command, string[] fullArguments)
    {
        string[] commandArg = new string[fullArguments.Length - 1];

        for (int i = 1; i <= commandArg.Length; i++)
        {
            commandArg[i - 1] = fullArguments[i];
        }

        if (CheckHelpCommand(commandArg))
        {
            TriggerHelpResponse(command.INFO);
            OnHelpArgument?.Invoke();
            return;
        }

        if (!CheckCommandArguments(commandArg, command.INFO))
        {
            ShowTerminalLines(new List<string> { "Invalid argument amount" });
            return;
        }

        command.CALLBACK?.Invoke(commandArg, command.INFO);
    }

    public Command GetCommand(string id)
    {
        foreach (Command cmd in commands)
        {
            if (cmd.INFO.ID == id)
            {
                return cmd;
            }
        }

        return null;
    }

    public CommandSO GetNewCommand(string id)
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

    public List<CommandSO> GetNewCommands()
    {
        return newCommands;
    }

    private bool CheckCommandArguments(string[] args, CommandInfo info)
    {
        if (args != null)
        {
            if (args.Length == info.ARG_COUNT)
            {
                return true;
            }
        }

        if (args == null && info.ARG_COUNT == 0)
        {
            return true;
        }

        return false;
    }

    private bool CheckNewCommandArguments(string[] args, CommandSO command)
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

    private bool CheckHelpCommand(string[] args)
    {
        if (args != null && args.Length == 1)
        {
            for (int i = 0; i < helpInfo.HELPKEYWORDS.Length; i++)
            {
                if (args[0] == helpInfo.HELPKEYWORDS[i])
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void TriggerSuccessResponse(CommandInfo info)
    {
        ShowTerminalLines(info.SUCC_RESPONSE);
    }

    private void TriggerHelpResponse(CommandInfo info)
    {
        ShowTerminalLines(info.HELP_RESPONSE);
    }

    private void TriggerErrorResponse(CommandInfo info)
    {
        ShowTerminalLines(info.ERROR_RESPONSE);
    }

    private void ShowTerminalLines(List<string> lines)
    {
        terminal.AddInterpreterLines(lines);
    }

    #region COMMANDS
    public void Command_NetworkController(string[] args, CommandInfo cmdi)
    {
        if (args[0] == "init")
        {
            levelManager.BeginWave(OnGetCurrentWaveIndex());
            TriggerSuccessResponse(cmdi);
            //OnNetworkInit?.Invoke(); UNUSED
        }
    }

    public void Command_ReturnLocations(string[] args, CommandInfo cmdi)
    {
        List<string> locList = new List<string>();

        for (int i = 0; i < levelManager.LOCATIONS.Length; i++)
        {
            Location loc = levelManager.LOCATIONS[i];
            locList.Add(loc.ID);
            OnGeneratePopup?.Invoke(loc.ID, mainCamera.WorldToScreenPoint(loc.transform.position));
        }

        ShowTerminalLines(locList);
    }

    public void Command_UninstallTower(string[] args, CommandInfo cmdi)
    {
        if (mapHandler.GetIsCurrentLocationAvailable())
        {
            TriggerErrorResponse(cmdi);
            return;
        }

        string inputTowerId = args[0];
        Location currentLoc = mapHandler.CURRENT_LOCATION;

        BaseTower selectedTower = currentLoc.TOWER;

        if (inputTowerId != selectedTower.ID)
        {
            TriggerErrorResponse(cmdi);
            return;
        }

        towersController.ReleaseActiveTower(selectedTower);
        currentLoc.SetTower(null);
        currentLoc.SetAvailable(true);
        TriggerSuccessResponse(cmdi);
    }

    public void Command_WriteTutorial(string[] args, CommandInfo cmdi)
    {
        TutorialCommandInfo info = cmdi as TutorialCommandInfo;

        string tutorialId = args[0];
        for (int i = 0; i < info.TUTORIALS.Length; i++)
        {
            if (info.TUTORIALS[i].TUTORIAL_ID == tutorialId)
            {
                ShowTerminalLines(info.TUTORIALS[i].TUTORIAL_LINES);
                return;
            }
        }

        TriggerErrorResponse(info);
    }

    public void Command_ReloadScene(string[] args, CommandInfo cmdi)
    {
        SceneManager.LoadScene(1);
    }
    #endregion
}
