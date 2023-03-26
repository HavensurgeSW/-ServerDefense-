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

    public override void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<List<string>> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
    {
        string keyword = arguments[0];

        if(!HasNecessaryDataForUpdate(commandManagerModel, keyword, out List<string> errorResponse))
        {
            onTriggerMessage(errorResponse);
            onFailure(this);
            return;
        }

        TowersController towersController = commandManagerModel.TOWERS_CONTROLLER;

        BaseTower tower = commandManagerModel.MAP_HANDLER.CURRENT_LOCATION.TOWER;
        TowerLevelData[] towerLevelsData = towersController.GetTowerLevelsData(tower.ID);
        TowerLevelData nextLevel = towerLevelsData[tower.NEXT_LEVEL - 1];

        if (keyword == infoId)
        {
            onTriggerMessage(GetNextUpdateInfo(tower, nextLevel));
            onSuccess(this);
        }
        else if (keyword == deployId)
        {
            TriggerUpdateCommandDeploy(towersController, tower, nextLevel, commandManagerModel.CURRENCIES_CONTROLLER, onTriggerMessage, onSuccess, onFailure);
        }
    }

    private bool HasNecessaryDataForUpdate(CommandManagerModel commandManagerModel, string keyword, out List<string> response)
    {
        response = new List<string>();

        MapHandler mapHandler = commandManagerModel.MAP_HANDLER;
        BaseTower selectedTower = mapHandler.CURRENT_LOCATION.TOWER;
        bool isCorrectKeyword = keyword == infoId || keyword == deployId;
        bool hasAvailableTower = selectedTower != null;

        if (!isCorrectKeyword || !hasAvailableTower)
        {
            response = errorResponse;
            return false;
        }

        bool isLocationOccupied = !mapHandler.GetIsCurrentLocationAvailable();

        if(!isLocationOccupied)
        {
            response = invalidLocationResponse;
            return false;
        }

        TowersController towersController = commandManagerModel.TOWERS_CONTROLLER;
        TowerLevelData[] towerLevelsData = towersController.GetTowerLevelsData(selectedTower.ID);

        if (selectedTower.NEXT_LEVEL > towerLevelsData.Length)
        {
            response = maxLevelResponse;
            return false;
        }

        return true;
    }

    private void TriggerUpdateCommandDeploy(TowersController towerController, BaseTower tower, TowerLevelData nextLevel, CurrenciesController currenciesController, Action<List<string>> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
    {
        int packetAmount = currenciesController.GetCurrencyValue(CurrencyConstants.packetCurrency);
        if (packetAmount < nextLevel.PRICE)
        {
            onTriggerMessage(insufficientFundsResponse);
            onFailure(this);
            return;
        }

        currenciesController.SubstractCurrencyValue(CurrencyConstants.packetCurrency, nextLevel.PRICE);
        towerController.UpgradeTower(tower, tower.NEXT_LEVEL);

        onTriggerMessage(successResponse);
        onSuccess(this);
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