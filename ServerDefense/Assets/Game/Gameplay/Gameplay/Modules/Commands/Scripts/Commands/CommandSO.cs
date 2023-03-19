using System;
using System.Collections.Generic;

using UnityEngine;

public abstract class CommandSO : ScriptableObject
{
    [Header("Base Command Configuration")]
    [SerializeField] protected string commandId = null;
    [SerializeField] protected int argumentsCount = 0;
    [SerializeField] protected List<string> successResponse = null;
    [SerializeField] protected List<string> helpResponse = null;
    [SerializeField] protected List<string> errorResponse = null;

    public string COMMAND_ID { get => commandId; }
    public int ARGUMENTS_COUNT { get => argumentsCount; }
    public List<string> SUCCESS_RESPONSE { get => successResponse; }
    public List<string> HELP_RESPONSE { get => helpResponse; }
    public List<string> ERROR_RESPONSE { get => errorResponse; }

    public abstract void TriggerCommand(CommandManager commandManager, string[] arguments, Action<List<string>> onSuccess, Action<List<string>> onFailure);
}
