using System;

using UnityEngine;
using UnityEngine.UI;

using TMPro;
using System.Collections.Generic;

public class TutorialEvents : MonoBehaviour
{
    [SerializeField] TMP_Text tutorialText;
    [SerializeField] GameManager gameManager;
    [SerializeField] LevelManager levelManager;

    [TextArea(3, 6)]
    [SerializeField] string[] sentences;
    [SerializeField] string criteria0to1;
    [SerializeField] string criteria1to2;
    [SerializeField] string criteria2to3;
    [SerializeField] string criteria3to4;
    //Criteria 4 to 5 is calling any HELP argument
    //Criteria 5 to 6 is the TutorialWave finish spawning.
    [SerializeField] string criteria6to7;

    private void OnEnable()
    {
        gameManager.OnChangeDirectory += Tutorial0;
    }

    private void OnDisable()
    {
        gameManager.OnChangeDirectory -= Tutorial0;
        gameManager.OnChangeDirectory -= Tutorial1;
        gameManager.OnInstallTower -= Tutorial2;
        gameManager.OnInstallTower -= Tutorial3;
        gameManager.OnHelpArgument -= Tutorial4;
        levelManager.OnWaveEnd -= Tutorial5;
        gameManager.OnUpdateTower -= Tutorial6;

    }

    private void Start()
    {
        tutorialText.text = sentences[0];
    }

    void Tutorial0(string locName)
    {
        if (locName == criteria0to1) {
            tutorialText.text = sentences[1];
            gameManager.OnChangeDirectory += Tutorial1;
            gameManager.OnChangeDirectory -= Tutorial0;
        }
    }

    void Tutorial1(string locName)
    {
        for (int i = 0; i < 3; i++)
        {
            if (locName != criteria1to2)
            {
                tutorialText.text = sentences[2];
                gameManager.OnChangeDirectory -= Tutorial1;
                gameManager.OnInstallTower += Tutorial2;
            }
        }
    }

    void Tutorial2(string towerId) {
        if (towerId == criteria2to3) {
            tutorialText.text = sentences[3];
            gameManager.OnInstallTower -= Tutorial2;
            gameManager.OnInstallTower += Tutorial3;
        }
    }

    void Tutorial3(string towerId)
    {
        if (towerId == criteria3to4)
        {
            tutorialText.text = sentences[4];
            gameManager.OnInstallTower -= Tutorial3;
            gameManager.OnHelpArgument += Tutorial4;
        }
    }

    void Tutorial4()
    {
        tutorialText.text = sentences[5];
        gameManager.OnHelpArgument -= Tutorial4;
        levelManager.OnWaveEnd += Tutorial5;
    }

    void Tutorial5(){
        tutorialText.text = sentences[6];
        levelManager.PauseWave();
        levelManager.OnWaveEnd -= Tutorial5;
        gameManager.OnUpdateTower += Tutorial6;
    }

    void Tutorial6(string towerName){
        if (towerName == criteria6to7) { 
            tutorialText.text = sentences[7];
            gameManager.OnUpdateTower -= Tutorial6;
        }
    }

}
