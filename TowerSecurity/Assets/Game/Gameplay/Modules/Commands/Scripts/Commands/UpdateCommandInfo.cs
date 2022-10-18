using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Commands/UpdateCommandInfo", fileName = "UPDATE_Command")]

public class UpdateCommandInfo : CommandInfo
{
    [Header("Update Command Configuration")]
    [SerializeField] private string deployId = string.Empty;
    [SerializeField] private string infoId = string.Empty;
    
    [SerializeField] private List<string> invalidLocationResponse = null;
    [SerializeField] private List<string> insufficientFundsResponse = null;
    [SerializeField] private List<string> maxLevelResponse = null;

    public string DeployId { get => deployId; }
    public string InfoId { get => infoId; }
    public List<string> INVALID_LOCATION_RESPONSE { get => invalidLocationResponse; }
    public List<string> INSUFFICIENT_FUNDS_RESPONSE { get => insufficientFundsResponse; }
    public List<string> MAX_LEVEL_RESPONSE { get => maxLevelResponse; }

    public List<string> GetNextUpdateData(BaseTower tower, BaseTowerLevelData nextLevel)
    {
        List<string> toReturn = new List<string>();
        
        toReturn.Add("Damage: " + tower.DAMAGE + " -> " + nextLevel.DAMAGE);
        toReturn.Add("Range: " + tower.RANGE + " -> " + nextLevel.RANGE);
        toReturn.Add("Fire Rate: " + tower.FIRE_RATE + " -> " + nextLevel.FIRE_RATE);

        return toReturn;
    }
}
