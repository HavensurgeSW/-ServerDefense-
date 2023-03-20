using System.Collections.Generic;

using UnityEngine;

namespace ServerDefense.Systems.Currencies
{
    public class CurrenciesController : MonoBehaviour
    {
        [SerializeField] private List<CurrencySO> currencies = null;

        private Dictionary<string, int> currenciesDictionary = null;

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

        public void AddCurrencyValue(string currencyId, int amount)
        {
            if (currenciesDictionary.ContainsKey(currencyId))
            {
                currenciesDictionary[currencyId] += amount;
                return;
            }

            Debug.LogError("Failed to find currency of Id: " + currencyId);
        }

        public void SubstractCurrencyValue(string currencyId, int amount)
        {
            if (currenciesDictionary.ContainsKey(currencyId))
            {
                currenciesDictionary[currencyId] -= amount;
                return;
            }

            Debug.LogError("Failed to find currency of Id: " + currencyId);
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