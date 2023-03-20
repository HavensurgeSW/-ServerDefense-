using System;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "command_help", menuName = "ScriptableObjects/Commands/Help")]
public class HelpCommandSO : CommandSO
{
    [Header("Help Command Configuration")]
    [SerializeField] private string[] helpKeywords = null;

    public string[] HELP_KEYWORDS => helpKeywords;

    public override void TriggerCommand(CommandManager commandManager, string[] arguments, Action<List<string>> onSuccess, Action<List<string>> onFailure)
    {
    }
}