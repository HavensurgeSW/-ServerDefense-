using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Command> commands = new List<Command>();
    [SerializeField] private CommandManager commandManager = null;
    [SerializeField] private TerminalManager terminal;
    [SerializeField] private LevelManager levelManager;

    [SerializeField] private Tower prefab;
    [SerializeField] private Tower prefab2;

    [SerializeField] Tilemap tilemap;
    [SerializeField] TileBase selectedLocation;
    [SerializeField] TileBase defaultLocation;

    private Location currentLocation;

    private void Awake()
    {
        Server.OnDeath += LoseGame;
        terminal.Init(InterpretTerminalText);
        currentLocation = null;
    }

    private void OnDisable()
    {
        Server.OnDeath -= LoseGame;
    }

    private void LoseGame() {
        SceneManager.LoadScene(0);
    }

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

        if (!searchHit) {
            terminal.AddInterpreterLines(new List<string> { "Command not recognized. Type CMDS for a list of commands" });
        }
    }

    private void TriggerSuccessResponse(CommandInfo info)
    {
        terminal.AddInterpreterLines(info.SUCCRESPONSE);
    }
    
    private void TriggerHelpResponse(CommandInfo info)
    {
        terminal.AddInterpreterLines(info.HELPRESPONSE);

    }
    private void TriggerErrorResponse(CommandInfo info)
    {
        terminal.AddInterpreterLines(info.ERRORRESPONSE);
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

    #region COMMAND_IMPLEMENTATIONS
    public void Command_ReturnCommands(string[] arg, CommandInfo cmdi)
    {
        TriggerSuccessResponse(cmdi);
    }

    public void Command_NetworkController(string[] arg, CommandInfo cmdi)
    {
        if (arg[0] == "init") {
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

        for (int i = 0; i <levelManager.LOCATIONS.Length; i++)
        {
            locList.Add(levelManager.LOCATIONS[i].ID);
        }

        terminal.AddInterpreterLines(locList);
    }

    public void Command_ChangeDirectory(string[] arg, CommandInfo cmdi) {

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

        if (!searchHit) {
            TriggerErrorResponse(cmdi);
        }
    }
    public void Command_Hello(string[] arg, CommandInfo cmdi) {

        TriggerSuccessResponse(cmdi);
    }
    public void Command_InstallTower(string[] arg, CommandInfo cmdi){

        terminal.ClearCmdEntries();

        if (currentLocation != null && currentLocation.CheckForLocationAvailability())
        {
            Tower tower = null;
            switch (arg[0])
            {
                case "antivirus":
                    tower = prefab;
                    break;
                case "firewall":
                    tower = prefab2;
                    break;
                default:
                    TriggerErrorResponse(cmdi);
                    return;
            }

            currentLocation.SetAvailable(false);
            Instantiate(tower, currentLocation.transform);
            TriggerSuccessResponse(cmdi);
        }
    }
    public void Command_WriteTutorial(string[] arg, CommandInfo cmdi)
    {
        TriggerSuccessResponse(cmdi);
    }
    public void Command_ReloadScene(string[] arg, CommandInfo cmdi) {
        SceneManager.LoadScene(1);
    }
    public void Command_QuitGame(string[] arg, CommandInfo cmdi)
    {
        if (arg[0] == "application") {
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
