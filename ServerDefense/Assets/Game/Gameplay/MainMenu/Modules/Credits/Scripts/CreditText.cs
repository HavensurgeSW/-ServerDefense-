using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace ServerDefense.Gameplay.MainMenu
{
    public class CreditText : MonoBehaviour
    {
        [SerializeField] private TMP_Text text = null;
        [SerializeField] private Image arrow = null;

        public void SetData(string department, string name)
        {
            text.text = department + ": " + name;
        }

        public void SetArrowStatus(bool status)
        {
            arrow.gameObject.SetActive(status);
        }
    }
}