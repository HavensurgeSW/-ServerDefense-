using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Commands/HelpCommandInfo")]
public class HelpCommandInfo : CommandInfo
{
    [Header("Help Command Configuration")]
    [SerializeField] private string[] helpKeywords = null;

    public string[] HELPKEYWORDS => helpKeywords;
}
