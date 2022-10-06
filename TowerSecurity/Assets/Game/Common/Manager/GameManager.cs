using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Command> commands = new List<Command>();
    [SerializeField] private CommandManager commandManager = null;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private TerminalManager terminal;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private PacketData packetData;

    [SerializeField] private Tower prefab;
    [SerializeField] private Tower prefab2;

    [SerializeField] Tilemap tilemap;
    [SerializeField] TileBase selectedLocation;
    [SerializeField] TileBase defaultLocation;

    private int packetScore;
    private Location currentLocation;

    private void Awake()
    {
        packetScore = 10;
        uiManager.UpdatePacketPointsText(packetScore);
        Server.OnDeath += LoseGame;
        Server.OnPacketEntry += UpdatePacketScore;

        uiManager.Init();
        terminal.Init(InterpretTerminalText);
        currentLocation = null;
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
                currentLocation = loc;
                loc.ToggleSelected(true);
                Vector3Int cellPos = tilemap.WorldToCell(loc.transform.position);
                tilemap.SetTile(cellPos, selectedLocation);
                searchHit = true;
                terminal.ClearCmdEntries();
            }
            else
            {
                loc.ToggleSelected(false);
                Vector3Int cellPos = tilemap.WorldToCell(loc.transform.position);
                tilemap.SetTile(cellPos, defaultLocation);
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

        if (currentLocation != null && currentLocation.CheckForLocationAvailability())
        {
            Tower tower = null;
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
                    }
                    break;
                default:
                    TriggerErrorResponse(cmdi);
                    return;
            }

            currentLocation.SetAvailable(false);
            Instantiate(tower, currentLocation.transform);
            TriggerSuccessResponse(cmdi);
        }
        else
        {
            List<string> strings = new List<string>();
            strings.Add("No location selected");
            terminal.AddInterpreterLines(strings);
        }
    }
    public void Command_WriteTutorial(string[] arg, CommandInfo cmdi)
    {
        List<string> tutorialOutput = new List<string>();
        if (arg[0] == "1")
        {
            tutorialOutput.Add("Welcome to the Network");
            tutorialOutput.Add("Your job is to protect the server from viruses");
            tutorialOutput.Add("To do this, deploy towers on the <locations> available");
            tutorialOutput.Add("Begin by writing CD LOC1 on the terminal");
            tutorialOutput.Add("Then go ahead and run TUTORIAL 2 to continue");
        }
        
        if (arg[0] == "2") 
        {
            tutorialOutput.Add("You can use NETSTAT to find the name of all available locations");
            tutorialOutput.Add("Most commands can take <Arguments>. These are parameters that change the behaviour");
            tutorialOutput.Add("All commands have a HELP argument. EG: CD HELP, INSTALL HELP");
            tutorialOutput.Add("Use the HELP argument to find out more about the command");
            tutorialOutput.Add("Run TUTORIAL 3 to continue");
        }
        if (arg[0] == "3")
        {
            tutorialOutput.Add("Deploying towers costs Kilobytes, noted as KB on the left");
            tutorialOutput.Add("Packets reaching your server will increase your current KB");
            tutorialOutput.Add("With a selected <location>, run the INSTALL command");
            tutorialOutput.Add("You might want to add the HELP argument to learn how to use it");
            tutorialOutput.Add("All available commands can be seen by running CMDS");
            tutorialOutput.Add("When you're ready, open up your network by running NETWORK INIT");

        }

        terminal.AddInterpreterLines(tutorialOutput);
        //TriggerSuccessResponse(cmdi);

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