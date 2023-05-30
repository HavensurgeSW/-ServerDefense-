using UnityEngine;

using System.Collections.Generic;

namespace ServerDefense.Common.Currencies
{
    public class CurrenciesController : MonoBehaviour
    {
        [Header("Main Configuration")]
        [SerializeField] private List<CurrencySO> currencies = null;

        private Dictionary<string, int> currenciesDictionary = null;

        private const int invalidValue = -1;

        public void Init()
        {
            currenciesDictionary = new Dictionary<string, int>();

            for (int i = 0; i < currencies.Count; i++)
            {
                currenciesDictionary.Add(currencies[i].CURRENCY_ID, 0);
            }
        }

        public CurrencySO GetCurrency(string currencyId)
        {
            for (int i = 0; i < currencies.Count; i++)
            {
                if (currencies[i].CURRENCY_ID == currencyId)
                {
                    return currencies[i];
                }
            }

            Debug.LogError("Failed to find currency of Id: " + currencyId);
            return null;
        }

        public void SetCurrencyValue(string currencyId, int amount)
        {
            if (currenciesDictionary.ContainsKey(currencyId))
            {
                currenciesDictionary[currencyId] = amount;
                return;
            }

            Debug.LogError("Failed to find currency of Id: " + currencyId);
        }

        public int AddCurrencyValue(string currencyId, int amount)
        {
            if (currenciesDictionary.ContainsKey(currencyId))
            {
                currenciesDictionary[currencyId] += amount;
                return currenciesDictionary[currencyId];
            }

            Debug.LogError("Failed to find currency of Id: " + currencyId);
            return invalidValue;
        }

        public int SubstractCurrencyValue(string currencyId, int amount)
        {
            if (currenciesDictionary.ContainsKey(currencyId))
            {
                currenciesDictionary[currencyId] -= amount;
                return currenciesDictionary[currencyId];
            }

            Debug.LogError("Failed to find currency of Id: " + currencyId);
            return invalidValue;
        }

        public int GetCurrencyValue(string currencyId)
        {
            if (currenciesDictionary.ContainsKey(currencyId))
            {
                return currenciesDictionary[currencyId];
            }

            Debug.LogError("Failed to find currency of Id: " + currencyId);
            return -1;
        }
    }
}