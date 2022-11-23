using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Commands/TutorialCommandInfo")]
public class TutorialCommandInfo : CommandInfo
{
    [System.Serializable]
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
}
