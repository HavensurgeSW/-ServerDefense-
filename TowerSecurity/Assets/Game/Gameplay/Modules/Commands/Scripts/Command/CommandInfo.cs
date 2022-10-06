using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Commands/BaseCommandInfo")]
public class CommandInfo : ScriptableObject
{
    [Header("Base Command Configuration")]
    [SerializeField] private string id;
    [SerializeField] private int argCount;
    [SerializeField] private List<string> succResponse = new List<string>();
    [SerializeField] private List<string> helpResponse = new List<string>();
    [SerializeField] private List<string> errorResponse = new List<string>();

    public string ID { get => id; }
    public int ARG_COUNT { get => argCount; }
    public List<string> SUCC_RESPONSE { get => succResponse; }
    public List<string> HELP_RESPONSE { get => helpResponse; }
    public List<string> ERROR_RESPONSE { get => errorResponse; }
}
