using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "command_quit", menuName = "ScriptableObjects/Commands/Quit")]
public class QuitCommandSO : CommandSO
{
    [Header("Quit Command Configuration")]
    [SerializeField] private string[] quitKeywords = null;
    [SerializeField] private SCENE targetScene = SCENE.NONE;

    public override void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<List<string>> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
    {
        string keyword = arguments[0];

        for (int i = 0; i < quitKeywords.Length; i++)
        {
            if (keyword == quitKeywords[i])
            {
                onTriggerMessage(successResponse);
                onSuccess(this);
                SceneManager.LoadScene((int)targetScene);
                break;
            }
        }

        onTriggerMessage(errorResponse);
        onFailure(this);
    }
}
