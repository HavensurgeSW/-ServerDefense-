using System;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "command_uninstall_", menuName = "ScriptableObjects/Commands/Uninstalls/UninstallTower")]
public class UninstallTowerCommandSO : CommandSO
{
    public override void TriggerCommand(CommandManager commandManager, string[] arguments, Action<List<string>> onSuccess, Action<List<string>> onFailure)
    {
        MapHandler mapHandler = commandManager.GetMapHandler();

        if (mapHandler.GetIsCurrentLocationAvailable())
        {
            onFailure(errorResponse);
            return;
        }

        string inputTowerId = arguments[0];
        Location currentLoc = mapHandler.CURRENT_LOCATION;

        BaseTower selectedTower = currentLoc.TOWER;

        if (inputTowerId != selectedTower.ID)
        {
            onFailure(errorResponse);
            return;
        }

        TowersController towersController = commandManager.GetTowersController();

        towersController.ReleaseActiveTower(selectedTower);
        currentLoc.SetTower(null);
        currentLoc.SetAvailable(true);
        onSuccess(successResponse);
    }
}
