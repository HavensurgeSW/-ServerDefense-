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

    public void Init(CommandManagerModel commandManagerModel)
    {
        this.commandManagerModel = commandManagerModel;
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
            ShowTerminalLines(invalidArgumentAmountResponse);
            return;
        }

        command.TriggerCommand(commandManagerModel, commandArgs, ShowTerminalLines, OnCommandSuccess, OnCommandFailed);
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

        return null;
    }

    public List<CommandSO> GetCommands()
    {
        return commands;
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

    public TerminalResponseSO GetInvalidCommandResponse()
    {
        return invalidCommandResponse;
    }

    private void ShowTerminalLines(TerminalResponseSO response)
    {
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