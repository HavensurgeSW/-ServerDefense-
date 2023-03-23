using System;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "command_tutorial", menuName = "ScriptableObjects/Commands/Tutorial")]
public class TutorialCommandSO : CommandSO
{
    [Serializable]
    public class TutorialData
    {
        [SerializeField] private string tutorialId = string.Empty;
        [SerializeField] private List<string> tutorialLines = null;

        public string TUTORIAL_ID { get => tutorialId; }
        public List<string> TUTORIAL_LINES { get => tutorialLines; }
    }

    [Header("Tutorial Command Configuration")]
    [SerializeField] private TutorialData[] tutorials = null;

    public TutorialData[] TUTORIALS { get => tutorials; }

    public override void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<List<string>> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
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