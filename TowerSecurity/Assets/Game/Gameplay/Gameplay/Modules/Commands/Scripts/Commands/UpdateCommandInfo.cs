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

    public string DEPLOY_ID { get => deployId; }
    public string INFO_ID { get => infoId; }
    public List<string> INVALID_LOCATION_RESPONSE { get => invalidLocationResponse; }
    public List<string> INSUFFICIENT_FUNDS_RESPONSE { get => insufficientFundsResponse; }
    public List<string> MAX_LEVEL_RESPONSE { get => maxLevelResponse; }

    public List<string> GetNextUpdateInfo(BaseTower tower, TowerLevelData nextLevel)
    {
        List<string> toReturn = new List<string>();

        toReturn.Add("KB Cost: " + nextLevel.PRICE);
        toReturn.Add("Damage: " + tower.DAMAGE + " -> " + nextLevel.STATS.DAMAGE);
        toReturn.Add("Range: " + tower.RANGE + " -> " + nextLevel.STATS.RANGE);
        toReturn.Add("Fire Rate: " + tower.FIRE_RATE + " -> " + nextLevel.STATS.FIRE_RATE);

        return toReturn;
    }
}