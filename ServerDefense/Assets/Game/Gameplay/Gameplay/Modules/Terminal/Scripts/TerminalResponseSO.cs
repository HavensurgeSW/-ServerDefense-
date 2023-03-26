using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "response_", menuName = "ScriptableObjects/Terminal/Response")]
public class TerminalResponseSO : ScriptableObject
{
    [SerializeField] private List<string> response = new List<string>();

    public List<string> RESPONSE => response;
}
