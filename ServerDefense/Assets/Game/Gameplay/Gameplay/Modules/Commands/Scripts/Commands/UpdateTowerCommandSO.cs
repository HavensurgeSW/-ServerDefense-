using System;
using System.Collections.Generic;

using UnityEngine;

using ServerDefense.Systems.Currencies;

[CreateAssetMenu(fileName = "command_updatetower", menuName = "ScriptableObjects/Commands/UpdateTower")]
public class UpdateTowerCommandSO : CommandSO
{
    [Header("Update Command Configuration")]
    [SerializeField] private string deployId = string.Empty;
    [SerializeField] private string infoId = string.Empty;
    [SerializeField] private List<string> invalidLocationResponse = null;
    [SerializeField] private List<string> insufficientFundsResponse = null;
    [SerializeField] private List<string> maxLevelResponse = null;

    public override void TriggerCommand(CommandManager commandManager, string[] arguments, Action<List<string>> onSuccess, Action<List<string>> onFailure)
    {
        string keyword = arguments[0];

        if(!HasNecessaryDataForUpdate(commandManager, keyword, out List<string> errorResponse))
        {
            onFailure(errorResponse);
            return;
        }

        TowersController towersController = commandManager.GetTowersController();

        BaseTower tower = commandManager.GetMapHandler().CURRENT_LOCATION.TOWER;
        TowerLevelData[] towerLevelsData = towersController.GetTowerLevelsData(tower.ID);
        TowerLevelData nextLevel = towerLevelsData[tower.NEXT_LEVEL - 1];

        if (keyword == infoId)
        {
            onSuccess(GetNextUpdateInfo(tower, nextLevel));
        }
        else if (keyword == deployId)
        {
            TriggerUpdateCommandDeploy(towersController, tower, nextLevel, commandManager.GetCurrencyController(), onSuccess, onFailure);
        }
    }

    private bool HasNecessaryDataForUpdate(CommandManager commandManager, string keyword, out List<string> response)
    {
        response = new List<string>();

        MapHandler mapHandler = commandManager.GetMapHandler();
        BaseTower selectedTower = mapHandler.CURRENT_LOCATION.TOWER;
        bool isCorrectKeyword = keyword == infoId || keyword == deployId;
        bool isLocationAvailable = mapHandler.GetIsCurrentLocationAvailable();
        bool hasAvailableTower = selectedTower != null;

        if (!isCorrectKeyword || !isLocationAvailable || !hasAvailableTower)
        {
            response = errorResponse;
            return false;
        }

        TowersController towersController = commandManager.GetTowersController();
        TowerLevelData[] towerLevelsData = towersController.GetTowerLevelsData(selectedTower.ID);

        if (selectedTower.NEXT_LEVEL > towerLevelsData.Length)
        {
            response = maxLevelResponse;
            return false;
        }

        return true;
    }

    private void TriggerUpdateCommandDeploy(TowersController towerController, BaseTower tower, TowerLevelData nextLevel, CurrenciesController currenciesController, Action<List<string>> onSuccess, Action<List<string>> onFailure)
    {
        int packetAmount = currenciesController.GetCurrencyValue(CurrencyConstants.packetCurrency);
        if (packetAmount < nextLevel.PRICE)
        {
            onFailure(insufficientFundsResponse);
            return;
        }

        currenciesController.SubstractCurrencyValue(CurrencyConstants.packetCurrency, nextLevel.PRICE);
        towerController.UpgradeTower(tower, tower.NEXT_LEVEL);

        onSuccess(successResponse);
    }

    public List<string> GetNextUpdateInfo(BaseTower tower, TowerLevelData nextLevel)
    {
        List<string> toReturn = new List<string>
        {
            "KB Cost: " + nextLevel.PRICE,
            "Damage: " + tower.DAMAGE + " -> " + nextLevel.STATS.DAMAGE,
            "Range: " + tower.RANGE + " -> " + nextLevel.STATS.RANGE,
            "Fire Rate: " + tower.FIRE_RATE + " -> " + nextLevel.STATS.FIRE_RATE
        };

        return toReturn;
    }
}