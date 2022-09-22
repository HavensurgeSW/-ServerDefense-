using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Command> commands = new List<Command>();
    [SerializeField] private CommandManager commandManager = null;
    [SerializeField] private TerminalManager terminal;
    [SerializeField] private LevelManager levelManager;

    [SerializeField] private Tower prefab;
    [SerializeField] private Tower prefab2;

    private Location currentLocation;

    private void Awake()
    {
        Server.OnDeath += LoseGame;
        terminal.Init(InterpretCommand);
        currentLocation = null;
    }

    private void OnDisable()
    {
        Server.OnDeath -= LoseGame;
    }

    private void LoseGame() {
        SceneManager.LoadScene(0);
    }

    private void InterpretCommand(string text)
    {
        text = text.ToLower();
        string[] arguments = text.Split(' ');
        bool searchHit = false;
       
        foreach (Command cmd in commands)
        {
            if (cmd.INFO.ID == arguments[0])
            {
                searchHit = true;
                string[] tempArg = new string[arguments.Length - 1];
                for (int i = 1; i <= tempArg.Length; i++)
                {
                    tempArg[i - 1] = arguments[i];
                }
                cmd.CALLBACK?.Invoke(tempArg, cmd.INFO);
                break;
            }
     
        }

        if (!searchHit) {
            terminal.AddInterpreterLines(new List<string> { "Command not recognized. Type CMDS for a list of commands" });
        }

    }

    #region COMMAND_IMPLEMENTATIONS
    public void Command_ReturnCommands(string[] arg, CommandInfo cmdi)
    {
        if (commandManager.CheckHelpCommand(arg))
        {
            terminal.AddInterpreterLines(cmdi.HELPRESPONSE);
            return;
        }
        terminal.AddInterpreterLines(cmdi.SUCCRESPONSE);
    }

    public void Command_NetworkController(string[] arg, CommandInfo cmdi)
    {
        if (commandManager.CheckHelpCommand(arg))
        {
            terminal.AddInterpreterLines(cmdi.HELPRESPONSE);
            return;
        }

        if (arg[0] == "init") {
            levelManager.BeginWave();
            terminal.AddInterpreterLines(cmdi.SUCCRESPONSE);
        }

        if (arg[0] == "pause")
        {
            levelManager.PauseWave();
            terminal.AddInterpreterLines(cmdi.SUCCRESPONSE);
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

        if (commandManager.CheckHelpCommand(arg))
        {
            terminal.AddInterpreterLines(cmdi.HELPRESPONSE);
        }
    }
    public void Command_ChangeDirectory(string[] arg, CommandInfo cmdi) {
        //arguments.length-1 != argCountSO
        string locName = arg[0];
        bool searchHit = false;
        foreach (Location loc in levelManager.LOCATIONS)
        {
            if (commandManager.CheckHelpCommand(arg))
            {
                terminal.AddInterpreterLines(cmdi.HELPRESPONSE);
                searchHit = true;
                break;
            }

            if (loc.ID == locName)
            {
                currentLocation = loc;
                loc.ToggleSelected(true);
                loc.ToggleColor(Color.red);
                searchHit = true;
                terminal.ClearCmdEntries();
            }
            else
            {
                loc.ToggleSelected(false);
                loc.ToggleColor(Color.white);
            }
        }



        if (!searchHit) {
            terminal.AddInterpreterLines(cmdi.ERRORRESPONSE);
        }
    }
    public void Command_Hello(string[] arg, CommandInfo cmdi) {

        if (commandManager.CheckHelpCommand(arg))
        {
            terminal.AddInterpreterLines(cmdi.HELPRESPONSE);
            return;
        }

        terminal.AddInterpreterLines(cmdi.SUCCRESPONSE);
    }
    public void Command_InstallTower(string[] arg, CommandInfo cmdi){

        terminal.ClearCmdEntries();

        if (commandManager.CheckHelpCommand(arg))
        {
            terminal.AddInterpreterLines(cmdi.HELPRESPONSE);
            return;
        }

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
                    terminal.AddInterpreterLines(cmdi.ERRORRESPONSE);
                    return;
            }

            currentLocation.SetAvailable(false);
            Instantiate(tower, currentLocation.transform);
            terminal.AddInterpreterLines(cmdi.SUCCRESPONSE);
        }
    }
    public void Command_WriteTutorial(string[] arg, CommandInfo cmdi)
    {
        terminal.AddInterpreterLines(cmdi.SUCCRESPONSE);
    }
    public void Command_ReloadScene(string[] arg, CommandInfo cmdi) {
        SceneManager.LoadScene(1);
    }
    public void Command_QuitGame(string[] arg, CommandInfo cmdi)
    {
        if (commandManager.CheckHelpCommand(arg))
        {
            terminal.AddInterpreterLines(cmdi.HELPRESPONSE);
        }

        if (arg[0] == "application") {
#if !UNITY_EDITOR
        Application.Quit();
#else
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
    #endregion

}
