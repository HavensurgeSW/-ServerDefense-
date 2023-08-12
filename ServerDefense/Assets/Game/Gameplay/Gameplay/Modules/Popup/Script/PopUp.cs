using System;

using UnityEngine;

using TMPro;

namespace ServerDefense.Gameplay.Gameplay.Modules.PopUps
{
    public class PopUp : MonoBehaviour
    {
        [SerializeField] private TMP_Text locationText = null;
        [SerializeField] private TMP_Text towerDataText = null;

        private Action<PopUp> OnRelease = null;

        public void Init(Action<PopUp> onRelease)
        {
            OnRelease = onRelease;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetLocationIdText(string id)
        {
            locationText.text = id;
        }

        public void ToggleTowerDataText(bool status)
        {
            towerDataText.gameObject.SetActive(status);
        }

        public void SetTowerDataText(string towerId, int level)
        {
            towerDataText.text = towerId + ": " + $"V{level}.0";
        }

        public void Release()
        {
            OnRelease?.Invoke(this);
        }
    }
}