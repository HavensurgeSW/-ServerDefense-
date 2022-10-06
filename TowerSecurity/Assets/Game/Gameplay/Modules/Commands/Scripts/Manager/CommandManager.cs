using UnityEngine;

public class CommandManager : MonoBehaviour
{
    [SerializeField] private HelpCommandInfo helpInfo = null;

    public bool CheckCommandArguments(string[] args, CommandInfo info)
    {
        if (args != null)
        {
            if (args.Length == info.ARG_COUNT)
            {
                return true;
            }
        }

        if (args == null && info.ARG_COUNT == 0)
        {
            return true;
        }

        return false;
    }

    public bool CheckHelpCommand(string[] args)
    {
        if (args != null && args.Length == 1)
        {
            for (int i = 0; i < helpInfo.HELPKEYWORDS.Length; i++)
            {
                if (args[0] == helpInfo.HELPKEYWORDS[i])
                {
                    return true;
                }
            }
        }

        return false;
    }
}
