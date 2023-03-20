using System;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "command_reload", menuName = "ScriptableObjects/Commands/Reload")]
public class ReloadCommandSO : CommandSO
{
    public override void TriggerCommand(CommandManager commandManager, string[] arguments, Action<List<string>> onSuccess, Action<List<string>> onFailure)
    {
        commandManager.ChangeScene(SCENE.LEVEL_0);
    }
}