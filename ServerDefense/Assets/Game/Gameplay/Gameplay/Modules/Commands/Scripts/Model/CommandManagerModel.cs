using UnityEngine;

using System;
using System.Collections.Generic;

using ServerDefense.Systems.Currencies;

public class CommandManagerModel
{
    public TerminalManager TERMINAL_MANAGER => terminal;
    public LevelManager LEVEL_MANAGER => levelManager;
    public MapHandler MAP_HANDLER => mapHandler;
    public TowersController TOWERS_CONTROLLER => towersController;
    public CurrenciesController CURRENCIES_CONTROLLER => currencyController;
    public UIManager UI_MANAGER => uiManager;
    public Camera MAIN_CAMERA => mainCamera;

    private TerminalManager terminal = null;
    private LevelManager levelManager = null;
    private MapHandler mapHandler = null;
    private TowersController towersController = null;
    private CurrenciesController currencyController = null;
    private UIManager uiManager = null;
    private Camera mainCamera = null;

    public Action<SCENE> OnChangeScene { get; private set; }
    public Func<List<CommandSO>> OnGetCommands { get; private set; }

    public CommandManagerModel(TerminalManager terminal, LevelManager levelManager, MapHandler mapHandler, TowersController towersController, CurrenciesController currencyController, UIManager uiManager, Camera mainCamera)
    {
        this.terminal = terminal;
        this.levelManager = levelManager;
        this.mapHandler = mapHandler;
        this.towersController = towersController;
        this.currencyController = currencyController;
        this.uiManager = uiManager;
        this.mainCamera = mainCamera;
    }

    public void SetCallbacks(Action<SCENE> onChangeScene, Func<List<CommandSO>> onGetCommands)
    {
        OnChangeScene = onChangeScene;
        OnGetCommands = onGetCommands;
    }
}
