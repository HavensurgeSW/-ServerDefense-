using UnityEngine;

namespace ServerDefense.Gameplay.MainMenu
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Credits/CreditDepartmentData", fileName = "CreditData_")]
    public class CreditDepartmentData : ScriptableObject
    {
        [SerializeField] private string department = string.Empty;
        [SerializeField] private string[] staffNames = null;

        public string DEPARTMENT { get => department; }
        public string[] STAFF_NAMES { get => staffNames; }
    }
}