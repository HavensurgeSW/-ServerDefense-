using System;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "command_CMDS", menuName = "ScriptableObjects/Commands/CMDS")]
public class CMDSCommandSO : CommandSO
{
    public override void TriggerCommand(CommandManager commandManager, string[] arguments, Action<List<string>> onSuccess, Action<List<string>> onFailure)
    {
        List<string> commandIds = GetCommandIds(commandManager.GetNewCommands());

        onSuccess(commandIds);
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
