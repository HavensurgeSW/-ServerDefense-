using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Command> commands = new List<Command>();
    [SerializeField] private CommandManager commandManager = null;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private TerminalManager terminal;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private MapHandler mapHandler;
    [SerializeField] private PacketData packetData;

    [SerializeField] private BaseTower prefab;
    [SerializeField] private BaseTower prefab2;

    private int packetScore;

    private void Awake()
    {
        packetScore = 10;
        uiManager.UpdatePacketPointsText(packetScore);
        Server.OnDeath += LoseGame;
        Server.OnPacketEntry += UpdatePacketScore;

        uiManager.Init();
        mapHandler.Init();
        terminal.Init(InterpretTerminalText);
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
            terminal.AddInterpreterLines(new List<string> { "Command not recognized. Type CMDS for a list of commands" });
        }
    }

    private void TriggerSuccessResponse(CommandInfo info)
    {
        terminal.AddInterpreterLines(info.SUCC_RESPONSE);
    }

    private void TriggerHelpResponse(CommandInfo info)
    {
        terminal.AddInterpreterLines(info.HELP_RESPONSE);
    }

    private void TriggerErrorResponse(CommandInfo info)
    {
        terminal.AddInterpreterLines(info.ERROR_RESPONSE);
    }

    private void ProcessCommand(Command command, string[] fullArguments)
    {
        string[] commandArg = new string[fullArguments.Length - 1];

        for (int i = 1; i <= commandArg.Length; i++)
        {
            commandArg[i - 1] = fullArguments[i];
        }

        if (commandManager.CheckHelpCommand(commandArg))
        {
            TriggerHelpResponse(command.INFO);
            return;
        }

        if (!commandManager.CheckCommandArguments(commandArg, command.INFO))
        {
            terminal.AddInterpreterLines(new List<string> { "Invalid argument amount" });
            return;
        }

        command.CALLBACK?.Invoke(commandArg, command.INFO);
    }
    #endregion

    #region COMMAND_IMPLEMENTATIONS
    public void Command_ReturnCommands(string[] arg, CommandInfo cmdi)
    {
        TriggerSuccessResponse(cmdi);
    }

    public void Command_NetworkController(string[] arg, CommandInfo cmdi)
    {
        if (arg[0] == "init")
        {
            levelManager.BeginWave();
            TriggerSuccessResponse(cmdi);
        }

        if (arg[0] == "pause")
        {
            levelManager.PauseWave();
            TriggerSuccessResponse(cmdi);
        }
    }

    public void Command_ReturnLocations(string[] arg, CommandInfo cmdi)
    {
        List<string> locList = new List<string>();

        for (int i = 0; i < levelManager.LOCATIONS.Length; i++)
        {
            locList.Add(levelManager.LOCATIONS[i].ID);
        }

        terminal.AddInterpreterLines(locList);
    }

    public void Command_ChangeDirectory(string[] arg, CommandInfo cmdi)
    {
        string locName = arg[0];
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

                mapHandler.SetCurrentLocation(loc);
                loc.ToggleSelected(true);
                mapHandler.SetTileToSelected(loc.transform.position);
                searchHit = true;
                terminal.ClearCmdEntries();
                break;
            }
        }

        if (!searchHit)
        {
            TriggerErrorResponse(cmdi);
        }
    }
    public void Command_Hello(string[] arg, CommandInfo cmdi)
    {
        TriggerSuccessResponse(cmdi);
    }

    public void Command_InstallTower(string[] arg, CommandInfo cmdi)
    {
        terminal.ClearCmdEntries();

        if (mapHandler.GetIsCurrentLocationAvailable())
        {
            BaseTower tower = null;
            List<string> response = new List<string>();
            switch (arg[0])
            {
                case "antivirus":
                    if (packetScore >= 8) //Add cost through DataAsset to pull from
                    {
                        UpdatePacketScore(-8);
                        tower = prefab;
                    }
                    else {
                        response.Add("Insufficient funds to install program");
                        terminal.AddInterpreterLines(response);
                        return;
                    }
                    break;
                case "firewall":
                    
                    if (packetScore >= 16) //Add cost through DataAsset to pull from
                    {
                        UpdatePacketScore(-16);
                        tower = prefab2;
                    }
                    else
                    {
                        response.Add("Insufficient funds to install program");
                        terminal.AddInterpreterLines(response);
                        return;
                    }
                    break;
                default:
                    TriggerErrorResponse(cmdi);
                    return;
            }

            Location currentLoc = mapHandler.CURRENT_LOCATION;
            currentLoc.SetAvailable(false);
            Instantiate(tower, currentLoc.transform);
            TriggerSuccessResponse(cmdi);
        }
        else
        {
            List<string> strings = new List<string>();
            strings.Add("Invalid location selected");
            terminal.AddInterpreterLines(strings);
        }
    }
    public void Command_WriteTutorial(string[] arg, CommandInfo cmdi)
    {
        TutorialCommandInfo info = cmdi as TutorialCommandInfo;

        string tutorialId = arg[0];
        for (int i = 0; i < info.TUTORIALS.Length; i++)
        {
            if (info.TUTORIALS[i].TUTORIAL_ID == tutorialId)
            {
                terminal.AddInterpreterLines(info.TUTORIALS[i].TUTORIAL_LINES);
                return;
            }
        }

        TriggerErrorResponse(info);
    }

    public void Command_ReloadScene(string[] arg, CommandInfo cmdi)
    {
        SceneManager.LoadScene(1);
    }
    public void Command_QuitGame(string[] arg, CommandInfo cmdi)
    {
        if (arg[0] == "application")
        {
#if !UNITY_EDITOR
            Application.Quit();
#else
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
    #endregion

    #region DEBUG_COMMANDS
    public void Command_Debug1(string[] arg, CommandInfo cmdi)
    {
        TriggerSuccessResponse(cmdi);
    }
    #endregion
}