using UnityEngine;

namespace ServerDefense.Systems.Currencies
{
    [CreateAssetMenu(fileName = "currency_", menuName = "ScriptableObjects/Currencies/Currency")]
    public class CurrencySO : ScriptableObject
    {
        [SerializeField] private string currencyId = string.Empty;

        public string CURRENCY_ID => currencyId;
    }
}