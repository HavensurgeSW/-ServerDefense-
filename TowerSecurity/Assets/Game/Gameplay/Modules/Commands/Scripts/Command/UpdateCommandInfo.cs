
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Commands/UpdateCommandInfo", fileName = "UPDATE_Command")]

public class UpdateCommandInfo : CommandInfo
{
    [Header("Update Command Configuration")]
    [SerializeField] private List<string> invalidLocationResponse = null;
    [SerializeField] private List<string> insufficientFundsResponse = null;

    public List<string> INVALID_LOCATION_RESPONSE { get => invalidLocationResponse; }
    public List<string> INSUFFICIENT_FUNDS_RESPONSE { get => insufficientFundsResponse; }
}
