using System;

using UnityEngine;

[CreateAssetMenu(fileName = "command_reload", menuName = "ScriptableObjects/Commands/Reload")]
public class ReloadCommandSO : CommandSO
{
    public override void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<TerminalResponseSO> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
    {
        commandManagerModel.COMMAND_MANAGER.ChangeScene(SCENE.LEVEL_0);
    }
}