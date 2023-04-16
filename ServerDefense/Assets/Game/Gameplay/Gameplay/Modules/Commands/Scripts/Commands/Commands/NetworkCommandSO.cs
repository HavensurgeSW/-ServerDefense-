using System;

using UnityEngine;

[CreateAssetMenu(fileName = "command_network", menuName = "ScriptableObjects/Commands/Network")]
public class NetworkCommandSO : CommandSO
{
    [Header("Network Command Configuration")]
    [SerializeField] private string initId = string.Empty;

    public override void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<TerminalResponseSO> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
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