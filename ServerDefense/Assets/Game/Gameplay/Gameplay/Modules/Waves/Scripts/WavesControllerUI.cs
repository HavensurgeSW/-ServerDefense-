using UnityEngine;
using UnityEngine.UI;

namespace ServerDefense.Gameplay.Gameplay.Modules.Waves
{
    public class WavesControllerUI : MonoBehaviour
    {
        [SerializeField] private Image timerBar = null;

        public void SetBarProgress(float value)
        {
            timerBar.fillAmount = value;
        }
    }
}