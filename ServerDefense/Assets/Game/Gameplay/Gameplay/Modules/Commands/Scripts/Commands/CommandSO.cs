using System;
using System.Collections.Generic;

using UnityEngine;

public abstract class CommandSO : ScriptableObject
{
    [Header("Base Command Configuration")]
    [SerializeField] protected string commandId = null;
    [SerializeField] protected int argumentsCount = 0;
    [SerializeField] protected TerminalResponseSO successResponse = null;
    [SerializeField] protected TerminalResponseSO helpResponse = null;
    [SerializeField] protected TerminalResponseSO errorResponse = null;

    public string COMMAND_ID { get => commandId; }
    public int ARGUMENTS_COUNT { get => argumentsCount; }
    public TerminalResponseSO HELP_RESPONSE { get => helpResponse; }

    protected TerminalResponseSO GenerateCustomTerminalResponse(List<string> lines)
    {
        TerminalResponseSO responseSO = ScriptableObject.CreateInstance<TerminalResponseSO>();
        responseSO.OverrideResponse(lines);
        return responseSO;
    }

    protected void DeleteGeneratedTerminalResponse(TerminalResponseSO response)
    {
        response.ClearResponses();
        ScriptableObject.Destroy(response);
    }

    public abstract void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<TerminalResponseSO> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure);
}
