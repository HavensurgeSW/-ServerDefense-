using System;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "command_tutorial", menuName = "ScriptableObjects/Commands/Tutorial")]
public class TutorialCommandSO : CommandSO
{
    [Header("Tutorial Command Configuration")]
    [SerializeField] private TutorialData[] tutorials = null;

    public override void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<TerminalResponseSO> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
    {
        string tutorialId = arguments[0];

        for (int i = 0; i < tutorials.Length; i++)
        {
            TutorialData tutorial = tutorials[i];
            if (tutorial.TUTORIAL_ID == tutorialId)
            {
                onTriggerMessage(tutorial.TUTORIAL_LINES);
                onSuccess(this);
                return;
            }
        }

        onTriggerMessage(errorResponse);
        onFailure(this);
    }

    public override void TriggerHelpResponse(CommandManagerModel commandManagerModel, Action<TerminalResponseSO> onTriggerMessage)
    {
        TerminalManager terminal = commandManagerModel.TERMINAL_MANAGER;
        List<string> lines = new List<string>();
        lines.Add("TUTORIAL <Num. of page>");
        lines.Add("Prints out tutorial logs.");

        string lastLine = "NUMBER OF PAGES: ";

        for (int i = 0; i < tutorials.Length; i++)
        {
            lastLine += tutorials[i].TUTORIAL_ID;

            if (i != tutorials.Length - 1)
            {
                lastLine += ", ";
            }
        }

        lines.Add(lastLine);
        TerminalResponseSO response = terminal.GenerateCustomTerminalResponse(lines);
        onTriggerMessage(response);
        terminal.DeleteGeneratedTerminalResponse(response);
    }
}