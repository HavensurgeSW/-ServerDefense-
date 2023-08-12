using System;

using UnityEngine;

using ServerDefense.Gameplay.Gameplay.Modules.Map;
using ServerDefense.Gameplay.Gameplay.Modules.Terminal;

namespace ServerDefense.Gameplay.Gameplay.Modules.Commands
{
    [CreateAssetMenu(fileName = "command_CD", menuName = "ScriptableObjects/Commands/ChangeDirectory")]
    public class CDCommandSO : CommandSO
    {
        public override void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<TerminalResponseSO> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
        {
            string locName = arguments[0];
            MapHandler mapHandler = commandManagerModel.MAP_HANDLER;

            bool foundLocation = false;

            foreach (Location loc in mapHandler.LOCATIONS)
            {
                if (loc.ID == locName)
                {
                    SelectLocation(commandManagerModel, loc);
                    foundLocation = true;
                    onTriggerMessage(successResponse);
                    onSuccess(this);
                    break;
                }
            }

            if (!foundLocation)
            {
                onTriggerMessage(errorResponse);
                onFailure(this);
            }
        }

        private void SelectLocation(CommandManagerModel model, Location location)
        {
            MapHandler mapHandler = model.MAP_HANDLER;

            mapHandler.SetCurrentLocation(location);

            TerminalManager terminal = model.TERMINAL_MANAGER;
            terminal.ClearCmdEntries();
        }
    }
}