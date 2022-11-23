using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    [Header("Helper commands Configuration")]
    [SerializeField] private HelpCommandInfo helpInfo = null;

    private Camera mainCamera = null;

    private TerminalManager terminal = null;
    private MapHandler mapHandler = null;
    private LevelManager levelManager = null;
    private TowersController towersController = null;

    private Action<int> OnUpdatePacketScore = null;
    private Action<string, Vector3> OnGeneratePopup = null;
    private Func<int> OnGetCurrentWaveIndex = null;
    private Func<int> OnGetPacketScore = null;

    public void Init(TerminalManager terminal, LevelManager levelManager, MapHandler mapHandler, TowersController towersController)
    {
        mainCamera = Camera.main;

        this.terminal = terminal;
        this.levelManager = levelManager;
        this.mapHandler = mapHandler;
        this.towersController = towersController;
    }

    public void SetCallbacks(Action<int> onUpdatePacketScore, Func<int> onGetCurrentWaveIndex, Func<int> onGetPacketScore, Action<string, Vector3> onGeneratePopup)
    {
        OnUpdatePacketScore = onUpdatePacketScore;
        OnGetCurrentWaveIndex = onGetCurrentWaveIndex;
        OnGetPacketScore = onGetPacketScore;
        OnGeneratePopup = onGeneratePopup;
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
    public void Command_ReturnCommands(string[] args, CommandInfo cmdi)
    {
        //OnHelpCommand?.Invoke(); UNUSED

        List<string> commandIds = new List<string>();

        for (int i = 0; i < commands.Count; i++)
        {
            commandIds.Add(commands[i].INFO.ID.ToUpperInvariant());
        }

        ShowTerminalLines(commandIds);
    }

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

    public void Command_ChangeDirectory(string[] args, CommandInfo cmdi)
    {
        string locName = args[0];
        bool searchHit = false;

        foreach (Location loc in levelManager.LOCATIONS)
        {
            if (loc.ID == locName)
            {
                if (mapHandler.CURRENT_LOCATION != null)
                {
                    mapHandler.CURRENT_LOCATION.ToggleSelected(false);
                    mapHandler.SetTileToDefault(mapHandler.CURRENT_LOCATION.transform.position);
                }

                OnGeneratePopup?.Invoke(loc.ID, mainCamera.WorldToScreenPoint(loc.transform.position));

                mapHandler.SetCurrentLocation(loc);
                loc.ToggleSelected(true);
                mapHandler.SetTileToSelected(loc.transform.position);
                searchHit = true;
                terminal.ClearCmdEntries();

                //Action callout
                OnChangeDirectory?.Invoke(locName);

                break;
            }
        }

        if (!searchHit)
        {
            TriggerErrorResponse(cmdi);
        }
    }

    public void Command_InstallTower(string[] args, CommandInfo cmdi)
    {
        terminal.ClearCmdEntries();

        InstallCommandInfo info = cmdi as InstallCommandInfo;

        if (!mapHandler.GetIsCurrentLocationAvailable())
        {
            ShowTerminalLines(info.INVALID_LOCATION_RESPONSE);
            return;
        }

        string towerId = args[0];

        if (!towersController.DoesTowerIdExist(towerId))
        {
            ShowTerminalLines(info.INVALID_TOWER_ID_RESPONSE);
            return;
        }

        TowerData data = towersController.GetTowerData(towerId);

        if (OnGetPacketScore() < data.LEVELS[0].PRICE)
        {
            ShowTerminalLines(info.INSUFFICIENT_FUNDS_RESPONSE);
            return;
        }

        OnUpdatePacketScore?.Invoke(-data.LEVELS[0].PRICE);

        Location currentLoc = mapHandler.CURRENT_LOCATION;
        BaseTower actualTower = towersController.GenerateTower(towerId, currentLoc.transform);

        actualTower.SetPosition(currentLoc.transform.position + (Vector3)data.OFFSET);
        currentLoc.SetTower(actualTower);
        currentLoc.SetAvailable(false);

        OnInstallTower?.Invoke(towerId);

        TriggerSuccessResponse(cmdi);
    }

    public void Command_UpdateTower(string[] args, CommandInfo cmdi)
    {
        string keyword = args[0];
        UpdateCommandInfo info = cmdi as UpdateCommandInfo;

        if (keyword != info.INFO_ID && keyword != info.DEPLOY_ID)
        {
            TriggerErrorResponse(info);
            return;
        }

        if (mapHandler.GetIsCurrentLocationAvailable())
        {
            ShowTerminalLines(info.INVALID_LOCATION_RESPONSE);
            return;
        }

        BaseTower selectedTower = mapHandler.CURRENT_LOCATION.TOWER;
        if (selectedTower == null)
        {
            ShowTerminalLines(info.INVALID_LOCATION_RESPONSE);
            return;
        }

        TowerLevelData[] towerLevelsData = towersController.GetTowerLevelsData(selectedTower.ID);

        if (selectedTower.NEXT_LEVEL > towerLevelsData.Length)
        {
            ShowTerminalLines(info.MAX_LEVEL_RESPONSE);
            return;
        }

        TowerLevelData nextLevelData = towerLevelsData[selectedTower.NEXT_LEVEL - 1];

        if (keyword == info.INFO_ID)
        {
            TriggerUpdateCommandInfo(info, selectedTower, nextLevelData);
        }
        else if (keyword == info.DEPLOY_ID)
        {
            OnUpdateTower?.Invoke(selectedTower.ID);
            TriggerUpdateCommandDeploy(info, selectedTower, nextLevelData, () => TriggerSuccessResponse(info));
        }
    }

    private void TriggerUpdateCommandInfo(UpdateCommandInfo info, BaseTower tower, TowerLevelData nextLevelData)
    {
        List<string> updateInfoLines = info.GetNextUpdateInfo(tower, nextLevelData);
        ShowTerminalLines(updateInfoLines);
    }

    private void TriggerUpdateCommandDeploy(UpdateCommandInfo info, BaseTower tower, TowerLevelData nextLevelData, Action onSuccess)
    {
        if (OnGetPacketScore() < nextLevelData.PRICE)
        {
            ShowTerminalLines(info.INSUFFICIENT_FUNDS_RESPONSE);
            return;
        }

        OnUpdatePacketScore?.Invoke(-nextLevelData.PRICE);
        tower.CURRENT_LEVEL++;
        tower.SetData(nextLevelData.STATS);
        onSuccess?.Invoke();
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

    public void Command_QuitGame(string[] args, CommandInfo cmdi)
    {
        if (args[0] == "application")
        {
            SceneManager.LoadScene(1);
        }
    }
    #endregion
}
