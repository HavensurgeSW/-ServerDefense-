using System;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "command_network", menuName = "ScriptableObjects/Commands/Network")]
public class NetworkCommandSO : CommandSO
{
    [Header("network Command Configuration")]
    [SerializeField] private string initId = string.Empty;

    public override void TriggerCommand(CommandManager commandManager, string[] arguments, Action<List<string>> onSuccess, Action<List<string>> onFailure)
    {
        string keyword = arguments[0];

        if (keyword == initId)
        {
            LevelManager levelManager = commandManager.GetLevelManager();
            levelManager.BeginWave(levelManager.GetCurrentWaveIndex());
            onSuccess(successResponse);
            return;
        }

        onFailure(errorResponse);
    }
}