public class CommandManagerModel
{
    public CommandManager COMMAND_MANAGER { get; private set; }
    public TerminalManager TERMINAL_MANAGER { get; private set; }
    public WavesController WAVES_CONTROLLER { get; private set; }
    public MapHandler MAP_HANDLER { get; private set; }
    public TowersController TOWERS_CONTROLLER { get; private set; }
    public GameCurrenciesController CURRENCIES_CONTROLLER { get; private set; }

    public CommandManagerModel(CommandManager commandManager, TerminalManager terminal, WavesController wavesController, MapHandler mapHandler, TowersController towersController, GameCurrenciesController currencyController)
    {
        COMMAND_MANAGER = commandManager;
        TERMINAL_MANAGER = terminal;
        WAVES_CONTROLLER = wavesController;
        MAP_HANDLER = mapHandler;
        TOWERS_CONTROLLER = towersController;
        CURRENCIES_CONTROLLER = currencyController;
    }
}
