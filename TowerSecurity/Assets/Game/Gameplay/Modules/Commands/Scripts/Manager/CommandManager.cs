using UnityEngine;

public class CommandManager : MonoBehaviour
{
    [SerializeField] private HelpCommandInfo helpInfo = null;

    public bool CheckCommandArguments(string[] args, int argumentCount)
    {
        if (args == null && argumentCount == 0)
        {
            return true;
        }

        if (args == null && argumentCount > 0)
        {
            return false;
        }

        if (args.Length == argumentCount)
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
