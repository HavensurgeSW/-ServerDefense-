using System;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "command_uninstall_", menuName = "ScriptableObjects/Commands/Uninstalls/UninstallTower")]
public class UninstallTowerCommandSO : CommandSO
{
    public override void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<TerminalResponseSO> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
    {
        MapHandler mapHandler = commandManagerModel.MAP_HANDLER;

        if (mapHandler.GetIsCurrentLocationAvailable())
        {
            onTriggerMessage(errorResponse);
            onFailure(this);
            return;
        }

        string inputTowerId = arguments[0];
        Location currentLoc = mapHandler.CURRENT_LOCATION;

        BaseTower selectedTower = currentLoc.TOWER;

        if (inputTowerId != selectedTower.ID)
        {
            onTriggerMessage(errorResponse);
            onFailure(this);
            return;
        }

        TowersController towersController = commandManagerModel.TOWERS_CONTROLLER;

        towersController.ReleaseActiveTower(selectedTower);
        currentLoc.SetTower(null);
        currentLoc.SetAvailable(true);
        onTriggerMessage(successResponse);
        onSuccess(this);
    }
}
