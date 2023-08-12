using System;
using System.Collections.Generic;

using UnityEngine;

using ServerDefense.Gameplay.Gameplay.Modules.Terminal;

namespace ServerDefense.Gameplay.Gameplay.Modules.Commands
{
    [CreateAssetMenu(fileName = "command_returnCommands", menuName = "ScriptableObjects/Commands/ReturnCommandsCommand")]
    public class ReturnCommandsCommandSO : CommandSO
    {
        public override void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<TerminalResponseSO> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
        {
            List<string> commandIds = GetCommandIds(commandManagerModel.COMMAND_MANAGER.GetCommands());
            TerminalResponseSO response = TerminalResponseSO.CreateInstance(commandIds);
            onTriggerMessage(response);
            TerminalResponseSO.Destroy(response);
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
}