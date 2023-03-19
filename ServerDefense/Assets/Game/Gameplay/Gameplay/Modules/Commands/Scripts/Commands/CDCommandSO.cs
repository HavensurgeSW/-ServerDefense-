using System;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "command_CD", menuName = "ScriptableObjects/Commands/ChangeDirectory")]
public class CDCommandSO : CommandSO
{
    public override void TriggerCommand(CommandManager commandManager, string[] arguments, Action<List<string>> onSuccess, Action<List<string>> onFailure)
    {
        string locName = arguments[0];
        Camera mainCamera = commandManager.GetMainCamera();
        MapHandler mapHandler = commandManager.GetMapHandler();
        LevelManager levelManager = commandManager.GetLevelManager();
        TerminalManager terminal = commandManager.GetTerminalManager();
        UIManager uIManager = commandManager.GetUIManager();

        bool searchHit = false;

        foreach (Location loc in levelManager.LOCATIONS)
        {
            if (loc.ID == locName)
            {
                if (mapHandler.CURRENT_LOCATION != null)
                {
                    mapHandler.CURRENT_LOCATION.ToggleSelected(false);
                    mapHandler.SetTileToDefault(mapHandler.CURRENT_LOCATION.transform.position);
                }

                uIManager.GeneratePopUp(loc.ID, mainCamera.WorldToScreenPoint(loc.transform.position));

                mapHandler.SetCurrentLocation(loc);
                loc.ToggleSelected(true);
                mapHandler.SetTileToSelected(loc.transform.position);
                searchHit = true;
                terminal.ClearCmdEntries();

                //Action callout
                //OnChangeDirectory?.Invoke(locName);
                onSuccess(successResponse);

                break;
            }
        }

        if (!searchHit)
        {
            onFailure(errorResponse);
        }
    }
}