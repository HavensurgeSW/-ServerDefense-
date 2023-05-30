using System;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "command_NETSTAT", menuName = "ScriptableObjects/Commands/NETSTAT")]
public class NETSTATCommandSO : CommandSO
{
    public override void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<TerminalResponseSO> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
    {
        MapHandler mapHandler = commandManagerModel.MAP_HANDLER;
        List<string> locList = new List<string>();

        for (int i = 0; i < mapHandler.LOCATIONS.Length; i++)
        {
            Location loc = mapHandler.LOCATIONS[i];
            locList.Add(loc.ID);
            mapHandler.GeneratePopUp(loc);
        }

        TerminalManager terminal = commandManagerModel.TERMINAL_MANAGER;
        TerminalResponseSO response = terminal.GenerateCustomTerminalResponse(locList);
        onTriggerMessage(response);
        terminal.DeleteGeneratedTerminalResponse(response);
        onSuccess(this);
    }
}