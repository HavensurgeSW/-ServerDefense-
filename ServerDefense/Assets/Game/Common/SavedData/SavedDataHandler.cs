using ServerDefense.Tools.Singleton;

public class SavedDataHandler : Singleton<SavedDataHandler>
{
    private bool gameStatus = false;
    private SCENE selectedScene = SCENE.NONE;

    public bool GameStatus { get => gameStatus; }
    public SCENE SelectedScene { get => selectedScene; }

    public void SetGameWonStatus(bool status)
    {
        gameStatus = status;
    }

    public void SetSelectedScene(SCENE scene)
    {
        selectedScene = scene;
    }

    public void ResetData()
    {
        SetGameWonStatus(false);
        SetSelectedScene(SCENE.NONE);
    }
}
