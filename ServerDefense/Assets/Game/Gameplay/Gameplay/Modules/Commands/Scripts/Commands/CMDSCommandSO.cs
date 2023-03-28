using System;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "command_CMDS", menuName = "ScriptableObjects/Commands/CMDS")]
public class CMDSCommandSO : CommandSO
{
    public override void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<TerminalResponseSO> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
    {
        List<string> commandIds = GetCommandIds(commandManagerModel.COMMAND_MANAGER.GetCommands());

        TerminalResponseSO response = GenerateCustomTerminalResponse(commandIds);
        onTriggerMessage(response);
        DeleteGeneratedTerminalResponse(response);
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
