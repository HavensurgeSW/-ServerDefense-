using System;

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
}