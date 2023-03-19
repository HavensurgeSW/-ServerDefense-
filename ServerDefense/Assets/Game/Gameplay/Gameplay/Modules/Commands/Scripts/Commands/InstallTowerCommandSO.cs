using System;
using System.Collections.Generic;

using UnityEngine;

using ServerDefense.Systems.Currencies;

[CreateAssetMenu(fileName = "command_install_", menuName = "ScriptableObjects/Commands/Installs/InstallTower")]
public class InstallTowerCommandSO : CommandSO
{
    [Header("Install Command Configuration")]
    [SerializeField] private List<string> invalidLocationResponse = null;
    [SerializeField] private List<string> invalidTowerIdResponse = null;
    [SerializeField] private List<string> insufficientFundsResponse = null;

    public List<string> INVALID_LOCATION_RESPONSE { get => invalidLocationResponse; }
    public List<string> INVALID_TOWER_ID_RESPONSE { get => invalidTowerIdResponse; }
    public List<string> INSUFFICIENT_FUNDS_RESPONSE { get => insufficientFundsResponse; }

    public override void TriggerCommand(CommandManager commandManager, string[] arguments, Action<List<string>> onSuccess, Action<List<string>> onFailure)
    {
        TerminalManager terminal = commandManager.GetTerminalManager();
        MapHandler mapHandler = commandManager.GetMapHandler();
        TowersController towersController = commandManager.GetTowersController();

        terminal.ClearCmdEntries();

        if (!mapHandler.GetIsCurrentLocationAvailable())
        {
            onFailure(invalidLocationResponse);
            return;
        }

        string towerId = arguments[0];

        if (!towersController.DoesTowerIdExist(towerId))
        {
            onFailure(invalidTowerIdResponse);
            return;
        }

        TowerData data = towersController.GetTowerData(towerId);

        CurrenciesController currenciesController = commandManager.GetCurrencyController();
        if (currenciesController.GetCurrencyValue(CurrencyConstants.packetCurrency) < data.LEVELS[0].PRICE)
        {
            onFailure(insufficientFundsResponse);
            return;
        }

        currenciesController.SubstractCurrencyValue(CurrencyConstants.packetCurrency, data.LEVELS[0].PRICE);

        Location currentLoc = mapHandler.CURRENT_LOCATION;
        BaseTower actualTower = towersController.GenerateTower(towerId, currentLoc.transform);

        actualTower.SetPosition(currentLoc.transform.position + (Vector3)data.OFFSET);
        currentLoc.SetTower(actualTower);
        currentLoc.SetAvailable(false);

        //OnInstallTower?.Invoke(towerId);

        onSuccess(successResponse);
    }
}
