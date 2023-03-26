using UnityEngine;

using ServerDefense.Systems.Currencies;

public class CommandManagerModel
{
    public CommandManager COMMAND_MANAGER => commandManager;
    public TerminalManager TERMINAL_MANAGER => terminal;
    public LevelManager LEVEL_MANAGER => levelManager;
    public MapHandler MAP_HANDLER => mapHandler;
    public TowersController TOWERS_CONTROLLER => towersController;
    public CurrenciesController CURRENCIES_CONTROLLER => currencyController;
    public UIManager UI_MANAGER => uiManager;
    public Camera MAIN_CAMERA => mainCamera;

    private CommandManager commandManager = null;
    private TerminalManager terminal = null;
    private LevelManager levelManager = null;
    private MapHandler mapHandler = null;
    private TowersController towersController = null;
    private CurrenciesController currencyController = null;
    private UIManager uiManager = null;
    private Camera mainCamera = null;

    public CommandManagerModel(CommandManager commandManager, TerminalManager terminal, LevelManager levelManager, MapHandler mapHandler, TowersController towersController, CurrenciesController currencyController, UIManager uiManager, Camera mainCamera)
    {
        this.commandManager = commandManager;
        this.terminal = terminal;
        this.levelManager = levelManager;
        this.mapHandler = mapHandler;
        this.towersController = towersController;
        this.currencyController = currencyController;
        this.uiManager = uiManager;
        this.mainCamera = mainCamera;
    }
}
