using System;

using UnityEngine;

[Serializable]
public class TutorialData
{
    [SerializeField] private string tutorialId = string.Empty;
    [SerializeField] private TerminalResponseSO tutorialLines = null;

    public string TUTORIAL_ID { get => tutorialId; }
    public TerminalResponseSO TUTORIAL_LINES { get => tutorialLines; }
}