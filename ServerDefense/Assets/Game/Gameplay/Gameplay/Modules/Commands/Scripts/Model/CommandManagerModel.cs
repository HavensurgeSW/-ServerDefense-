public class CommandManagerModel
{
    public CommandManager COMMAND_MANAGER => commandManager;
    public TerminalManager TERMINAL_MANAGER => terminal;
    public WavesController WAVES_CONTROLLER => wavesController;
    public MapHandler MAP_HANDLER => mapHandler;
    public TowersController TOWERS_CONTROLLER => towersController;
    public GameCurrenciesController CURRENCIES_CONTROLLER => currencyController;

    private CommandManager commandManager = null;
    private TerminalManager terminal = null;
    private WavesController wavesController = null;
    private MapHandler mapHandler = null;
    private TowersController towersController = null;
    private GameCurrenciesController currencyController = null;

    public CommandManagerModel(CommandManager commandManager, TerminalManager terminal, WavesController wavesController, MapHandler mapHandler, TowersController towersController, GameCurrenciesController currencyController)
    {
        this.commandManager = commandManager;
        this.terminal = terminal;
        this.wavesController = wavesController;
        this.mapHandler = mapHandler;
        this.towersController = towersController;
        this.currencyController = currencyController;
    }
}
