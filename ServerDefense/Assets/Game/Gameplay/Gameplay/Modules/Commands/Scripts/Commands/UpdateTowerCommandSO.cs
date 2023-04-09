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
    [SerializeField] private TerminalResponseSO invalidLocationResponse = null;
    [SerializeField] private TerminalResponseSO maxLevelResponse = null;

    public override void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<TerminalResponseSO> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
    {
        string keyword = arguments[0];

        if (!HasDefaultDataToTrigger(commandManagerModel, keyword))
        {
            onTriggerMessage(errorResponse);
            onFailure(this);
            return;
        }

        if (commandManagerModel.MAP_HANDLER.GetIsCurrentLocationAvailable())
        {
            onTriggerMessage(invalidLocationResponse);
            onFailure(this);
            return;
        }

        if (IsTowerMaxLevel(commandManagerModel))
        {
            onTriggerMessage(maxLevelResponse);
            onFailure(this);
            return;
        }

        TowersController towersController = commandManagerModel.TOWERS_CONTROLLER;

        BaseTower tower = commandManagerModel.MAP_HANDLER.CURRENT_LOCATION.TOWER;
        TowerLevelData[] towerLevelsData = towersController.GetTowerLevelsData(tower.ID);
        TowerLevelData nextLevel = towerLevelsData[tower.NEXT_LEVEL - 1];

        if (keyword == infoId)
        {
            TriggerUpdateCommandInfo(commandManagerModel.TERMINAL_MANAGER, tower, nextLevel, onTriggerMessage, onSuccess);
        }
        else if (keyword == deployId)
        {
            TriggerUpdateCommandDeploy(towersController, tower, nextLevel, commandManagerModel.CURRENCIES_CONTROLLER, onTriggerMessage, onSuccess, onFailure);
        }
    }

    private bool IsTowerMaxLevel(CommandManagerModel commandManagerModel)
    {
        MapHandler mapHandler = commandManagerModel.MAP_HANDLER;
        BaseTower selectedTower = mapHandler.CURRENT_LOCATION.TOWER;

        TowersController towersController = commandManagerModel.TOWERS_CONTROLLER;
        TowerLevelData[] towerLevelsData = towersController.GetTowerLevelsData(selectedTower.ID);

        return selectedTower.NEXT_LEVEL > towerLevelsData.Length;
    }

    private bool HasDefaultDataToTrigger(CommandManagerModel commandManagerModel, string keyword)
    {
        MapHandler mapHandler = commandManagerModel.MAP_HANDLER;
        BaseTower selectedTower = mapHandler.CURRENT_LOCATION.TOWER;
        bool isCorrectKeyword = keyword == infoId || keyword == deployId;
        bool hasAvailableTower = selectedTower != null;

        return isCorrectKeyword && hasAvailableTower;
    }

    private void TriggerUpdateCommandInfo(TerminalManager terminalManager, BaseTower tower, TowerLevelData nextLevel, Action<TerminalResponseSO> onTriggerMessage, Action<CommandSO> onSuccess)
    {
        TerminalResponseSO response = terminalManager.GenerateCustomTerminalResponse(GetNextUpdateInfo(tower, nextLevel));
        onTriggerMessage(response);
        terminalManager.DeleteGeneratedTerminalResponse(response);
        onSuccess(this);
    }

    private void TriggerUpdateCommandDeploy(TowersController towerController, BaseTower tower, TowerLevelData nextLevel, CurrenciesController currenciesController, Action<TerminalResponseSO> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
    {
        int packetAmount = currenciesController.GetCurrencyValue(CurrencyConstants.packetCurrency);
        if (packetAmount < nextLevel.PRICE)
        {
            onTriggerMessage(currenciesController.GetInsufficientCurrencyResponse());
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