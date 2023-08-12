using UnityEngine;

namespace ServerDefense.Gameplay.Gameplay.Modules.Commands
{
    [CreateAssetMenu(fileName = "command_help", menuName = "ScriptableObjects/Commands/Help")]
    public class HelpCommandSO : ReturnCommandsCommandSO
    {
        [Header("Help Command Configuration")]
        [SerializeField] private string[] helpKeywords = null;

        public string[] HELP_KEYWORDS => helpKeywords;
    }
}