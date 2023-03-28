using System;

using UnityEngine;

[CreateAssetMenu(fileName = "command_CD", menuName = "ScriptableObjects/Commands/ChangeDirectory")]
public class CDCommandSO : CommandSO
{
    public override void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<TerminalResponseSO> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
    {
        string locName = arguments[0];
        Camera mainCamera = commandManagerModel.MAIN_CAMERA;
        MapHandler mapHandler = commandManagerModel.MAP_HANDLER;
        LevelManager levelManager = commandManagerModel.LEVEL_MANAGER;

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

                UIManager uIManager = commandManagerModel.UI_MANAGER;
                uIManager.GeneratePopUp(loc.ID, mainCamera.WorldToScreenPoint(loc.transform.position));

                mapHandler.SetCurrentLocation(loc);
                loc.ToggleSelected(true);
                mapHandler.SetTileToSelected(loc.transform.position);
                searchHit = true;
                
                TerminalManager terminal = commandManagerModel.TERMINAL_MANAGER;
                terminal.ClearCmdEntries();

                //Action callout
                //OnChangeDirectory?.Invoke(locName);
                onTriggerMessage(successResponse);
                onSuccess(this);
                break;
            }
        }

        if (!searchHit)
        {
            onTriggerMessage(errorResponse);
            onFailure(this);
        }
    }
}