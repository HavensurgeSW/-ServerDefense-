using System;

using UnityEngine;
using UnityEngine.UI;

namespace ServerDefense.Gameplay.MainMenu
{
    public class CreditsHandler : MonoBehaviour
    {
        [Header("Main Configuration")]
        [SerializeField] private GameObject holder = null;
        [SerializeField] private Button exitButton = null;
        [SerializeField] private Transform creditsDataHolder = null;

        [Header("Credits Configuration")]
        [SerializeField] private CreditDepartmentData[] creditsData = null;
        [SerializeField] private GameObject departmentHolderPrefab = null;
        [SerializeField] private GameObject creditTextPrefab = null;
        [SerializeField] private GameObject logoHolderPrefab = null;

        public void Init(Action onSwitchToMenu)
        {
            exitButton.onClick.AddListener(() => onSwitchToMenu?.Invoke());

            LoadCredits();
        }

        public void ToggleStatus(bool status)
        {
            holder.SetActive(status);
        }

        private void LoadCredits()
        {
            for (int i = 0; i < creditsData.Length; i++)
            {
                Transform holder = Instantiate(departmentHolderPrefab, creditsDataHolder).transform;

                for (int j = 0; j < creditsData[i].STAFF_NAMES.Length; j++)
                {
                    CreditText credit = Instantiate(creditTextPrefab, holder).GetComponent<CreditText>();
                    credit.SetData(creditsData[i].DEPARTMENT, creditsData[i].STAFF_NAMES[j]);

                    bool isFirstNameFromDepartment = j == 0;
                    credit.SetArrowStatus(isFirstNameFromDepartment);
                }
            }

            Transform logoholder = Instantiate(logoHolderPrefab, creditsDataHolder).transform;
        }
    }
}