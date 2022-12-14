using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSFXUtils : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] Slider axSlider;
    [SerializeField] Slider mxSlider;
    [SerializeField] Slider tmlSlider;
    [SerializeField] Slider wldSlider;

    [Header("Bus names")]
    [SerializeField]string busAX = null;
    [SerializeField]string busMX = null;
    [SerializeField]string busTerminal = null;
    [SerializeField]string busWorld = null;

    public void SetAudioLevels() {
        AkSoundEngine.SetRTPCValue(busAX, axSlider.value);
        AkSoundEngine.SetRTPCValue(busMX, mxSlider.value);
        AkSoundEngine.SetRTPCValue(busTerminal, tmlSlider.value);
        AkSoundEngine.SetRTPCValue(busWorld, wldSlider.value);

        PlayerPrefs.SetFloat(busAX, axSlider.value);
        PlayerPrefs.SetFloat(busMX, axSlider.value);
        PlayerPrefs.SetFloat(busTerminal, axSlider.value);
        PlayerPrefs.SetFloat(busWorld, axSlider.value);
    
    }

    public void LoadAudioLevelsFromPrefs() {
        AkSoundEngine.SetRTPCValue(busAX, PlayerPrefs.GetFloat(busAX));
        AkSoundEngine.SetRTPCValue(busMX, PlayerPrefs.GetFloat(busMX));
        AkSoundEngine.SetRTPCValue(busTerminal, PlayerPrefs.GetFloat(busTerminal));
        AkSoundEngine.SetRTPCValue(busWorld, PlayerPrefs.GetFloat(busWorld));
    }
}
