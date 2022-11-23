using System;

using UnityEngine;
using UnityEngine.UI;

using static CommonUtils;

public class MenuHandler : MonoBehaviour
{
    [Header("Main Configuration")]
    [SerializeField] private GameObject holder = null;
    [SerializeField] private Button startGameButton = null;
    [SerializeField] private Button tutorialButton = null;
    [SerializeField] private Button creditsButton = null;
    [SerializeField] private Button exitButton = null;

    public void Init(Action onSwitchToCredits, Action<SCENE, bool> onChangeScene, Action onExitGame)
    {
        startGameButton.onClick.AddListener(() => onChangeScene?.Invoke(SCENE.TOWER_DEFENSE, true));
        tutorialButton.onClick.AddListener(() => onChangeScene?.Invoke(SCENE.TUTORIAL, true));
        creditsButton.onClick.AddListener(() => onSwitchToCredits?.Invoke());
        exitButton.onClick.AddListener(() => onExitGame?.Invoke());
    }

    public void ToggleStatus(bool status)
    {
        holder.SetActive(status);
    }
}
