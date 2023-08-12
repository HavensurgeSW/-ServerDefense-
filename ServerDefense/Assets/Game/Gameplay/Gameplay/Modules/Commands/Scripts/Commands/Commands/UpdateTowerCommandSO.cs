using System;
using System.Collections.Generic;

using UnityEngine;

using ServerDefense.Common.Currencies;

using ServerDefense.Gameplay.Gameplay.Modules.Towers;
using ServerDefense.Gameplay.Gameplay.Modules.Currencies;
using ServerDefense.Gameplay.Gameplay.Modules.Map;
using ServerDefense.Gameplay.Gameplay.Modules.Terminal;

namespace ServerDefense.Gameplay.Gameplay.Modules.Commands
{
    [CreateAssetMenu(fileName = "command_updatetower", menuName = "ScriptableObjects/Commands/UpdateTower")]
    public class UpdateTowerCommandSO : CommandSO
    {
        [Header("Update Command Configuration")]
        [SerializeField] private string deployId = string.Empty;
        [SerializeField] private string infoId = string.Empty;
        [SerializeField] private CurrencySO currencyToUse = null;
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
                onTriggerMessage(commandManagerModel.MAP_HANDLER.GetInvalidLocationResponse());
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
            TowerLevelSO[] towerLevelsData = towersController.GetTowerLevelsData(tower.ID);
            TowerLevelSO nextLevel = towerLevelsData[tower.NEXT_LEVEL - 1];

            if (keyword == infoId)
            {
                TriggerUpdateCommandInfo(tower, nextLevel, onTriggerMessage, onSuccess);
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
            TowerLevelSO[] towerLevelsData = towersController.GetTowerLevelsData(selectedTower.ID);

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

        private void TriggerUpdateCommandInfo(BaseTower tower, TowerLevelSO nextLevel, Action<TerminalResponseSO> onTriggerMessage, Action<CommandSO> onSuccess)
        {
            TerminalResponseSO response = TerminalResponseSO.CreateInstance(GetNextUpdateInfo(tower, nextLevel));
            onTriggerMessage(response);
            TerminalResponseSO.Destroy(response);
            onSuccess(this);
        }

        private void TriggerUpdateCommandDeploy(TowersController towerController, BaseTower tower, TowerLevelSO nextLevel, GameCurrenciesController currenciesController, Action<TerminalResponseSO> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
        {
            int packetAmount = currenciesController.GetCurrencyValue(currencyToUse);
            if (packetAmount < nextLevel.PRICE)
            {
                onTriggerMessage(currenciesController.GetInsufficientCurrencyResponse());
                onFailure(this);
                return;
            }

            currenciesController.SubstractCurrencyValue(currencyToUse, nextLevel.PRICE);
            towerController.UpgradeTower(tower, tower.NEXT_LEVEL);

            onTriggerMessage(successResponse);
            onSuccess(this);
        }

        public List<string> GetNextUpdateInfo(BaseTower tower, TowerLevelSO nextLevel)
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
}