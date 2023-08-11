public class CommandDataModel
{
    private string id = string.Empty;
    private string[] arguments = null;

    public string ID => id;
    public string[] ARGUMENTS => arguments;

    public CommandDataModel(string id, string[] arguments) 
    {
        this.id = id;
        this.arguments = arguments;
    }

    public CommandDataModel(string fullArgumentsString)
    {
        id = GetId(fullArgumentsString);
        arguments = GetArguments(fullArgumentsString);
    }

    private string[] GetArguments(string fullArgumentsString)
    {
        string[] arguments = fullArgumentsString.Split(' ');
        string[] commandArgs = new string[arguments.Length - 1];

        for (int i = 1; i <= commandArgs.Length; i++)
        {
            commandArgs[i - 1] = arguments[i];
        }

        return commandArgs;
    }

    private string GetId(string fullArgumentsString)
    {
        string[] arguments = fullArgumentsString.Split(' ');
        return arguments[0];
    }
}
