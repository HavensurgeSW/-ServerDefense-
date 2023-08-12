using System;

using UnityEngine;

using ServerDefense.Gameplay.Gameplay.Modules.Terminal;

namespace ServerDefense.Gameplay.Gameplay.Modules.Commands
{
    [CreateAssetMenu(fileName = "command_reload", menuName = "ScriptableObjects/Commands/Reload")]
    public class ReloadCommandSO : CommandSO
    {
        public override void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<TerminalResponseSO> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
        {
            commandManagerModel.COMMAND_MANAGER.ChangeScene(SCENE.LEVEL_0);
        }
    }
}