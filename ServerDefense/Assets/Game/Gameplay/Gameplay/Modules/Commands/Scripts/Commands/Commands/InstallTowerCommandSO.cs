using System;

using UnityEngine;

using ServerDefense.Common.Currencies;

[CreateAssetMenu(fileName = "command_install_", menuName = "ScriptableObjects/Commands/Installs/InstallTower")]
public class InstallTowerCommandSO : CommandSO
{
    [Header("Install Command Configuration")]
    [SerializeField] private CurrencySO currencyToConsume = null;
    [SerializeField] private TerminalResponseSO invalidTowerIdResponse = null;

    public override void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<TerminalResponseSO> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
    {
        TerminalManager terminal = commandManagerModel.TERMINAL_MANAGER;
        MapHandler mapHandler = commandManagerModel.MAP_HANDLER;
        TowersController towersController = commandManagerModel.TOWERS_CONTROLLER;
        terminal.ClearCmdEntries();        

        if (!mapHandler.GetIsCurrentLocationAvailable())
        {
            onTriggerMessage(mapHandler.GetInvalidLocationResponse());
            onFailure(this);
            return;
        }

        string towerId = arguments[0];
        if (!towersController.DoesTowerIdExist(towerId))
        {
            onTriggerMessage(invalidTowerIdResponse);
            onFailure(this);
            return;
        }

        TowerSO data = towersController.GetTowerData(towerId);

        GameCurrenciesController currenciesController = commandManagerModel.CURRENCIES_CONTROLLER;
        if (currenciesController.GetCurrencyValue(currencyToConsume.CURRENCY_ID) < data.LEVELS[0].PRICE)
        {
            onTriggerMessage(currenciesController.GetInsufficientCurrencyResponse());
            onFailure(this);
            return;
        }

        currenciesController.SubstractCurrencyValue(currencyToConsume.CURRENCY_ID, data.LEVELS[0].PRICE);

        Location currentLoc = mapHandler.CURRENT_LOCATION;
        BaseTower actualTower = towersController.GenerateTower(towerId, currentLoc.transform);

        actualTower.SetPosition(currentLoc.transform.position + (Vector3)data.OFFSET);
        currentLoc.SetTower(actualTower);
        currentLoc.SetAvailable(false);

        //OnInstallTower?.Invoke(towerId);

        onTriggerMessage(successResponse);
        onSuccess(this);
    }
}
