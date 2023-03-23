using System;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "command_CMDS", menuName = "ScriptableObjects/Commands/CMDS")]
public class CMDSCommandSO : CommandSO
{
    public override void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<List<string>> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
    {
        List<string> commandIds = GetCommandIds(commandManagerModel.OnGetCommands());

        onTriggerMessage(commandIds);
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
