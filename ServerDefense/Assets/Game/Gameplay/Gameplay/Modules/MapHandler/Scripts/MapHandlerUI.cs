using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Pool;

using ServerDefense.Gameplay.Gameplay.Modules.PopUps;

namespace ServerDefense.Gameplay.Gameplay.Modules.Map
{
    public class MapHandlerUI : MonoBehaviour
    {
        [Header("Popups Configuration")]
        [SerializeField] private Transform popUpsHolder = null;
        [SerializeField] private GameObject popUpPrefab = null;
        [SerializeField] private Vector2 offset = Vector2.zero;

        private ObjectPool<PopUp> popUpPool = null;
        private List<PopUp> activePopUps = null;
        private Camera mainCamera = null;

        public void Init()
        {
            mainCamera = Camera.main;

            activePopUps = new List<PopUp>();
            popUpPool = new ObjectPool<PopUp>(CreatePopUp, GetPopUp, ReleasePopUp);
        }

        public void GeneratePopUp(Location location)
        {
            GeneratePopUp(location.ID, mainCamera.WorldToScreenPoint(location.transform.position));
        }

        public void GeneratePopUp(string id, Vector3 position)
        {
            PopUp popUp = popUpPool.Get();
            activePopUps.Add(popUp);
            popUp.SetPosition(position + (Vector3)offset);
            popUp.SetLocationIdText(id);
            popUp.ToggleTowerDataText(false);
        }

        public void ClearAllPopUps()
        {
            for (int i = 0; i < activePopUps.Count; i++)
            {
                popUpPool.Release(activePopUps[i]);
            }

            activePopUps.Clear();
        }

        private void RemovePopUp(PopUp item)
        {
            item.Release();

            if (activePopUps.Contains(item))
            {
                activePopUps.Remove(item);
            }
        }

        private PopUp CreatePopUp()
        {
            PopUp item = Instantiate(popUpPrefab, popUpsHolder).GetComponent<PopUp>();
            item.Init(RemovePopUp);
            return item;
        }

        private void GetPopUp(PopUp item)
        {
            item.gameObject.SetActive(true);
        }

        private void ReleasePopUp(PopUp item)
        {
            item.gameObject.SetActive(false);
        }
    }
}