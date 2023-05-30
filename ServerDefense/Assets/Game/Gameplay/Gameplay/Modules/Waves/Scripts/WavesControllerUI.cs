using UnityEngine;
using UnityEngine.UI;

public class WavesControllerUI : MonoBehaviour
{
    [SerializeField] private Image timerBar = null;

    public void SetBarProgress(float value)
    {
        timerBar.fillAmount = value;
    }
}
