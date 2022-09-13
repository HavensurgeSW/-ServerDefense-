using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Commands/HelpCommandInfo")]
public class HelpCommandInfo : CommandInfo
{
    [SerializeField] private string[] helpKeywords = null;

    public string[] HELPKEYWORDS => helpKeywords;
}
