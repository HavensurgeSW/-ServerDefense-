using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Commands/BaseCommandInfo")]
public class CommandInfo : ScriptableObject {
    [SerializeField]private string id;
    [SerializeField]private int argCount;
    [SerializeField]private List<string> succResponse = new List<string>();
    [SerializeField]private List<string> helpResponse = new List<string>();
    [SerializeField]private List<string> errorResponse = new List<string>();

    public string ID { get=>id;}
    public int ARGCOUNT { get => argCount; }
    public List<string> SUCCRESPONSE { get=>succResponse;}
    public List<string> HELPRESPONSE { get => helpResponse; }
    public List<string> ERRORRESPONSE { get => errorResponse; }
}
