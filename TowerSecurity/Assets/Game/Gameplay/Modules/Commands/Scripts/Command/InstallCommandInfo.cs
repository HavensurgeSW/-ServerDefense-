using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Commands/InstallCommandInfo", fileName = "INSTALL_Command")]
public class InstallCommandInfo : CommandInfo
{
    [Header("Install Command Configuration")]
    [SerializeField] private List<string> invalidLocationResponse = null;
    [SerializeField] private List<string> invalidTowerIdResponse = null;
    [SerializeField] private List<string> insufficientFundsResponse = null;

    public List<string> INVALID_LOCATION_RESPONSE { get => invalidLocationResponse; }
    public List<string> INVALID_TOWER_ID_RESPONSE { get => invalidTowerIdResponse; }
    public List<string> INSUFFICIENT_FUNDS_RESPONSE { get => insufficientFundsResponse; }
}
