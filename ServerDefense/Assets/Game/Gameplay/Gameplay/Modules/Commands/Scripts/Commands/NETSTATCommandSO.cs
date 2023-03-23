using System;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "command_NETSTAT", menuName = "ScriptableObjects/Commands/NETSTAT")]
public class NETSTATCommandSO : CommandSO
{
    public override void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<List<string>> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
    {
        LevelManager levelManager = commandManagerModel.LEVEL_MANAGER;
        UIManager uIManager = commandManagerModel.UI_MANAGER;
        Camera mainCamera = commandManagerModel.MAIN_CAMERA;
        List<string> locList = new List<string>();

        for (int i = 0; i < levelManager.LOCATIONS.Length; i++)
        {
            Location loc = levelManager.LOCATIONS[i];
            locList.Add(loc.ID);
            uIManager.GeneratePopUp(loc.ID, mainCamera.WorldToScreenPoint(loc.transform.position));
        }

        onTriggerMessage(locList);
        onSuccess(this);
    }
}