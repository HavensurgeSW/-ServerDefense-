using UnityEngine;

using TMPro;

public class TutorialEvents : MonoBehaviour
{
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private CommandManager commandManager;
    [SerializeField] private LevelManager levelManager;

    [TextArea(3, 10)]
    [SerializeField] private string[] sentences;
    [SerializeField] private string criteria0to1;
    [SerializeField] private string criteria1to2;
    [SerializeField] private string criteria2to3;
    [SerializeField] private string criteria3to4;
    //Criteria 4 to 5 is calling any HELP argument
    //Criteria 5 to 6 is the TutorialWave finish spawning.
    [SerializeField] private string criteria6to7;

    private void Start()
    {
        tutorialText.text = sentences[0];
    }

    private void OnEnable()
    {
        commandManager.OnChangeDirectory += Tutorial0;
    }

    private void OnDisable()
    {
        commandManager.OnChangeDirectory -= Tutorial0;
        commandManager.OnChangeDirectory -= Tutorial1;
        commandManager.OnInstallTower -= Tutorial2;
        commandManager.OnInstallTower -= Tutorial3;
        commandManager.OnHelpArgument -= Tutorial4;
        levelManager.RemoveOnWaveEndCallback(Tutorial5);
        commandManager.OnUpdateTower -= Tutorial6;
    }

    private void Tutorial0(string locName)
    {
        if (locName == criteria0to1)
        {
            tutorialText.text = sentences[1];
            commandManager.OnChangeDirectory += Tutorial1;
            commandManager.OnChangeDirectory -= Tutorial0;
        }
    }

    private void Tutorial1(string locName)
    {
        for (int i = 0; i < 3; i++)
        {
            if (locName != criteria1to2)
            {
                tutorialText.text = sentences[2];
                commandManager.OnChangeDirectory -= Tutorial1;
                commandManager.OnInstallTower += Tutorial2;
            }
        }
    }

    private void Tutorial2(string towerId)
    {
        if (towerId == criteria2to3)
        {
            tutorialText.text = sentences[3];
            commandManager.OnInstallTower -= Tutorial2;
            commandManager.OnInstallTower += Tutorial3;
        }
    }

    private void Tutorial3(string towerId)
    {
        if (towerId == criteria3to4)
        {
            tutorialText.text = sentences[4];
            commandManager.OnInstallTower -= Tutorial3;
            commandManager.OnHelpArgument += Tutorial4;
        }
    }

    private void Tutorial4()
    {
        tutorialText.text = sentences[5];
        commandManager.OnHelpArgument -= Tutorial4;
        levelManager.AddOnWaveEndCallback(Tutorial5);
    }

    private void Tutorial5()
    {
        tutorialText.text = sentences[6];
        //levelManager.PauseWave();
        levelManager.RemoveOnWaveEndCallback(Tutorial5);
        commandManager.OnUpdateTower += Tutorial6;
    }

    private void Tutorial6(string towerName)
    {
        if (towerName == criteria6to7)
        {
            tutorialText.text = sentences[7];
            commandManager.OnUpdateTower -= Tutorial6;
        }
    }
}