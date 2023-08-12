using UnityEngine;

using ServerDefense.Common.Currencies;

using ServerDefense.Gameplay.Gameplay.Modules.Terminal;

namespace ServerDefense.Gameplay.Gameplay.Modules.Currencies
{
    public class GameCurrenciesController : CurrenciesController
    {
        [Header("Responses Configuration")]
        [SerializeField] private TerminalResponseSO insufficientCurrencyResponse = null;

        public TerminalResponseSO GetInsufficientCurrencyResponse()
        {
            return insufficientCurrencyResponse;
        }
    }
}