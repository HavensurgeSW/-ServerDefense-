using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Command
{
    [Tooltip("For inspector debug purposes")]
    [SerializeField] private string name = string.Empty;
    [SerializeField] private CommandInfo info = null;
    [SerializeField] private UnityEvent<string[], CommandInfo> callback = null;

    public CommandInfo INFO { get => info; }
    public UnityEvent<string[], CommandInfo> CALLBACK { get => callback; }
}
