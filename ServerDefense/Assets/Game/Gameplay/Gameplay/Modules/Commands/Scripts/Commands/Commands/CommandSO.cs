using System;
using System.Reflection;

using UnityEditor;
using UnityEngine;

public abstract class CommandSO : ScriptableObject
{
    [Header("Base Command Configuration")]
    [SerializeField] protected string commandId = string.Empty;
    [SerializeField] protected int argumentsCount = 0;
    [SerializeField] protected TerminalResponseSO successResponse = null;
    [SerializeField] protected TerminalResponseSO helpResponse = null;
    [SerializeField] protected TerminalResponseSO errorResponse = null;

    public string COMMAND_ID { get => commandId; }
    public int ARGUMENTS_COUNT { get => argumentsCount; }

    public virtual void TriggerHelpResponse(CommandManagerModel commandManagerModel, Action<TerminalResponseSO> onTriggerMessage)
    {
        onTriggerMessage(helpResponse);
    }

    public abstract void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<TerminalResponseSO> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure);
}
