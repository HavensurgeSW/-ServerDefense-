using System;

using UnityEngine;

using ServerDefense.Gameplay.Gameplay.Modules.Terminal;
using ServerDefense.Gameplay.Gameplay.Modules.Waves;

namespace ServerDefense.Gameplay.Gameplay.Modules.Commands
{
    [CreateAssetMenu(fileName = "command_network", menuName = "ScriptableObjects/Commands/Network")]
    public class NetworkCommandSO : CommandSO
    {
        [Header("Network Command Configuration")]
        [SerializeField] private string initId = string.Empty;

        public override void TriggerCommand(CommandManagerModel commandManagerModel, string[] arguments, Action<TerminalResponseSO> onTriggerMessage, Action<CommandSO> onSuccess, Action<CommandSO> onFailure)
        {
            string keyword = arguments[0];

            if (keyword == initId)
            {
                WavesController wavesController = commandManagerModel.WAVES_CONTROLLER;
                wavesController.StartWave(wavesController.CURRENT_WAVE_INDEX, null);

                //wavesController.StartWave(index, null);
                //
                //packetsHandler.StartPacketsWave();

                onTriggerMessage(successResponse);
                onSuccess(this);
                return;
            }

            onTriggerMessage(errorResponse);
            onFailure(this);
        }
    }
}