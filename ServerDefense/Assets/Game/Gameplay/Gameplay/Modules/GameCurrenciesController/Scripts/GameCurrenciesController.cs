using UnityEngine;

using ServerDefense.Common.Currencies;

public class GameCurrenciesController : CurrenciesController
{
    [Header("Responses Configuration")]
    [SerializeField] private TerminalResponseSO insufficientCurrencyResponse = null;

    public TerminalResponseSO GetInsufficientCurrencyResponse()
    {
        return insufficientCurrencyResponse;
    }
}
