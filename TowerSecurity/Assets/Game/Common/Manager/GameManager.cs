using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public event Action<string> OnChangeDirectory;
    public event Action<string> OnInstallTower;
    public event Action OnHelpArgument;
    public event Action OnNetworkInit;
    public event Action<string> OnUpdateTower;
    public event Action OnHelpCommand;

    [SerializeField] private List<Command> commands = new List<Command>();
    [SerializeField] private CommandManager commandManager = null;
    [SerializeField] private UIManager uiManager = null;
    [SerializeField] private TerminalManager terminal = null;
    [SerializeField] private TowersController towersController = null;
    [SerializeField] private LevelManager levelManager = null;
    [SerializeField] private MapHandler mapHandler = null;

    [Header("Debugging")]
    [SerializeField] private bool debugScore = false;
    [SerializeField] private int startingPackets;

    private Camera mainCamera = null;
    private int packetScore = 0;
    private int currentWave = 0;

    private void Awake()
    {
        mainCamera = Camera.main;

        packetScore = debugScore ? 10000 : startingPackets;
        uiManager.UpdatePacketPointsText(packetScore);

        Server.OnDeath += LoseGame;
        Server.OnPacketEntry += UpdatePacketScore;

        levelManager.Init();
        levelManager.OnWaveEnd += IncreaseCurrentWaveValue;
        uiManager.Init(/*levelManager.TIME_BETWEEN_WAVES*/0);
        towersController.Init();
        mapHandler.Init();
        terminal.Init(InterpretTerminalText);
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
        SceneManager.LoadScene(0);
    }

    #region TerminalInteractions
    private void InterpretTerminalText(string text)
    {
        text = text.ToLower();
        string[] arguments = text.Split(' ');
        bool searchHit = false;

        foreach (Command cmd in commands)
        {
            if (cmd.INFO.ID == arguments[0])
            {
                searchHit = true;
                ProcessCommand(cmd, arguments);
                break;
            }
        }

        if (!searchHit)
        {
            ShowTerminalLines(new List<string> { "Command not recognized. Type HELP for a list of commands" });
        }
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

    private void ProcessCommand(Command command, string[] fullArguments)
    {
        uiManager.ClearAllPopUps();

        string[] commandArg = new string[fullArguments.Length - 1];

        for (int i = 1; i <= commandArg.Length; i++)
        {
            commandArg[i - 1] = fullArguments[i];
        }

        if (commandManager.CheckHelpCommand(commandArg))
        {
            TriggerHelpResponse(command.INFO);
            OnHelpArgument?.Invoke();
            return;
        }

        if (!commandManager.CheckCommandArguments(commandArg, command.INFO))
        {
            ShowTerminalLines(new List<string> { "Invalid argument amount" });
            return;
        }

        command.CALLBACK?.Invoke(commandArg, command.INFO);
    }
    #endregion

    #region COMMAND_IMPLEMENTATIONS
    public void Command_ReturnCommands(string[] args, CommandInfo cmdi)
    {
        OnHelpCommand?.Invoke();

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
            levelManager.BeginWave(currentWave);
            TriggerSuccessResponse(cmdi);
            OnNetworkInit?.Invoke();
        }
    }

    public void Command_ReturnLocations(string[] args, CommandInfo cmdi)
    {
        List<string> locList = new List<string>();

        for (int i = 0; i < levelManager.LOCATIONS.Length; i++)
        {
            Location loc = levelManager.LOCATIONS[i];
            locList.Add(loc.ID);
            uiManager.GeneratePopUp(loc.ID, mainCamera.WorldToScreenPoint(loc.transform.position));
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

                uiManager.GeneratePopUp(loc.ID, mainCamera.WorldToScreenPoint(loc.transform.position));

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

        if (packetScore < data.LEVELS[0].PRICE)
        {
            ShowTerminalLines(info.INSUFFICIENT_FUNDS_RESPONSE);
            return;
        }

        UpdatePacketScore(-data.LEVELS[0].PRICE);

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

        List<BaseTowerLevelData> towerLevelsData = towersController.GetTowerLevelsData(selectedTower.ID);

        if (selectedTower.NEXT_LEVEL > towerLevelsData.Count)
        {
            ShowTerminalLines(info.MAX_LEVEL_RESPONSE);
            return;
        }

        BaseTowerLevelData nextLevelData = towerLevelsData[selectedTower.NEXT_LEVEL - 1];

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

    private void TriggerUpdateCommandInfo(UpdateCommandInfo info, BaseTower tower, BaseTowerLevelData nextLevelData)
    {
        List<string> updateInfoLines = info.GetNextUpdateInfo(tower, nextLevelData);
        ShowTerminalLines(updateInfoLines);
    }

    private void TriggerUpdateCommandDeploy(UpdateCommandInfo info, BaseTower tower, BaseTowerLevelData nextLevelData, Action onSuccess)
    {
        if (packetScore < nextLevelData.PRICE)
        {
            ShowTerminalLines(info.INSUFFICIENT_FUNDS_RESPONSE);
            return;
        }

        UpdatePacketScore(-nextLevelData.PRICE);
        tower.CURRENT_LEVEL++;
        tower.SetData(nextLevelData.DAMAGE, nextLevelData.RANGE, nextLevelData.FIRE_RATE, nextLevelData.TARGET_COUNT);
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