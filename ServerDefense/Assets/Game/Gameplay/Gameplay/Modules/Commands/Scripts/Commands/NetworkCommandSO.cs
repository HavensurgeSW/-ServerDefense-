using System;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "command_network", menuName = "ScriptableObjects/Commands/Network")]
public class NetworkCommandSO : CommandSO
{
    [Header("network Command Configuration")]
    [SerializeField] private string initId = string.Empty;

    public override void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<List<string>> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
    {
        string keyword = arguments[0];

        if (keyword == initId)
        {
            LevelManager levelManager = commandManagerModel.LEVEL_MANAGER;
            levelManager.BeginWave(levelManager.GetCurrentWaveIndex());
            onTriggerMessage(successResponse);
            onSuccess(this);
            return;
        }

        onTriggerMessage(errorResponse);
        onFailure(this);
    }
}