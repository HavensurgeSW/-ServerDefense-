using System;

using UnityEngine;

[CreateAssetMenu(fileName = "command_CD", menuName = "ScriptableObjects/Commands/ChangeDirectory")]
public class CDCommandSO : CommandSO
{
    public override void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<TerminalResponseSO> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
    {
        string locName = arguments[0];
        LevelManager levelManager = commandManagerModel.LEVEL_MANAGER;

        bool foundLocation = false;

        foreach (Location loc in levelManager.LOCATIONS)
        {
            if (loc.ID == locName)
            {
                SelectLocation(commandManagerModel, loc);
                foundLocation = true;
                
                //Action callout
                //OnChangeDirectory?.Invoke(locName);
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

        if (mapHandler.CURRENT_LOCATION != null)
        {
            mapHandler.CURRENT_LOCATION.ToggleSelected(false);
            mapHandler.SetTileToDefault(mapHandler.CURRENT_LOCATION.transform.position);
        }

        Camera mainCamera = model.MAIN_CAMERA;
        UIManager uIManager = model.UI_MANAGER;
        uIManager.GeneratePopUp(location.ID, mainCamera.WorldToScreenPoint(location.transform.position));

        mapHandler.SetCurrentLocation(location);
        location.ToggleSelected(true);
        mapHandler.SetTileToSelected(location.transform.position);

        TerminalManager terminal = model.TERMINAL_MANAGER;
        terminal.ClearCmdEntries();
    }
}