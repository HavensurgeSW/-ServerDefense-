using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Command
{
    [SerializeField] private CommandInfo info;
    [SerializeField] private UnityEvent<string[], CommandInfo> callback;

    public CommandInfo INFO { get => info; }
    public UnityEvent<string[], CommandInfo> CALLBACK { get => callback; }
}
