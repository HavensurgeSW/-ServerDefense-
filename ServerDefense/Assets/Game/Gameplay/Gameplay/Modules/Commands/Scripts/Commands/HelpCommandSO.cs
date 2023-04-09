using System;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "command_help", menuName = "ScriptableObjects/Commands/Help")]
public class HelpCommandSO : CommandSO
{
    [Header("Help Command Configuration")]
    [SerializeField] private string[] helpKeywords = null;

    public string[] HELP_KEYWORDS => helpKeywords;

    public override void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<TerminalResponseSO> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
    {
        List<string> commandIds = GetCommandIds(commandManagerModel.COMMAND_MANAGER.GetCommands());

        TerminalManager terminal = commandManagerModel.TERMINAL_MANAGER;
        TerminalResponseSO response = terminal.GenerateCustomTerminalResponse(commandIds);
        onTriggerMessage(response);
        terminal.DeleteGeneratedTerminalResponse(response);
        onSuccess(this);
    }

    private List<string> GetCommandIds(List<CommandSO> commands)
    {
        List<string> ids = new List<string>();

        foreach (CommandSO command in commands)
        {
            ids.Add(command.COMMAND_ID.ToUpperInvariant());
        }

        return ids;
    }
}