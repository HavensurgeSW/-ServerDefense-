using UnityEngine;

using ServerDefense.Systems.Currencies;

public class CommandManagerModel
{
    public CommandManager COMMAND_MANAGER { get; private set; }
    public TerminalManager TERMINAL_MANAGER { get; private set; }
    public LevelManager LEVEL_MANAGER { get; private set; }
    public MapHandler MAP_HANDLER { get; private set; }
    public TowersController TOWERS_CONTROLLER { get; private set; }
    public GameCurrenciesController CURRENCIES_CONTROLLER { get; private set; }
    public UIManager UI_MANAGER { get; private set; }
    public Camera MAIN_CAMERA { get; private set; }

    public CommandManagerModel(CommandManager commandManager, TerminalManager terminal, LevelManager levelManager, MapHandler mapHandler, TowersController towersController, GameCurrenciesController currencyController, UIManager uiManager, Camera mainCamera)
    {
        COMMAND_MANAGER = commandManager;
        TERMINAL_MANAGER = terminal;
        LEVEL_MANAGER = levelManager;
        MAP_HANDLER = mapHandler;
        TOWERS_CONTROLLER = towersController;
        CURRENCIES_CONTROLLER = currencyController;
        UI_MANAGER = uiManager;
        MAIN_CAMERA = mainCamera;
    }
}
