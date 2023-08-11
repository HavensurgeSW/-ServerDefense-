using System;
using System.Collections.Generic;

using UnityEngine;

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
    [SerializeField] private List<CommandSO> commands = null;
    
    [Header("Helper commands Configuration")]
    [SerializeField] private HelpCommandSO helpCommand = null;

    [Header("Responses Configuration")]
    [SerializeField] private TerminalResponseSO invalidCommandResponse = null;
    [SerializeField] private TerminalResponseSO invalidArgumentAmountResponse = null;

    private CommandManagerModel commandManagerModel = null;

    private Action<SCENE> OnChangeScene = null;
    private Action OnCommandProcessed = null;

    public void Init(CommandManagerModel commandManagerModel)
    {
        this.commandManagerModel = commandManagerModel;
    }

    public void SetCallbacks(Action<SCENE> onChangeScene, Action onCommandProcessed)
    {
        OnChangeScene = onChangeScene;
        OnCommandProcessed = onCommandProcessed;
    }

    public void ChangeScene(SCENE scene)
    {
        OnChangeScene?.Invoke(scene);
    }

    public void ProcessCommand(string textToInterpret)
    {
        textToInterpret = textToInterpret.ToLower();
        CommandDataModel commandModel = new CommandDataModel(textToInterpret);
        CommandSO command = GetCommand(commandModel);

        OnCommandProcessed?.Invoke();
        ProcessCommand(command, commandModel);
    }

    public void ProcessCommand(CommandSO command, CommandDataModel commandModel)
    {
        if (command == null)
        {
            ShowTerminalLines(invalidCommandResponse);
            return;
        }

        if (CheckForHelpCommand(commandModel.ARGUMENTS))
        {
            command.TriggerHelpResponse(commandManagerModel, ShowTerminalLines);
            OnHelpArgument?.Invoke();
            return;
        }

        if (!CheckCommandArguments(commandModel.ARGUMENTS, command))
        {
            ShowTerminalLines(invalidArgumentAmountResponse);
            return;
        }

        command.TriggerCommand(commandManagerModel, commandModel.ARGUMENTS, ShowTerminalLines, OnCommandSuccess, OnCommandFailed);
    }

    public CommandSO GetCommand(CommandDataModel model)
    {
        return GetCommand(model.ID);
    }

    public CommandSO GetCommand(string id)
    {
        foreach (CommandSO cmd in commands)
        {
            if (cmd.COMMAND_ID == id)
            {
                return cmd;
            }
        }

        Debug.LogError("Command of id " + id + " not found.");
        return null;
    }

    public List<CommandSO> GetCommands()
    {
        return commands;
    }

    private bool CheckCommandArguments(string[] args, CommandSO command)
    {
        return args.Length == command.ARGUMENTS_COUNT;
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

    private void ShowTerminalLines(TerminalResponseSO response)
    {
        if (response == null)
        {
            return;
        }

        commandManagerModel.TERMINAL_MANAGER.AddInterpreterLines(response);
    }

    private void OnCommandSuccess(CommandSO command)
    {
        // whatever
    }

    private void OnCommandFailed(CommandSO command)
    {
        // whatever
    }
}