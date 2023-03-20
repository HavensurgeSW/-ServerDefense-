using System;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "command_NETSTAT", menuName = "ScriptableObjects/Commands/NETSTAT")]
public class NETSTATCommandSO : CommandSO
{
    public override void TriggerCommand(CommandManager commandManager, string[] arguments, Action<List<string>> onSuccess, Action<List<string>> onFailure)
    {
        LevelManager levelManager = commandManager.GetLevelManager();
        UIManager uIManager = commandManager.GetUIManager();
        Camera mainCamera = commandManager.GetMainCamera();
        List<string> locList = new List<string>();

        for (int i = 0; i < levelManager.LOCATIONS.Length; i++)
        {
            Location loc = levelManager.LOCATIONS[i];
            locList.Add(loc.ID);
            uIManager.GeneratePopUp(loc.ID, mainCamera.WorldToScreenPoint(loc.transform.position));
        }

        onSuccess(locList);
    }
}