using System;
using ServerDefense.Gameplay.Gameplay.Modules.Commands;
using ServerDefense.Gameplay.Gameplay.Modules.Terminal;

namespace ServerDefense.Gameplay.Gameplay
{
    public interface IHelpCommandResponder
    {
        public TerminalResponseSO HelpResponse { get; }

        public void TriggerHelpResponse(CommandManagerModel commandManagerModel, Action<TerminalResponseSO> onTriggerMessage)
        {
            onTriggerMessage?.Invoke(HelpResponse);
        }
    }
}